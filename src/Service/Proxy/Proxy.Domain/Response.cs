using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain
{
    public class Response
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string ClientIP { get; set; }

        public string Endpoint { get; set; }

        public string Body { get; set; }

        public int StatusCode { get; set; }

        public DateTime ResponseDateTime { get; set; } = DateTime.Now;

        public Guid RequestId { get; set; }

        public virtual Request Request { get; set; }

    }
}
