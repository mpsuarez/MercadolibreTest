using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Proxy.Domain;
using Proxy.Persistence.Database;
using Proxy.Service.Queries.DataTransferObjects;
using Proxy.Service.Queries.QueryServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries
{
    public class GeneralSettingsQueryService : IGeneralSettingsQueryService
    {

        private readonly ProxyDbContext _proxyDbContext;
        private readonly IMapper _mapper;

        public GeneralSettingsQueryService(ProxyDbContext proxyDbContext, IMapper mapper)
        {
            _proxyDbContext = proxyDbContext;
            _mapper = mapper;
        }

        public async Task<GeneralSettingsDataTransfer> GetAsync()
        {
            GeneralSettings generalSettings = await _proxyDbContext.GeneralSettings.SingleOrDefaultAsync();
            return _mapper.Map<GeneralSettings, GeneralSettingsDataTransfer>(generalSettings);
        }

    }
}
