using Net60_ApiTemplate_20231.DTOs.Hospital;
using Net60_ApiTemplate_20231.DTOs.Orders;
using static Net60_ApiTemplate_20231.DTOs.Hospital.HospitalResultDto;

namespace Net60_ApiTemplate_20231.Services.Hospital
{
    public interface IHospitalServices
    {
        Task<RootResultHospital> AddHospital();
        Task<RootResultHospital> AddHospitalSf();

        Task<RootResultHospital> AddHospitalByFile();

    }
}
