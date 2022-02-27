using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Add DbContext service
            services.AddDbContext<StoreContext>(options
                => options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

            #region moved Repository service
            //add ProductRepository service
            //services.AddScoped<IProductRepository, ProductRepository>();

            //add GenericRepository service
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            //ApplicationSerice is a class Extend IServiceCollection
            services.AddApplicationServices();

            //adding identity service
            services.AddIdentityServices(_config);

            //add AutoMapper service
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();

            #region moved Swagger service
            /*
            //add Swagger Service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sela Store API", Version = "v1" });
            });
            */
            #endregion

            services.AddSwaggerDocumentation();
            #region moved override controller behavior service
            //override controller behavior
            /*
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            */
            #endregion

            //add CORS service
            services.AddCors(options =>
            {
                options.AddPolicy("CorePolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            //adding Redis services
            services.AddSingleton<IConnectionMultiplexer>(c => {

                var configration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);

                return ConnectionMultiplexer.Connect(configration);
            });

            //adding Identity Dbcontext service
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("IdentityConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region
            /*
             * if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
            */
            #endregion


            //Add Custom Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            //handle empty endpoints errors
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            //serve static Image Files in wwwroot files
            app.UseStaticFiles();

            //add CORS middleware
            app.UseCors("CorePolicy");

            //adding Authentication service
            app.UseAuthentication();

            app.UseAuthorization();

            #region moved swagger middlewares
            //add swagger middlewares
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sela Store API v1"); });
            #endregion
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
