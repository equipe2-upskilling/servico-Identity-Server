using Grupo2_Identity_Server.Entities;
using Grupo2_Identity_Server.Interfaces;
using System.Text.Json;

namespace Grupo2_Identity_Server.Services
{
    public class AccessService
    {
        private readonly IToken _token;
        private readonly TimeSpan _time;
        private readonly string _secret;
        public AccessService(IToken token)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            _time = TimeSpan.FromDays(Convert.ToInt16(configuration["TokenConfigurations:ExpireHours"]));
            _secret = configuration["Jwt:Key"];
            _token = token;
        }

        public string BuildToken(User usersDto)
        {

            var json = JsonSerializer.Serialize(usersDto);
            return _token.Encrypt(json, _secret, _time);
        }

        public User TokenAccess(string access)
        {
            try
            {
                var json = _token.Decrypt(access, _secret);
                return JsonSerializer.Deserialize<User>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
