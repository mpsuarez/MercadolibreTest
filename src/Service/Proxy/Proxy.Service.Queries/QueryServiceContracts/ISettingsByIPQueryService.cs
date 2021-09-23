using Proxy.Service.Queries.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries.QueryServiceContracts
{
    public interface ISettingsByIPQueryService
    {

        Task<IEnumerable<SettingsByIPDataTransfer>> GetAllAsync();

        Task<SettingsByIPDataTransfer> GetAsync(Guid Id);

        Task<SettingsByIPDataTransfer> GetAsync(string IPAdress);

    }
}
