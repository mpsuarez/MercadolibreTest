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
    public class GeneralSettingsMapper : Profile
    {

        public GeneralSettingsMapper()
        {
            CreateMap<GeneralSettings, GeneralSettingsDataTransfer>();
        }

    }
}
