using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net60_ApiTemplate_20231.DTOs.Hospital;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Net60_ApiTemplate_20231.Services.Hospital;
using Net60_ApiTemplate_20231.Services.Order;
using Newtonsoft.Json;
using Serilog;
using static Net60_ApiTemplate_20231.DTOs.Hospital.HospitalResultDto;


namespace Net60_ApiTemplate_20231.Controllers
{
    /// <summary>
    /// Hospital Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalServices _hosiptalServices;
        private readonly Serilog.ILogger _logger;

        const string _controllerName = nameof(HospitalController);

        public HospitalController(IHospitalServices hosiptalServices , Serilog.ILogger? logger = null)
        {
            _hosiptalServices = hosiptalServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductController)) : logger.ForContext("ServiceName", nameof(ProductController));
        }


        /// <summary>
        ///     GetHospitalMaster
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet(Name = "GetHospitalMaster")]
        public async Task<ServiceResponse<RootResultHospitalDto>> GetHospitalMaster()
        {
            
            _logger.Debug("[{controllerName}] - GetHospitalMaster is Started: {date}", _controllerName, DateTime.Now);

            var result = await _hosiptalServices.AddHospital();

            _logger.Information("[{actionName}] - Sussess: {date}", _controllerName, DateTime.Now);

            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     GetHospitalMasterFile
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("File", Name = "GetHospitalMasterFile")]
        public async Task<ServiceResponse<RootResultHospitalDto>> GetHospitalMasterFile()
        {

            const string actionName = nameof(GetHospitalMasterFile);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var result = await _hosiptalServices.AddHospitalByFile();

            _logger.Information("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     GetHospitalMasterSf
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("Res", Name = "GetHospitalMasterSf")]
        public async Task<ServiceResponse<RootResultHospitalDto>> GetHospitalMasterSf()
        {

            const string actionName = nameof(GetHospitalMasterSf);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);
            
            var result = await _hosiptalServices.AddHospitalSf();

            _logger.Information("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return ResponseResult.Success(result);

        }
    }
}
