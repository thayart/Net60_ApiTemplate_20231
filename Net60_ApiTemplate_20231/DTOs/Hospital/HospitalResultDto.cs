using Newtonsoft.Json;

namespace Net60_ApiTemplate_20231.DTOs.Hospital
{
    public class HospitalResultDto
    {
        public class AddressHospital            
        {
            public int Id { get; set; }
            public string? Moo { get; set; }
            public string? Room { get; set; }
            public string? Alley { get; set; }
            public string? Floor { get; set; }
            public string? Steet { get; set; }
            public string? Number { get; set; }
            public string? Region { get; set; }
            public string? Details { get; set; }
            public string? Village { get; set; }
            public string? Zipcode { get; set; }
            public string? Building { get; set; }
            public string? District { get; set; }
            public string? Province { get; set; }
            public string? Health_zone { get; set; }
            public string? Subdistrict { get; set; }
            public string? District_code { get; set; }
            public string? Province_code { get; set; }
            public string? Province_type { get; set; }
            public bool Is_double_space { get; set; }
            public string? Subdistrict_code { get; set; }
        }

        public class ResultHospital
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Code9 { get; set; }
            public string? Code5 { get; set; }
            public string? Results { get; set; }
            public string? Organization_type { get; set; }
            public string? Private_code { get; set; }
            public string? Health_office_type { get; set; }
            public string? Health_office_type_code { get; set; }
            public string? Organization { get; set; }
            public string? Organization_code { get; set; }
            public string? Department { get; set; }
            public string? Department_code { get; set; }
            public bool Active { get; set; }
            public AddressHospital Address { get; set; }
            public string? Established_date { get; set; }
            public string? Elosed_date { get; set; }
            public string? Bed { get; set; }
            public bool Is_client_hospital { get; set; }
            public string? Host_agency { get; set; }
            public string? Client_health_office_type { get; set; }
            public DateTime Modified_date { get; set; }
            public List<object>? Notes { get; set; }
        }

        public class RootResultHospital
        {
            public int Count { get; set; }
            public string? Next { get; set; }
            public object Previous { get; set; }
            public List<ResultHospital>? Results { get; set; }
        }

        public class RootTokensHospital
        {
            [JsonProperty("refresh")]
            public string? Refresh { get; set; }
            [JsonProperty("access")]
            public string? Access { get; set; }
        }

    }
}
