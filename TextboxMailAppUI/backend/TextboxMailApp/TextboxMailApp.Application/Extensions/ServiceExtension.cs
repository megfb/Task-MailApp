using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Application.Features.Users;


namespace TextboxMailApp.Application.Extensions
{
  public static class ServiceExtension
  {
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
      services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
      services.AddScoped<IEmailFetchService, EmailFetchService>();
      services.AddScoped<IUserCommandService, UserCommandService>();
      services.AddScoped<IUserQueryService, UserQueryService>();
      return services;
    }
  }
}
