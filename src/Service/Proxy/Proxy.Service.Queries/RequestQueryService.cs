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
    public class RequestQueryService : IRequestQueryService
    {

        private readonly ProxyDbContext _proxyDbContext;
        private readonly IMapper _mapper;

        public RequestQueryService(ProxyDbContext proxyDbContext, IMapper mapper)
        {
            _proxyDbContext = proxyDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RequestDataTransfer>> GetAllAsync()
        {
            IEnumerable<Request> requests = await _proxyDbContext.Request.ToListAsync();
            return _mapper.Map<IEnumerable<Request>, IEnumerable<RequestDataTransfer>>(requests);
        }

        public async Task<RequestDataTransfer> GetAsync(Guid Id)
        {
            Request request = await _proxyDbContext.Request.SingleOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<Request, RequestDataTransfer>(request);
        }

    }
}
