using AutoMapper;
using BankTransactionConciliationAPI.Extensions;
using BankTransactionConciliationAPI.MIddlewares;
using BankTransactionConciliationAPI.Models.Mapper;
using BankTransactionConciliationAPI.Models.Settings;
using BankTransactionConciliationAPI.Parsers;
using BankTransactionConciliationAPI.Parsers.Interfaces;
using BankTransactionConciliationAPI.Repository;
using BankTransactionConciliationAPI.Repository.Interfaces;
using BankTransactionConciliationAPI.Serializers;
using BankTransactionConciliationAPI.Services;
using BankTransactionConciliationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankTransactionConciliationAPI
{
    public class Startup
    {
        private static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddControllers()
               .AddJsonOptions(opts =>
               {
                   opts.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
                   opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
               });

            services.AddScoped<IBankTransactionService, BankTransactionService>();
            services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
            services.AddSingleton<ICsvParser, CsvParser>();
            services.AddSingleton<IOfxParser, OfxParser>();

            this.ConfigureMapper(services);
            this.ConfigureMongo(services);
            this.ConfigureSwagger(services);
            this.ConfigureCors(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseCustomExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMapper(IServiceCollection services)
        {
            var mapperConfiguration = GeneralMapper.ConfigureMapper();

            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void ConfigureMongo(IServiceCollection services)
        {
            services.Configure<MongoSettings>(options => Configuration.GetSection("MongoConnection").Bind(options));

            services.AddSingleton<IMongoDatabase>(provider => 
            {
                var settings = provider.GetService<IOptions<MongoSettings>>().Value;
                var mongoUrl = new MongoUrl(settings.ConnectionString);
                IMongoClient client = new MongoClient(mongoUrl);
                return client.GetDatabase(settings.Database);
            });
        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Contact = new OpenApiContact
                        {
                            Name = "Raphael Barradas",
                            Email = "raphaeldsb@gmail.com"
                        },
                        Title = "Bank Transaction Conciliation Api",
                        Version = "v1"
                    });
            });
        }

        public void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
