using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain
{
    public class GeneralSettings
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public int MaxRequestsByIP { get; set; }

        public int MaxRequestsByEndpoint { get; set; }

    }
}
