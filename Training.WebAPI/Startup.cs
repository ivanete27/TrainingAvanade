using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Training.Application.Albums;
using Training.Application.Books;
using Training.Application.Reservations;
using Training.Core.Repositories;
using Training.DAL;
using Training.WebAPI.Helpers;

namespace Training.WebAPI
{
    public class Startup
    {
        private const string AUTH_SCHEMA = "BasicAuthentication";
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DbConnection");

            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddFile("app.log", append: true);
            });

            services.AddAuthentication(AUTH_SCHEMA)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(AUTH_SCHEMA, null);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Training.WebAPI", Version = "v1" });
            });

            services.AddScoped<RequestCultureMiddleware>();
            services.AddScoped<RequestLoggingMiddleware>();

            services.AddTransient<ICustomDateTimeProvider, CustomDateTimeProvider>();

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IAlbumService, AlbumService>();

            services.AddScoped(x => new TrainingDbContext(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
                        
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAlbumRepository, AlbumRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training.WebAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
