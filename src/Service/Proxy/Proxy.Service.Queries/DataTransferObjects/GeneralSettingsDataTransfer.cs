using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries.DataTransferObjects
{
    public class GeneralSettingsDataTransfer
    {

        public int MaxRequestsByIP { get; set; }

        public int MaxRequestsByEndpoint { get; set; }

    }
}
