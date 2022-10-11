﻿using home_energy_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceReportController : ControllerBase
    {
        private IDeviceReporter _deviceReporter;

        public DeviceReportController(IDeviceReporter deviceReporter)
        {
            _deviceReporter = deviceReporter;
        }

        [HttpPost]
        [Route("Report")]
        [Authorize]
        public IActionResult ReportDevice([FromBody] DeviceReportModel reportModel)
        {
            try
            {
                var report = ReportApiModelToReport(reportModel);

                _deviceReporter.Report(report);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro reportar o dispositivo: " + ex.Message);
            }
        }

        private DeviceReport ReportApiModelToReport(DeviceReportModel report)
        {
            return new DeviceReport
            {
                IdentificationCode = report.IdentificationCode,
                WattsUsage = Convert.ToDecimal(String.Format("{0:0.##}", report.WattsUsage))
            };
        }
    }
}