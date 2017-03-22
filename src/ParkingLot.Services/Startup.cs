namespace ParkingLot.Services
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using ParkingLot.Data;
    using Interfaces;
    using Model;
    using ParkingLot.Data.Interfaces;
    using Entities = ParkingLot.Data.Model;
    using ParkingLot.Data.Repository;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            // adds json file to environment.
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // get connection string from configuration json file.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // inject context.
            services.AddDbContext<ParkingLotContext>(options =>
                options.UseSqlServer(connectionString));

            // dependency injection
            services.AddScoped<IRepository<Entities.ParkingLot>, Repository<Entities.ParkingLot>>();
            services.AddScoped<IParkingLotFacade, ParkingLotFacade>();

            // adds all of the dependencies that MVC 6 requires
            services.AddMvc();

            // Enabled cors. (don't do that in production environment, specify only trust origins)
            var policy = new CorsPolicy();
            policy.Headers.Add("*");
            policy.Methods.Add("*");
            policy.Origins.Add("*");
            policy.SupportsCredentials = true;

            services.AddCors(x => x.AddPolicy("defaultPolicy", policy));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Use the new policy globally
            app.UseCors("defaultPolicy");

            // adds MVC 6 to the pipeline
            app.UseMvc();
        }
    }
}