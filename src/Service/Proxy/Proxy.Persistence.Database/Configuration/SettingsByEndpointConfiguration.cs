using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proxy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Persistence.Database.Configuration
{
    public class SettingsByEndpointConfiguration
    {

        public static void SetEntityBuilder(EntityTypeBuilder<SettingsByEndpoint> entityBuilder)
        {

            entityBuilder
                .HasKey(x => x.Id);

        }

    }
}
