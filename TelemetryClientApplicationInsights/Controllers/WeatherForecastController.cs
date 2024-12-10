using LoggingAppInsights.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TelemetryClientApplicationInsights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
         

        private readonly TelemetryClient _telemetryClient;
        public WeatherForecastController(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var model = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            
            var modelString = JsonSerializer.Serialize(model);

            var processName = nameof(Get);

            //=====================================================//
            // 1. START_PROCESS
            //=====================================================// 
            //_telemetryClient.TrackTrace(LoggingMessages.START_PROCESS + processName + "Request data: {Request}", SeverityLevel.Information, new Dictionary<string, string> { { "RequestData", modelString } });

            _telemetryClient.TrackTrace($"{
                LoggingMessages.START_PROCESS}{processName} - Request initiated",
                SeverityLevel.Information,
                new Dictionary<string, string>
                {
                    { "RequestData", modelString },
                    { "Timestamp", DateTime.UtcNow.ToString("o") }, // ISO 8601 format for better readability
                    { "UserId", HttpContext.User?.Identity?.Name ?? "Anonymous" }, // Example: capture user info
                    { "ProcessName", processName },
                    { "LogLevel", SeverityLevel.Information.ToString() }
                });

            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................
                //=====================================================//
                // 2. WARNING_PROCESS
                //=====================================================// 
                _telemetryClient.TrackTrace(LoggingMessages.WARNING_PROCESS + processName, SeverityLevel.Warning);

                // ..............................
                //*** Business logic
                // ..............................
                //=====================================================//
                // 3. SUCCESS_PROCESS
                //=====================================================// 
                _telemetryClient.TrackTrace(LoggingMessages.SUCCESS_PROCESS + processName  , SeverityLevel.Information );


                // Test exception
                throw new Exception(LoggingMessages.START_PROCESS + processName + " Test exception for logging.");

            }
            catch (Exception ex)
            {
                //=====================================================//
                // 4. EXCEPTION_PROCESS
                //=====================================================//

                var errorData = new
                {
                    ExceptionMessage = ex.Message,
                    FileName = new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileName(),
                    LineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber().ToString()
                };
                var errorDataString = JsonSerializer.Serialize(errorData);
                 
                _telemetryClient.TrackTrace(LoggingMessages.EXCEPTION_PROCESS + processName + "LogError data: {LogError}", SeverityLevel.Error, new Dictionary<string, string> { { "ErrorData", errorDataString } });
                _telemetryClient.TrackException(ex);
            }
            

            return model;
        }
    }
}
