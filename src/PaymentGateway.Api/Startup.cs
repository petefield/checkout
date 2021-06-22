
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using AcquiringBank.Contracts;
using AcquiringBank.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PaymentGateway.Data.Contracts;
using PaymentGateway.Validation;
using Microsoft.Extensions.Hosting;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

namespace PaymentGateway.Api
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
            services.AddApplicationInsightsTelemetry();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            }); ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout.Com DotNet Test : Payment Gateway API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddScoped<IAcquiringBank, InMemoryAcquiringBank>();
            services.AddScoped<IValidCurrencyCodeProvider, InMemoryCurrencyCodeProvider>();
            services.AddSingleton<IPaymentStore, Data.InMemory.PaymentStore>();
            services.AddSingleton<IDynamoDBContext>( sp => {
                AmazonDynamoDBClient awsDbClient = new AmazonDynamoDBClient("AKIAYEGJL3VYUGJGIT6C", "Sj77i7tx5dPxWWxHJwoUD5DRe5fcPz531qhsOz9X", Amazon.RegionEndpoint.EUWest2);
                return new DynamoDBContext(awsDbClient); 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "src v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
