using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(
                c =>
                {
                    c.DescribeAllEnumsAsStrings();
                    c.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "BYS API",
                            Version = "v1",
                            Description = "ASP.NET Core Web API 2",
                            TermsOfService = "None",
                            Contact = new Contact
                            {
                                Name = "Tran Dat",
                                Email = "dat.tran@bys.vn",
                                Url = "dat.tran@bys.vn"
                            },
                            License = new License
                            {
                                Name = "Copyright by BYS JSC",
                                Url = "https://wwww.bys.vn"
                            }
                        });

                    c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                    //var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Bys.Miracle.WebApi.xml");
                    //c.IncludeXmlComments(filePath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure Cors
            app.UseCors(
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                c =>
                {
                    c.DocumentTitle("BYS API Document");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
