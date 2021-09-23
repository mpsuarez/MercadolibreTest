using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.Queries.DataTransferObjects
{
    public class RequestDataTransfer
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string ClientIP { get; set; }

        public string Endpoint { get; set; }

        public string Body { get; set; }

        public DateTime RequestDateTime { get; set; } = DateTime.Now;

        public ResponseDataTransfer Response { get; set; }

    }
}
