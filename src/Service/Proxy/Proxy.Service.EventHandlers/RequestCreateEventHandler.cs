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
    public class RequestCreateEventHandler : IRequestHandler<RequestCreateCommand, IdentityResult>
    {

        private readonly ProxyDbContext _proxyDbContext;

        public RequestCreateEventHandler(ProxyDbContext proxyDbContext)
        {
            _proxyDbContext = proxyDbContext;
        }

        public async Task<IdentityResult> Handle(RequestCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Request _request = new Request()
                {
                    Id = request.Id,
                    Body = request.Body,
                    ClientIP = request.ClientIP,
                    Endpoint = request.Endpoint,
                    RequestDateTime = DateTime.Now
                };
                await _proxyDbContext.Request.AddAsync(_request, cancellationToken);
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
