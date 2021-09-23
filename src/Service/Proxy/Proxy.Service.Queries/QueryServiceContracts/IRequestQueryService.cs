using Proxy.Domain;
using Proxy.Service.Queries.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries.QueryServiceContracts
{
    public interface IRequestQueryService
    {

        Task<IEnumerable<RequestDataTransfer>> GetAllAsync();

        Task<RequestDataTransfer> GetAsync(Guid Id);

    }
}
