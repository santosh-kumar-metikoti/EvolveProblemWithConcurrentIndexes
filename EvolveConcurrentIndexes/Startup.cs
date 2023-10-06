using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EvolveConcurrentIndexes
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
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            try
            {
                IDbConnection cnx = new NpgsqlConnection("User ID=evolvetest;Password=evolvetest;server=localhost;Port=5434;Database=evolvetestdb; Timeout = 0");

                Regex regex = new Regex(@"successfully.*v\d*__.*\.sql.*");

                var evolve = new Evolve.Evolve(cnx, msg =>
                {
                })
                {
                    OutOfOrder = true,
                    Locations = new List<string> { "Migrations" },
                    IsEraseDisabled = true,
                    CommandTimeout = 0
                };

                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:- " + ex.Message);
                throw;
            }
        }
    }
}