using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain
{
    public class SettingsByEndpoint
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Endpoint { get; set; }

        public int NumberOfRequestByEndpoint { get; set; }

        public int MaxRequestsByEndpoint { get; set; }

    }
}
