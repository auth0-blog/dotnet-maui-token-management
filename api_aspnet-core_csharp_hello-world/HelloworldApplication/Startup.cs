using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using HelloworldApplication.Models;
using System.Text.Json;

namespace HelloworldApplication
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

      services.AddCors(options =>
      {
        options.AddDefaultPolicy(
           builder =>
           {
             builder.WithOrigins("http://localhost:4040")
                .WithHeaders("Authorization");
           });
      });

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
            options.Audience = Configuration["Auth0:Audience"];

            options.Events = new JwtBearerEvents
            {
              OnChallenge = context =>
              {
                context.Response.OnStarting(async () =>
                    {
                      await context.Response.WriteAsync(
                          JsonSerializer.Serialize(new ApiResponse("You are not authorized!"))
                          );
                    });

                return Task.CompletedTask;
              }
            };
          });

      services.AddAuthorization(options =>
      {
        options.AddPolicy("Admin", policy =>
              policy.RequireAssertion(context =>
                  context.User.HasClaim(c =>
                      (c.Type == "permissions" &&
                      c.Value == "read:admin-messages") &&
                      c.Issuer == $"https://{Configuration["Auth0:Domain"]}/")));

      });

      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseCors();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
