using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.EventHandlers.Commands
{
    public class RequestCreateCommand : IRequest<IdentityResult>
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string ClientIP { get; set; }

        public string Endpoint { get; set; }

        public string Body { get; set; }

    }
}
