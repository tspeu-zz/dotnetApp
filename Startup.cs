using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DatinApp.API
{
    public class Startup
    {
        //consrtructor->
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

                public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //añadir el servicio dbContext  e la clase que se a creado
            //ADEMAS SE PASA LA CONFIGURACIONA APPSETTING y de ahí puede obtener la configuracion por getConnctionString
            services.AddDbContext<DataContext>( data => data.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            //hay que añadir UseQlite del nugetpackage como npm/tarn
            //>Microsoft.EntityFrameworkCore.Sqlite
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        
        //add CORS->
            services.AddCors();

        //Add service de repository
        // services.AddSingleton puede traer problemas al hacer llamadas recurrentes
        
        //crea una instancia por cada http pero usa la misma si se reutilaza
        services.AddScoped<IAuthRepository, AuthRepository>();
        //se le pasa la interfase y la implementacion específica
        
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //verifia el entorno
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //use htpps 
                // app.UseHsts();
            }
//
//se especifica el origen de las llamadas permitidas en develop
            app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
            
            // app.UseHttpsRedirection();
            //el framewor pero lo usa como middlware
            app.UseMvc();
        }
    }
}
