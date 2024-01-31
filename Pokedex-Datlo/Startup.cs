using Pokedex_Datlo.Application.AppServices;
using Pokedex_Datlo.Domain.Services;
using Pokedex_Datlo.Infrastructure.Repositories;

namespace Pokedex_Datlo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //Services
            services.AddScoped<IDataSetAppService, DataSetAppService>();
            services.AddScoped<IFileImporterFactory, FileImporterFactory>();

            //Repositories
            services.AddSingleton<IDataSetRepository, DataSetRepository>();
            services.AddSingleton<IFileImporterRepository, CsvFileImporter>();
            services.AddSingleton<IFileImporterRepository, ExcelFileImporter>();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
