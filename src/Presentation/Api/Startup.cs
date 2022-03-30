using Boyner.CaseStudy.Infrastructure.Data;
using Boyner.CaseStudy.Infrastructure.MediatR;
using Boyner.CaseStudy.Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boyner.CaseStudy.Presentation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {   services.AddCors(o => o.AddPolicy("CorePolicy", builder =>
{
    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
}));
            services.AddRabbitMQ(Configuration);
            services.AddMediatR(Configuration);
            services.AddDatabase(Configuration);
            services.AddControllers().AddJsonOptions(options =>
            {
                // options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Boyner.CaseStudy.Presentation.Api", Version = "v1" });
            });
         
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boyner.CaseStudy.Presentation.Api v1"));
                app.UseCors("CorePolicy");
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.Use(async (context, next) =>
            //{
            //    await next();

            //    if (context.Response.StatusCode == 404 && !context.Request.Path.Value.Contains("/api"))
            //    {
            //        context.Request.Path = new PathString("/index.html");
            //        await next.Invoke();
            //    }
            //});

        }
    }
}
