using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Proxy.Domain;
using Proxy.Persistence.Database;
using Proxy.Service.EventHandlers.Commands;
using Proxy.Service.Queries.DataTransferObjects;
using Proxy.Service.Queries.QueryServiceContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Api.Middleware
{
    public class MercadoLibreProxyMiddleware
    {

        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly RequestDelegate _next;

        public MercadoLibreProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IMediator mediator)
        {

            AllowAccessCommand allowAccessCommand = new AllowAccessCommand()
            {
                Endpoint = httpContext.Request.Path,
                IPAdress = httpContext.Connection.RemoteIpAddress.ToString()
            };

            IdentityResult allowAccess = await mediator.Send(allowAccessCommand);
            if (allowAccess.Succeeded)
            {
                Uri targetUri = new Uri("https://api.mercadolibre.com" + httpContext.Request.Path);

                if (targetUri != null)
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage();

                    //Cabezera y cuerpo por metodo
                    string requestMethod = httpContext.Request.Method;

                    if (
                        !HttpMethods.IsGet(requestMethod) &&
                        !HttpMethods.IsHead(requestMethod) &&
                        !HttpMethods.IsDelete(requestMethod) &&
                        !HttpMethods.IsTrace(requestMethod)
                    )
                    {
                        StreamContent streamContent = new StreamContent(httpContext.Request.Body);
                        requestMessage.Content = streamContent;
                    }

                    foreach (var header in httpContext.Request.Headers)
                    {
                        requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                    }

                    requestMessage.RequestUri = targetUri;
                    requestMessage.Headers.Host = targetUri.Host;

                    //Asignar el httpmethod

                    HttpMethod requestHttpMethod;

                    if (HttpMethods.IsGet(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Get;
                    }
                    else if (HttpMethods.IsPost(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Post;
                    }
                    else if (HttpMethods.IsPut(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Put;
                    }
                    else if (HttpMethods.IsDelete(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Delete;
                    }
                    else if (HttpMethods.IsOptions(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Options;
                    }
                    else if (HttpMethods.IsTrace(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Trace;
                    }
                    else if (HttpMethods.IsHead(requestMethod))
                    {
                        requestHttpMethod = HttpMethod.Head;
                    }
                    else
                    {
                        requestHttpMethod = new HttpMethod(requestMethod);
                    }

                    requestMessage.Method = requestHttpMethod;

                    //Respuesta
                    using (HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, httpContext.RequestAborted))
                    {

                        httpContext.Response.StatusCode = (int)responseMessage.StatusCode;

                        foreach (var header in responseMessage.Headers)
                        {
                            httpContext.Response.Headers[header.Key] = header.Value.ToArray();
                        }

                        foreach (var header in responseMessage.Content.Headers)
                        {
                            httpContext.Response.Headers[header.Key] = header.Value.ToArray();
                        }
                        httpContext.Response.Headers.Remove("transfer-encoding");

                        //Poder leer memorystream 2 veces 
                        MemoryStream memoryStream = new MemoryStream();
                        await responseMessage.Content.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        await memoryStream.CopyToAsync(httpContext.Response.Body);
                        memoryStream.Position = 0;

                        string RequestBody;

                        using (StreamReader requestReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, false))
                        {
                            RequestBody = await requestReader.ReadToEndAsync();
                        }

                        string ResponseBody;

                        using (StreamReader requestReader = new StreamReader(memoryStream, Encoding.UTF8, false))
                        {
                            ResponseBody = await requestReader.ReadToEndAsync();
                        }

                        RequestCreateCommand request = new RequestCreateCommand()
                        {
                            ClientIP = httpContext.Connection.RemoteIpAddress.ToString(),
                            Endpoint = targetUri.AbsoluteUri,
                            Body = RequestBody
                        };

                        await mediator.Send(request);

                        ResponseCreateCommand response = new ResponseCreateCommand()
                        {
                            ClientIP = httpContext.Connection.RemoteIpAddress.ToString(),
                            Endpoint = targetUri.AbsoluteUri,
                            Body = ResponseBody,
                            StatusCode = httpContext.Response.StatusCode,
                            RequestId = request.Id
                        };

                        await mediator.Send(response);

                    }

                }
            }

            //evitar la exepcion que me estaba dando por "Razones"
            if (!httpContext.Response.HasStarted)
            {
                await _next(httpContext);
            }
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MercadoLibreProxyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMercadoLibreProxyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MercadoLibreProxyMiddleware>();
        }
    }

}
