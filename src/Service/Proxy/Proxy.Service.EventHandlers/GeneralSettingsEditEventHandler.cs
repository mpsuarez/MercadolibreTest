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
    public class GeneralSettingsEditEventHandler : IRequestHandler<GeneralSettingsEditCommand, IdentityResult>
    {

        private readonly ProxyDbContext _proxyDbContext;

        public GeneralSettingsEditEventHandler(ProxyDbContext proxyDbContext)
        {
            _proxyDbContext = proxyDbContext;
        }

        public async Task<IdentityResult> Handle(GeneralSettingsEditCommand request, CancellationToken cancellationToken)
        {
            try
            {
                GeneralSettings generalSettings = await _proxyDbContext.GeneralSettings.SingleAsync();
                generalSettings.MaxRequestsByEndpoint = request.MaxRequestsByEndpoint;
                generalSettings.MaxRequestsByIP = request.MaxRequestsByIP;
                _proxyDbContext.Update(generalSettings);
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
