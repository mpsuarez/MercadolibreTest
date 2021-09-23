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
    public class SettingsByIPQueryService : ISettingsByIPQueryService
    {

        private readonly ProxyDbContext _proxyDbContext;
        private readonly IMapper _mapper;

        public SettingsByIPQueryService(ProxyDbContext proxyDbContext, IMapper mapper)
        {
            _proxyDbContext = proxyDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SettingsByIPDataTransfer>> GetAllAsync()
        {
            IEnumerable<SettingsByIP> settingsByIP = await _proxyDbContext.SettingsByIP.ToListAsync();
            return _mapper.Map<IEnumerable<SettingsByIP>, IEnumerable<SettingsByIPDataTransfer>>(settingsByIP);
        }

        public async Task<SettingsByIPDataTransfer> GetAsync(Guid Id)
        {
            SettingsByIP settingsByIP = await _proxyDbContext.SettingsByIP.SingleOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<SettingsByIP, SettingsByIPDataTransfer>(settingsByIP);
        }

        public async Task<SettingsByIPDataTransfer> GetAsync(string IPAdress)
        {
            SettingsByIP settingsByIP = await _proxyDbContext.SettingsByIP.SingleOrDefaultAsync(x => x.IPAdress == IPAdress);
            return _mapper.Map<SettingsByIP, SettingsByIPDataTransfer>(settingsByIP);
        }

    }
}
