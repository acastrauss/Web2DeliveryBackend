using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private String _CorsName = "cors";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Models.IDBModels.IConversion, DataLayer.MSSQLDB.Conversion.MSSQLConversion>();
            services.AddScoped<Services.IEmailService, Services.EmailService>();

            //Configuration.GetSection("ApplicationSettings")
            services.Configure<Services.AppSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddScoped<Services.IUsersService>(
                x =>
                    new Services.UsersService(
                        x.GetRequiredService<Models.IDBModels.IConversion>()
                        )
                    );

            services.AddScoped<Services.IDelivererService>(
                x =>
                new Services.DelivererService(
                    x.GetRequiredService<Models.IDBModels.IConversion>(),
                    x.GetRequiredService<Services.IEmailService>()
                    )
                );

            services.AddScoped<Services.IProductService>(
                x =>
                new Services.ProductService(
                    x.GetRequiredService<Models.IDBModels.IConversion>()
                    )
                );

            services.AddScoped<Services.IPurchaseService>(
                x =>
                new Services.PurchaseService(
                    x.GetRequiredService<Models.IDBModels.IConversion>()
                    )
                );

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(_CorsName, builder =>
                {
                    builder
                    .WithOrigins(Configuration["ApplicationSettings:Client_URL"]) // angular port
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build();
                });
            });

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key)
,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuers = new List<string>() { "https://localhost:5001", "http://localhost:5000" },
                    ValidAudience = "https://localhost:44339/api/"
                };
            });

            services.AddDbContext<DataLayer.DBModels.DeliveryDBContext>(
                options =>
                options.UseSqlServer("Server=DESKTOP-RA6QVFS;Database=DeliveryDB;Trusted_Connection=True;")
                );

            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(_CorsName);

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"imgs")),
                RequestPath = new PathString("/imgs")
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
