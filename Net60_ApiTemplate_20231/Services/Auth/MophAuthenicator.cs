using Net60_ApiTemplate_20231.Configurations;
using Net60_ApiTemplate_20231.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Net60_ApiTemplate_20231.Services.Auth
{
    public class MophAuthenicator : AuthenticatorBase
    {
        private readonly MophApiSettings _setting;
        private DateTime _tokenExpiration;
        private readonly Serilog.ILogger _logger;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);
        private const string _serviceName = nameof(MophAuthenicator);
        public MophAuthenicator(MophApiSettings setting) : base("")

        {
            _setting = setting;
            _logger = Serilog.Log.ForContext<MophAuthenicator>();

            Token = string.Empty;

        }

        /// <summary>
        /// Get Authentication Parameter
        /// </summary>
        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            // check token expired
            if (string.IsNullOrEmpty(Token) || DateTime.UtcNow.AddMinutes(5) >= _tokenExpiration)
                await CheckTokenExpired();

            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }

        /// <summary>
        /// Check Token Expired
        /// </summary>
        /// <returns></returns>
        private async Task CheckTokenExpired()
        {
            // prevent multiple request
            await _tokenLock.WaitAsync();

            try
            {
                // check token expired again after lock
                if (string.IsNullOrEmpty(Token) || DateTime.UtcNow.AddMinutes(5) >= _tokenExpiration)
                    await GetToken();
            }
            finally
            {
                _tokenLock.Release();
            }

        }

        /// <summary>
        /// Get Token
        /// </summary>
        public async Task<RootTokensHospital> GetToken()
        {
            // Create authentication request
            var endpoint = _setting.GetTokenUrl;

            _logger.Debug("[{ServiceName}] Get Token at {EndPoint}", _serviceName, endpoint);

            var options = new RestClientOptions(endpoint);

            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };

            using var client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson(jsonSetting));

            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", _setting.UserName);
            request.AddParameter("password", _setting.Password);

            var response = await client.ExecutePostAsync<RootTokensHospital>(request);

            if (!response.IsSuccessful)
            {
                _logger.Error("[{ServiceName}] Get Token Error", _serviceName);
                throw new ApiException("Get Token Error");
            }

            // set token and expiration from unixtimestamp
            _tokenExpiration = DateTime.Now.AddMinutes(35).ToUniversalTime();
            Token = $"Bearer {response.Data.Access}";

            _logger.Information("[{ServiceName}] Get Token Successfully", _serviceName);
            return response.Data;
        }

        public class RootTokensHospital
        {
            [JsonProperty("refresh")]
            public string? Refresh { get; set; }
            [JsonProperty("access")]
            public string? Access { get; set; }
            [JsonProperty("token_type")]
            public string? TokenType { get; set; }
        }

    }
}
