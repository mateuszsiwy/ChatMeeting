using ChatMeeting.Core.Application.Services;
using ChatMeeting.Core.Domain;
using ChatMeeting.Core.Domain.Interfaces.Repositories;
using ChatMeeting.Core.Domain.Interfaces.Services;
using ChatMeeting.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatMeeting.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }
    }
}
