using AutoMapper;
using Proxy.Domain;
using Proxy.Service.Queries.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Mappers
{
    public class ResponseMapper : Profile
    {

        public ResponseMapper()
        {
            CreateMap<Response, ResponseDataTransfer>();
        }

    }
}
