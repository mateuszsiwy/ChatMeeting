using ChatMeeting.Core.Application.Services;
using ChatMeeting.Core.Domain;
using ChatMeeting.Core.Domain.Interfaces.Producer;
using ChatMeeting.Core.Domain.Interfaces.Repositories;
using ChatMeeting.Core.Domain.Interfaces.Services;
using ChatMeeting.Core.Domain.Options;
using ChatMeeting.Infrastructure.Producer;
using ChatMeeting.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatMeeting.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            AddCustomAuthentication(services, configuration);
            var connectionString = configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettingsOptions>(options => configuration.GetSection(nameof(JwtSettingsOptions)).Bind(options));
            services.Configure<KafkaOptions>(options => configuration.GetSection(nameof(KafkaOptions)).Bind(options));
            return services;
        }

        private static void AddCustomAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettingsOptions)).Get<JwtSettingsOptions>();

            if(jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
            {
                throw new ArgumentNullException("Secret Key is empty");
            }

            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = GetTokenValidationParams(key);
                options.Events = GetEvents();
            });
        }

        private static JwtBearerEvents GetEvents()
        {
            return new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["token"];
                    var path = context.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/messageHub"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        }

        private static TokenValidationParameters GetTokenValidationParams(byte[] key)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // zapobiega podszywaniu sie pod legalne tokeny
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // sprawia ze nie sprawdzamy kto wydal token
                ValidateAudience = false, // validuje odbiorce
                ValidateLifetime = true, // sprawdza czy data waznosci tokena juz minela
                ClockSkew = TimeSpan.FromSeconds(120) // margines czasowy na weryfikacje dat waznosci tokenu, jest taki zeby wliczyc roznice czasu miedzy tokenami
            };
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddSingleton(new UserConnectionService());
            services.AddTransient<IKafkaProducer, KafkaProducer>();
            services.AddSignalR();
            return services;
        }
    }
}
