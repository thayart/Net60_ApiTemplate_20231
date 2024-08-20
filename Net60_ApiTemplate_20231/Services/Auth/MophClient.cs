using Microsoft.Extensions.Options;
using Net60_ApiTemplate_20231.Configurations;
using Net60_ApiTemplate_20231.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Net60_ApiTemplate_20231.DTOs.Hospital.HospitalResultDto;

namespace Net60_ApiTemplate_20231.Services.Auth
{
    public class MophClient
    {
        internal readonly RestClient _client;
        internal readonly MophApiSettings? _setting;
        internal readonly Serilog.ILogger _logger;

        public MophClient(IOptions<MophApiSettings> options, Serilog.ILogger? logger = null)
        {
            _setting = options.Value;
            var instanceUrl = $"{_setting.GetTokenUrl}";

            var restOptions = new RestClientOptions(instanceUrl)
            {
                //add default authenication
                Authenticator = new MophAuthenicator(_setting),
                PreAuthenticate = true,
            };

            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };

            _client = new RestClient(restOptions, configureSerialization: s => s.UseNewtonsoftJson(jsonSetting));

            _logger = logger?.ForContext("ClientName", nameof(MophClient))
                ?? Serilog.Log.ForContext("ClientName", nameof(MophClient));
        }

        private const string _clientName = nameof(MophClient);

        public async Task<RootResultHospitalDto> GetCoundData()
        {
            _logger.Debug("[{ClientName}] GetCoundData", _clientName);
            _logger.Verbose("[{ClientName}] GetCoundData [typeof={DeserializeType}]", _clientName, nameof(GetCoundData));

            var request = new RestRequest(_setting.GetCountHealthOfficeUrl);
            var response = await _client.GetAsync(request);

            if (response == null || response.IsSuccessful == false)
                throw new ApiException("Query Error");

            _logger.Information("[{ClientName}] Query Successfully", _clientName);
            _logger.Verbose("[{ClientName}] Query Response = {@response}", _clientName, response.Content);
            return JsonConvert.DeserializeObject<RootResultHospitalDto>(response.Content);
        }

        public async Task<RootResultHospitalDto> GetData(int page)
        {
            _logger.Debug("[{ClientName}] GetCoundData", _clientName);
            _logger.Verbose("[{ClientName}] GetCoundData [typeof={DeserializeType}]", _clientName, nameof(GetData));

            var instantUrl = _setting.GetCountHealthOfficeUrl + "?page=" + page + "&page_size=1000";
            var request = new RestRequest(instantUrl);
            var response = await _client.GetAsync(request);
            var result = new List<ResultHospitalDto>();
            if (response == null || response.IsSuccessful == false)
                throw new ApiException("Query Error");

            _logger.Information("[{ClientName}] Query Successfully", _clientName);
            _logger.Verbose("[{ClientName}] Query Response = {@response}", _clientName, response.Content);
            return  JsonConvert.DeserializeObject<RootResultHospitalDto>(response.Content);
        }

        public async Task<T?> Query<T>(string query, CancellationToken cancellationToken = default) where T : class
        {
            _logger.Debug("[{ClientName}] Query", _clientName);
            _logger.Verbose("[{ClientName}] Query [q = {query}, typeof={DeserializeType}]", _clientName, query, nameof(T));
            var request = new RestRequest("query");
            request.AddParameter("q", query);

            var response = await this._client.GetAsync(request, cancellationToken);

            if (response == null || response.IsSuccessful == false)
                throw new ApiException("Query Error");

            _logger.Information("[{ClientName}] Query Successfully", _clientName);
            _logger.Verbose("[{ClientName}] Query Response = {@response}", _clientName, response.Content);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public async Task<RootResultHospitalDto> Create<T>(string objectName, T data, CancellationToken cancellationToken = default) where T : class
        {
            _logger.Debug("[{ClientName}] Create objectName = {objectName}", _clientName, objectName);
            _logger.Verbose("[{ClientName}] Create [objectName = {objectName}, data = {@data}]", _clientName, objectName, data);
            var request = new RestRequest($"sobjects/{objectName}");
            request.AddJsonBody(data);

            var response = await this._client.PostAsync(request, cancellationToken);

            if (response == null || response.IsSuccessful == false)
                throw new ApiException("Create Error");

            _logger.Information("[{ClientName}] Create Successfully", _clientName);
            _logger.Verbose("[{ClientName}] Create Response = {@response}", _clientName, response.Content);
            return JsonConvert.DeserializeObject<RootResultHospitalDto>(response.Content);
        }

        public async Task<bool> Update<T>(string objectName, string id, T data, CancellationToken cancellationToken = default) where T : class
        {
            _logger.Debug("[{ClientName}] Update objectName = {objectName}, id = {id}", _clientName, objectName, id);
            _logger.Verbose("[{ClientName}] Update [objectName = {objectName}, id = {id}, data = {@data}]", _clientName, objectName, id, data);
            var request = new RestRequest($"sobjects/{objectName}/{id}");
            request.AddJsonBody(data);

            var response = await this._client.PatchAsync(request, cancellationToken);

            if (response == null || response.IsSuccessful == false)
                throw new ApiException("Update Error");

            _logger.Information("[{ClientName}] Update Successfully", _clientName);
            _logger.Verbose("[{ClientName}] Update Response = {@response}", _clientName, response.Content);
            return response.IsSuccessful;
        }

    }
}
