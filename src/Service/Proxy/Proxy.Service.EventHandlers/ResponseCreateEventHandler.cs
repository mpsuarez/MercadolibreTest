using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proxy.Domain;
using Proxy.Persistence.Database;
using Proxy.Service.EventHandlers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy.Service.EventHandlers
{
    public class ResponseCreateEventHandler : IRequestHandler<ResponseCreateCommand, IdentityResult>
    {

        private readonly ProxyDbContext _proxyDbContext;

        public ResponseCreateEventHandler(ProxyDbContext proxyDbContext)
        {
            _proxyDbContext = proxyDbContext;
        }

        public async Task<IdentityResult> Handle(ResponseCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Response response = new Response()
                {
                    Id = request.Id,
                    Body = request.Body,
                    ClientIP = request.ClientIP,
                    Endpoint = request.Endpoint,
                    ResponseDateTime = DateTime.Now,
                    StatusCode = request.StatusCode,
                    RequestId = request.RequestId
                };
                await _proxyDbContext.Response.AddAsync(response, cancellationToken);
                await _proxyDbContext.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }
            catch (DbUpdateException ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.InnerException.Message });
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }
    }
}
