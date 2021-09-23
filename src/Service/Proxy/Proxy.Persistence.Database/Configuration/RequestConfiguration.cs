using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proxy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Persistence.Database.Configuration
{
    public class RequestConfiguration
    {

        public static void SetEntityBuilder(EntityTypeBuilder<Request> entityBuilder)
        {

            entityBuilder
                .HasKey(x => x.Id);

            entityBuilder
                .HasOne(x => x.Response)
                .WithOne(x => x.Request)
                .HasForeignKey<Response>(x => x.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
