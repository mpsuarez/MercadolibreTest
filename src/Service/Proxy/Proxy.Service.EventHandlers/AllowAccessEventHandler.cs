using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proxy.Domain;
using Proxy.Persistence.Database;
using Proxy.Service.EventHandlers.Commands;
using Proxy.Service.Queries.DataTransferObjects;
using Proxy.Service.Queries.QueryServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy.Service.EventHandlers
{
    public class AllowAccessEventHandler : IRequestHandler<AllowAccessCommand, IdentityResult>
    {

        private readonly ProxyDbContext _proxyDbContext;
        private readonly IGeneralSettingsQueryService _generalSettingsQueryService;
        private readonly IMediator _mediator;

        public AllowAccessEventHandler(ProxyDbContext proxyDbContext, IGeneralSettingsQueryService generalSettingsQueryService, IMediator mediator)
        {
            _proxyDbContext = proxyDbContext;
            _generalSettingsQueryService = generalSettingsQueryService;
            _mediator = mediator;

        }

        public async Task<IdentityResult> Handle(AllowAccessCommand request, CancellationToken cancellationToken)
        {
            try
            {

                GeneralSettingsDataTransfer generalSettingsDataTransfer = await _generalSettingsQueryService.GetAsync();
                IList<IdentityError> identityErrors = new List<IdentityError>();

                SettingsByIP settingsByIP = await _proxyDbContext.SettingsByIP.SingleOrDefaultAsync(x => x.IPAdress == request.IPAdress);
                if(settingsByIP == null)
                {

                    SettingsByIP newSettingByIP = new SettingsByIP()
                    {
                        Id = Guid.NewGuid(),
                        IPAdress = request.IPAdress,
                        NumberOfRequestById = 0,
                        MaxRequestsByIP = generalSettingsDataTransfer.MaxRequestsByIP
                    };

                    await _proxyDbContext.SettingsByIP.AddAsync(newSettingByIP, cancellationToken);
                    await _proxyDbContext.SaveChangesAsync(cancellationToken);
                    settingsByIP = newSettingByIP;
                }

                SettingsByEndpoint settingsByEndpoint = await _proxyDbContext.SettingsByEndpoint.SingleOrDefaultAsync(x => x.Endpoint == request.Endpoint);
                if(settingsByEndpoint == null)
                {

                    SettingsByEndpoint newSettingsByEndpoint = new SettingsByEndpoint()
                    {
                        Id = Guid.NewGuid(),
                        Endpoint = request.Endpoint,
                        NumberOfRequestByEndpoint = 0,
                        MaxRequestsByEndpoint = generalSettingsDataTransfer.MaxRequestsByEndpoint
                    };

                    await _proxyDbContext.SettingsByEndpoint.AddAsync(newSettingsByEndpoint, cancellationToken);
                    await _proxyDbContext.SaveChangesAsync(cancellationToken);
                    settingsByEndpoint = newSettingsByEndpoint;
                }

                if (settingsByIP.NumberOfRequestById == settingsByIP.MaxRequestsByIP)
                {
                    identityErrors.Add(new IdentityError() { Description = "Numero de peticiones permitidas de IP agotado" });
                }

                if (settingsByEndpoint.NumberOfRequestByEndpoint == settingsByEndpoint.MaxRequestsByEndpoint)
                {
                    identityErrors.Add(new IdentityError() { Description = "Numero de peticiones permitidas al Endpoint agotado" });
                }

                if (identityErrors.Count > 0)
                {
                    return IdentityResult.Failed(identityErrors.ToArray());
                }

                settingsByEndpoint.NumberOfRequestByEndpoint++;
                settingsByIP.NumberOfRequestById++;

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
