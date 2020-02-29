using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.API.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Course.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Course.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                    {
                        options.ReturnHttpNotAcceptable = true;
                    }).AddNewtonsoftJson(setupAction =>
                    {
                        setupAction.SerializerSettings.ContractResolver =
                           new CamelCasePropertyNamesContractResolver();
                    })
                    .AddXmlDataContractSerializerFormatters()
                    .ConfigureApiBehaviorOptions(setupAction =>
                    {
                        setupAction.InvalidModelStateResponseFactory = context =>
                        {
                            var problemDetails = new ValidationProblemDetails(context.ModelState)
                            {
                                Type = "https://courselibrary.com/modelvalidationproblem",
                                Title = "One or more model validation errors occurred.",
                                Status = StatusCodes.Status422UnprocessableEntity,
                                Detail = "See the errors property for details.",
                                Instance = context.HttpContext.Request.Path
                            };

                            problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        };
                    });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
                {
                    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CourseLibraryDB;Trusted_Connection=True;");
                }

            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
