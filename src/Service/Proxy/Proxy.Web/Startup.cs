using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proxy.Mappers;
using Proxy.Persistence.Database;
using Proxy.Service.Queries;
using Proxy.Service.Queries.QueryServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Proxy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ProxyDbContext>(conf =>
            {
                conf.UseSqlServer(Configuration.GetConnectionString("ProxyConnection")).UseLazyLoadingProxies();
            });

            services.AddMediatR(Assembly.Load("Proxy.Service.EventHandlers"));

            services.AddHealthChecks().AddDbContextCheck<ProxyDbContext>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new RequestMapper());
                config.AddProfile(new ResponseMapper());
                config.AddProfile(new GeneralSettingsMapper());
            });

            services.AddTransient<IRequestQueryService, RequestQueryService>();
            services.AddTransient<IResponseQueryService, ResponseQueryService>();
            services.AddTransient<IGeneralSettingsQueryService, GeneralSettingsQueryService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
