using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proxy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Persistence.Database.Configuration
{
    public class GeneralSettingsConfiguration
    {

        public static void SetEntityBuilder(EntityTypeBuilder<GeneralSettings> entityBuilder)
        {

            entityBuilder
                .HasKey(x => x.Id);

            IList<GeneralSettings> initialGeneralSettings = new List<GeneralSettings>
            {
                new GeneralSettings()
                {
                    Id = Guid.NewGuid(),
                    MaxRequestsByEndpoint = 300,
                    MaxRequestsByIP = 300
                }
            };

            entityBuilder
                .HasData(initialGeneralSettings);

        }

    }
}
