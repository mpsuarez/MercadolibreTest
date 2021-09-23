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
    public class ResponseQueryService : IResponseQueryService
    {

        private readonly ProxyDbContext _proxyDbContext;
        private readonly IMapper _mapper;

        public ResponseQueryService(ProxyDbContext proxyDbContext, IMapper mapper)
        {
            _proxyDbContext = proxyDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseDataTransfer>> GetAllAsync()
        {
            IEnumerable<Response> responses = await _proxyDbContext.Response.ToListAsync();
            return _mapper.Map<IEnumerable<Response>, IEnumerable<ResponseDataTransfer>>(responses);
        }

        public async Task<ResponseDataTransfer> GetAsync(Guid Id)
        {
            Response response = await _proxyDbContext.Response.SingleOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<Response, ResponseDataTransfer>(response);
        }
    }
}
