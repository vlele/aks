using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TaskAPI.AspNetCore.Web.Models.Persistent;

namespace TaskAPI.AspnetCore
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
            //services.AddDbContext<Models.TaskContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc()
                .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(); });

            services.AddTransient<ITaskService, TaskService>(obj => { return new TaskService(Configuration.GetConnectionString("TaskDB")); });


            // Register the Swagger generator, defining one or more Swagger documents
            var swaggerTitle = Configuration.GetSection("Swagger:Title").Value;
            var swaggerVersion = Configuration.GetSection("Swagger:Version").Value;
            var swaggerDescription = Configuration.GetSection("Swagger:Description").Value;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = swaggerTitle, Version = swaggerVersion, Description =swaggerDescription });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            var swaggerTitle = Configuration.GetSection("Swagger:Title").Value;

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerTitle);
            });

            app.UseMvc();
        }
    }
}
