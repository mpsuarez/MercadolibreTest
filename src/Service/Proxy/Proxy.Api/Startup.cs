using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Proxy.Api.Middleware;
using Proxy.Mappers;
using Proxy.Persistence.Database;
using Proxy.Service.Queries;
using Proxy.Service.Queries.QueryServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Proxy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ProxyDbContext>(conf =>
            {
                conf.UseSqlServer(Configuration.GetConnectionString("ProxyConnection")).UseLazyLoadingProxies();
            });

            services.AddMediatR(Assembly.Load("Proxy.Service.EventHandlers"));

            services.AddHealthChecks().AddDbContextCheck<ProxyDbContext>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new RequestMapper());
                config.AddProfile(new ResponseMapper());
                config.AddProfile(new GeneralSettingsMapper());
            });

            services.AddTransient<IRequestQueryService, RequestQueryService>();
            services.AddTransient<IResponseQueryService, ResponseQueryService>();
            services.AddTransient<IGeneralSettingsQueryService, GeneralSettingsQueryService>();


            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("192.168.0.251"));
            });

            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proxy.Api", Version = "v1" });
            //});

            services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proxy.Api v1"));
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMercadoLibreProxyMiddleware();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("HealthCheck", new HealthCheckOptions()
            //    {
            //        Predicate = _ => true,
            //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //    });
            //    endpoints.MapControllers();
            //});
        }
    }
}
