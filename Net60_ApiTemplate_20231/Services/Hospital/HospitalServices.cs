using AutoMapper;
using Humanizer;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net60_ApiTemplate_20231.Configurations;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs.Hospital;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Net60_ApiTemplate_20231.Services.Order;
using Net60_ApiTemplate_20231.Services.Product;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http;
using static Net60_ApiTemplate_20231.DTOs.Hospital.HospitalResultDto;

namespace Net60_ApiTemplate_20231.Services.Hospital
{
    public class HospitalServices : IHospitalServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;
        private readonly Serilog.ILogger _logger;
        private readonly MophApiSettings _mophApiSettings;

        public HospitalServices(AppDBContext dbContext,
            IMapper mapper,
            IOptions<MophApiSettings> options,
            IHttpContextAccessor httpContext,
            ILoginDetailServices loginDetailServices,
            Serilog.ILogger? logger = null)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext;
            _loginDetailServices = loginDetailServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(HospitalServices)) : logger.ForContext("ServiceName", nameof(HospitalServices));
            _mophApiSettings = options.Value;

        }

        private async Task<int> CountHospital(string Token)
        {
            const string actionName = nameof(CountHospital);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var datasCountHospital = new RootResultHospital();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiHospitalUrl = _mophApiSettings.GetCountHealthOfficeUrl;

                    httpClient.BaseAddress = new Uri(apiHospitalUrl);
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

                    HttpResponseMessage requestHospital = await httpClient.GetAsync(apiHospitalUrl);
                    if (requestHospital.IsSuccessStatusCode == true)
                    {
                        var responseHospital = await requestHospital.Content.ReadAsStringAsync();
                        datasCountHospital = JsonConvert.DeserializeObject<RootResultHospital>(responseHospital);

                    }
                }
            }
            catch (Exception msg)
            {
                string message = msg.Message;
            }

            _logger.Debug("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return datasCountHospital.Count;
        }

        public async Task<RootResultHospital> AddHospital()
        {
            const string actionName = nameof(AddHospital);
            int updateRows = 0;
            int insertRows = 0;
            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            try
            {
                int ResultCount = 0;
                string accessToken = "";
                string apiTokenUrl = _mophApiSettings.GetTokenUrl;
                using (HttpClient client = new HttpClient())
                {
                    var postBody = new Dictionary<String, String>
                   {
                       { "username", _mophApiSettings.UserName},
                       { "password", _mophApiSettings.Password}
                   };
                    var getToken = new HttpRequestMessage(HttpMethod.Post, apiTokenUrl)
                    {
                        Content = new FormUrlEncodedContent(postBody),

                    };
                    var requestToken = await client.SendAsync(getToken);

                    if (requestToken.IsSuccessStatusCode == true)
                    {
                        var responseToken = await requestToken.Content.ReadAsStringAsync();
                        var datasToken = JsonConvert.DeserializeObject<RootTokensHospital>(responseToken);

                        // Go to Function Count Total Data
                        ResultCount = await CountHospital(datasToken.Access);

                        accessToken = datasToken.Access;

                    }
                }

                int perRecord = 1000;
                int maxRound = (ResultCount / perRecord)+1;
                //int maxRound = 10;
                var getHospitalDB = await _dbContext.HospitalMophs.ToListAsync();
                for (int i = 1; i <= maxRound; i++)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiHospitalUrl = _mophApiSettings.GetHealthOfficeUrl + "?page=" + i + "&page_size=" + perRecord;

                        httpClient.BaseAddress = new Uri(apiHospitalUrl);
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);


                        HttpResponseMessage requestHospital = await httpClient.GetAsync(apiHospitalUrl);
                        if (requestHospital.IsSuccessStatusCode == true)
                        {
                            var responseHospital = await requestHospital.Content.ReadAsStringAsync();
                            var datasHospital = JsonConvert.DeserializeObject<RootResultHospital>(responseHospital);
                            var tempAddHospitals = new List<HospitalMoph>();
                            foreach (var temp in datasHospital.Results)
                            {
                                if (getHospitalDB.Any(w => w.HospitalNineCode == temp.Code9 && w.HospitalName == temp.Name))
                                {
                                    var getHospital = getHospitalDB.Where(w => w.HospitalNineCode == temp.Code9 && w.HospitalName == temp.Name).FirstOrDefault();
                                    if (Convert.ToDateTime(getHospital.UpdateOrigin).Date != temp.Modified_date.Date)
                                    {
                                        getHospital.HospitalOriginId = temp.Id;
                                        getHospital.HospitalLicence = temp.Private_code;
                                        getHospital.HospitalTypeName = temp.Health_office_type;
                                        getHospital.IsActive = temp.Active;
                                        getHospital.No = temp.Address.Number;
                                        getHospital.Moo = temp.Address.Moo;
                                        getHospital.Room = temp.Address.Room;
                                        getHospital.Floor = temp.Address.Floor;
                                        getHospital.BuildingName = temp.Address.Building;
                                        getHospital.VillageName = temp.Address.Village;
                                        getHospital.Soi = temp.Address.Alley;
                                        getHospital.Road = temp.Address.Steet;
                                        getHospital.AddressDetail = temp.Address.Details;
                                        getHospital.SubdistrictId = Convert.ToInt32(temp.Address.Subdistrict_code);
                                        getHospital.SubdistrictName = temp.Address.Subdistrict;
                                        getHospital.DistrictId = Convert.ToInt32(temp.Address.District_code);
                                        getHospital.DistrictName = temp.Address.District;
                                        getHospital.ProvinceId = Convert.ToInt32(temp.Address.Province_code);
                                        getHospital.ProvinceName = temp.Address.Province;
                                        getHospital.UpdateOrigin = temp.Modified_date;
                                        getHospital.UpdatedDate = DateTime.Now;

                                        _dbContext.UpdateRange(getHospital);

                                        updateRows++;
                                    }
                                }
                                else
                                {
                                    var insHospitals = new HospitalMoph();
                                    insHospitals.HospitalOriginId = temp.Id;
                                    insHospitals.HospitalName = temp.Name;
                                    insHospitals.HospitalNineCode = temp.Code9;
                                    insHospitals.HospitalFiveCode = temp.Code5;
                                    insHospitals.HospitalLicence = temp.Private_code;
                                    insHospitals.HospitalTypeName = temp.Health_office_type;
                                    insHospitals.IsActive = temp.Active;
                                    insHospitals.No = temp.Address.Number;
                                    insHospitals.Moo = temp.Address.Moo;
                                    insHospitals.Room = temp.Address.Room;
                                    insHospitals.Floor = temp.Address.Floor;
                                    insHospitals.BuildingName = temp.Address.Building;
                                    insHospitals.VillageName = temp.Address.Village;
                                    insHospitals.Soi = temp.Address.Alley;
                                    insHospitals.Road = temp.Address.Steet;
                                    insHospitals.AddressDetail = temp.Address.Details;
                                    insHospitals.SubdistrictId = Convert.ToInt32(temp.Address.Subdistrict_code);
                                    insHospitals.SubdistrictName = temp.Address.Subdistrict;
                                    insHospitals.DistrictId = Convert.ToInt32(temp.Address.District_code);
                                    insHospitals.DistrictName = temp.Address.District;
                                    insHospitals.ProvinceId = Convert.ToInt32(temp.Address.Province_code);
                                    insHospitals.ProvinceName = temp.Address.Province;
                                    insHospitals.UpdateOrigin = temp.Modified_date;
                                    insHospitals.UpdatedDate = DateTime.Now;

                                    _dbContext.AddRange(insHospitals);

                                    insertRows++;
                                }
                            }
                            await _dbContext.SaveChangesAsync();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            var dto = new RootResultHospital();
            dto.Count = _dbContext.HospitalMophs.Count();
            dto.Next = "Insert Data: " + insertRows + " -Update Data: " + updateRows;

            _logger.Debug("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return dto;

        }


    }
}
