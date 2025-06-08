using Invoice.BLL.Common.Authentication;
using Invoice.BLL.Services;
using Invoice.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Invoice.PL
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoice API", Version = "v1" });

            });

           services.AddSignalR();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1uDZcf23nTqXgOIrvLwbW4qocRiueNl1")),
                    ValidIssuer = "InvoiceApp",
                    ValidAudience = "InvoiceApp Users"
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;

            });


            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IInvoiceServices, InvoiceServices>();
            services.AddScoped<IStoresServices, StoresServices>();
            services.AddScoped<ICustomersServices, CustomersServices>();
            services.AddScoped<IProductsServices, ProductsServices>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddOpenApi();
            return services;
        }
    }
}
