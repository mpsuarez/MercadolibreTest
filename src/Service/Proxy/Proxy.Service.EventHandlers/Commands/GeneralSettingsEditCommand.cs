using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Service.EventHandlers.Commands
{
    public class GeneralSettingsEditCommand : IRequest<IdentityResult>
    {

        public int MaxRequestsByIP { get; set; }

        public int MaxRequestsByEndpoint { get; set; }

    }
}
