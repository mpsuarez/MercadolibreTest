using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.EventHandlers.Commands
{
    public class AllowAccessCommand : IRequest<IdentityResult>
    {

        public string IPAdress { get; set; }

        public string Endpoint { get; set; }

    }
}
