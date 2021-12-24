using eBroker.BLL;
using eBroker.DAL;
using eBroker.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.API
{
    [ExcludeFromCodeCoverage]
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
            services.AddDbContext<EBrokerContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            ef => ef.MigrationsAssembly(typeof(EBrokerContext).Assembly.FullName)));

            services.AddScoped<ITransactionOrderRepository, TransactionOrderRepository>();
            services.AddScoped<IEquityRepository, EquityRepository>();
            services.AddScoped<IEquityHoldingRepository, EquityHoldingRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IWrapper, Wrapper>();

            services.AddTransient<ITradeService, TradeService>();
            services.AddTransient<IFundsService, FundsService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
