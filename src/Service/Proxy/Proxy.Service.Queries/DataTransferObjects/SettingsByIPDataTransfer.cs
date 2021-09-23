using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries.DataTransferObjects
{
    public class SettingsByIPDataTransfer
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string IPAdress { get; set; }

        public int NumberOfRequestById { get; set; }

        public int MaxRequestsByIP { get; set; }

    }
}
