# Telemetry with Application Insights in ASP.NET Core

This project demonstrates integrating **Azure Application Insights** with an ASP.NET Core Web API. It uses **TelemetryClient** for logging key events, warnings, and exceptions with custom messages in Application Insights.

---

## Project Structure

### `Program.cs`
1. **Setup Configuration:**
   - Application Insights is configured using a connection string from `appsettings.json` or environment variables.
   - A custom logging utility `LoggingMessages` initializes with app-specific configurations.

2. **Application Initialization:**
   - Registers services like controllers, Swagger, and Application Insights in the DI container.
   - Configures middleware for development and production environments.

---

### `LoggingMessages.cs`
- A static class providing reusable logging message templates.
- **Initialization:**
  - Reads the `ApplicationLogName` from the configuration to prefix log messages with the application name.
- **Message Templates:**
  - `FILTER_PROCESS`: General message template for process filtering.
  - `START_PROCESS`: Logs when a process starts.
  - `WARNING_PROCESS`: Logs warnings during a process.
  - `SUCCESS_PROCESS`: Logs successful completion of a process.
  - `EXCEPTION_PROCESS`: Logs details of exceptions.

---

### `WeatherForecastController.cs`
- **Purpose:**
  - Implements an example API endpoint that demonstrates logging with **TelemetryClient**.

- **Key Features:**
  1. **START_PROCESS Logging:**
     - Logs the start of the process with request details.
  2. **WARNING_PROCESS Logging:**
     - Logs potential issues or warnings.
  3. **SUCCESS_PROCESS Logging:**
     - Logs successful execution of the business logic.
  4. **EXCEPTION_PROCESS Logging:**
     - Catches exceptions, logs error details (stack trace, file name, line number), and reports them to Application Insights.

- **Integration with TelemetryClient:**
  - Uses `TrackTrace` for custom log messages and `TrackException` for exceptions.
  - Demonstrates the use of **Severity Levels** (e.g., Information, Warning, Error).

---

### Logging Example in Application Insights

1. **Trace Message Format**:
   ```plaintext
   <ApplicationName> Process Start: <MethodName> Request data: {Request}
   ```
2. **Warning Format**:
   ```plaintext
   <ApplicationName> Process Warning: <MethodName>
   ```
3. **Exception Format**:
   ```json
   {
     "ExceptionMessage": "Error message",
     "FileName": "FileName.cs",
     "LineNumber": "42"
   }
   ```

---

## Running the Project

1. **Setup Application Insights**:
   - Add your Application Insights connection string in `appsettings.json` under `ApplicationInsights:ConnectionString`.

2. **Run Locally**:
   - Use `dotnet run` to start the application.
   - Open the Swagger UI to test the `WeatherForecast` endpoint.

3. **Monitoring Logs**:
   - View logs and telemetry in the **Azure Application Insights** dashboard.

---

## Dependencies

- **Azure.ApplicationInsights.AspNetCore**: Enables Application Insights in ASP.NET Core.
- **System.Text.Json**: Serializes request and error data for logging.

---

## Future Enhancements

1. Add more detailed log categories (e.g., `DEBUG`, `CRITICAL`).
2. Extend the `LoggingMessages` class with dynamic metadata.
3. Implement middleware for centralized logging.

---

This project illustrates structured logging and how to leverage **Azure Application Insights** for comprehensive telemetry in an ASP.NET Core Web API.