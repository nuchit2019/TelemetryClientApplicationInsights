# Telemetry with Application Insights in ASP.NET Core

This project demonstrates integrating **Azure Application Insights** with an ASP.NET Core Web API. It uses **TelemetryClient** for logging key events, warnings, and exceptions with custom messages in Application Insights.


## Getting Started

Follow these steps to set up and configure dependencies, including **Application Insights** for telemetry:

### Prerequisites

- **.NET 6.0 SDK** or later
- A valid **Application Insights Connection String** from the Azure Portal
- An IDE like **Visual Studio 2022** or **VS Code**

## Setup Guide

### 1. Install Dependencies

Run the following commands to install the required NuGet packages:

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

### 2. Configure `appsettings.json`

Ensure the configuration file is properly structured for flexibility.

```json
{
  "ApplicationLogName": "WeatherApp",
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=your-instrumentation-key"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

---

### 3. Project Structure

### `Program.cs`
1. **Setup Configuration:**
   - Application Insights is configured using a connection string from `appsettings.json` or environment variables.
   - A custom logging utility `LoggingMessages` initializes with app-specific configurations.

2. **Application Initialization:**
   - Registers services like controllers, Swagger, and Application Insights in the DI container.
   - Configures middleware for development and production environments.

---

#### `LoggingMessages.cs`
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

#### `WeatherForecastController.cs`

Flow Code Logging Example...
```markdown
//=====================================================//
// 1. START_PROCESS
//=====================================================//

// ..............................
//*** Validation Logic
// ..............................
//=====================================================//
// 2. WARNING_PROCESS
//=====================================================//

// ..............................
//*** Business logic
// ..............................
//=====================================================//
// 3. SUCCESS_PROCESS
//=====================================================//

//=====================================================//
// 4. EXCEPTION_PROCESS
//=====================================================//
```

- **Purpose:**
  - Implements an example API endpoint that demonstrates logging with `**TelemetryClient**.`

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

## TelemetryClient.TrackTrace(...)

`TelemetryClient.TrackTrace()` is a method in Microsoft Application Insights used to send trace logs to Application Insights. It helps monitor and track system operations, such as logging process states or reporting issues. You can specify the severity level and additional properties for detailed insights.

---

### Format of `TrackTrace`
```csharp
public void TrackTrace(
    string message,
    SeverityLevel severityLevel,
    IDictionary<string, string> properties
)
```

### Parameter Details
1. **`message` (string)**  
   - The message to be logged or sent to Application Insights, such as `"Process started"`.  
   - The message should be clear and provide useful context for later analysis.

2. **`severityLevel` (SeverityLevel)**  
   - The severity level of the message, e.g., general information (Information), warnings (Warning), or errors (Error).  
   - Available `SeverityLevel` values:
     - `Verbose` - Detailed, low-priority information.
     - `Information` - General information.
     - `Warning` - Potential issues requiring attention.
     - `Error` - An error that occurred.
     - `Critical` - A critical error requiring immediate action.

3. **`properties` (IDictionary<string, string>)**  
   - Additional key-value pairs to provide context, such as variable values, extra descriptions, or situational details.  
   - Example:
     ```csharp
     new Dictionary<string, string>
     {
         { "UserId", "12345" },
         { "RequestId", "abcde-12345" }
     }
     ```

---

### Examples of `TrackTrace` Usage

#### Example 1: Log a General Message
```csharp
_telemetryClient.TrackTrace(
    "Application started",
    SeverityLevel.Information
);
```

#### Example 2: Log a Message with Additional Data
```csharp
_telemetryClient.TrackTrace(
    "Processing request for user",
    SeverityLevel.Information,
    new Dictionary<string, string>
    {
        { "UserId", "12345" },
        { "Action", "Login" },
        { "Timestamp", DateTime.UtcNow.ToString("o") }
    }
);
```

#### Example 3: Log an Error
```csharp
try
{
    // Business logic
    throw new Exception("Database connection failed");
}
catch (Exception ex)
{
    _telemetryClient.TrackTrace(
        "An error occurred while processing the request",
        SeverityLevel.Error,
        new Dictionary<string, string>
        {
            { "ExceptionMessage", ex.Message },
            { "StackTrace", ex.StackTrace ?? "No stack trace available" },
            { "Timestamp", DateTime.UtcNow.ToString("o") }
        }
    );
    _telemetryClient.TrackException(ex); // Send exception directly
}
```

---

### Benefits of `TrackTrace`

1. **Helps in Debugging**  
   - Logs information about processes or errors for easy analysis later.

2. **Flexibility**  
   - Allows you to define messages and additional data as needed.

3. **Supports Monitoring**  
   - Integrates with Azure Application Insights for real-time or historical data viewing through dashboards.

4. **Severity Level Prioritization**  
   - Helps distinguish the importance of logs systematically.

---

### Viewing Data in Azure Application Insights
In the Azure Portal, you can view trace logs sent via `TrackTrace` under **Search** or **Live Metrics**. Logs can be filtered by **Severity Level** or **Properties**, making analysis more efficient and insightful.

#### Logging Example in Application Insights


To enhance the `_telemetryClient.TrackTrace(...)` logging for better detail and structure, you can improve it in the following ways:

1. **Provide More Contextual Information**: Add additional key-value pairs to the `properties` dictionary, such as user ID, timestamp, or additional request-specific metadata.

2. **Use a Structured Dictionary for Detailed Data**: Instead of a single key-value pair, add multiple entries for more granular logging.

3. **Ensure Consistent Logging Format**: Adopt a standard format for log messages across your application.

Here’s the refactored `TrackTrace` call with these improvements:

### 

```csharp
_telemetryClient.TrackTrace(
    $"{LoggingMessages.START_PROCESS}{processName} - Request initiated",
    SeverityLevel.Information,
    new Dictionary<string, string>
    {
        { "RequestData", modelString },
        { "Timestamp", DateTime.UtcNow.ToString("o") }, // ISO 8601 format for better readability
        { "UserId", HttpContext.User?.Identity?.Name ?? "Anonymous" }, // Example: capture user info
        { "ProcessName", processName },
        { "LogLevel", SeverityLevel.Information.ToString() }
    }
);
```

---

### Explanation of Changes

1. **Enhanced Log Message**: 
   - Updated the string message to indicate what the log represents (`Request initiated`).

2. **Additional Dictionary Entries**:
   - **`Timestamp`**: Adds a timestamp to help in correlating logs across systems.
   - **`UserId`**: If available, logs the user ID or indicates if the request is anonymous.
   - **`ProcessName`**: Explicitly logs the name of the process being tracked.
   - **`LogLevel`**: Explicitly records the severity level as a property.

3. **Consistent and Structured Format**:
   - Ensures all relevant data is included in the dictionary for easier parsing and filtering.

---

### Example Output in Application Insights

This approach ensures that logs will have a structured view in Application Insights:

```json
{
  "message": "DefaultApp Process Start: Get - Request initiated",
  "severityLevel": "Information",
  "properties": {
    "RequestData": "[{...model data...}]",
    "Timestamp": "2024-12-10T12:00:00Z",
    "UserId": "john.doe",
    "ProcessName": "Get",
    "LogLevel": "Information"
  }
}
```

### Benefits of Detailed Logging

1. **Improved Debugging**: Additional details provide more context to quickly pinpoint issues.
2. **Enhanced Observability**: The structured dictionary allows for powerful filtering and searching in Application Insights.
3. **Consistency**: Standardized logging across processes makes it easier to understand the application’s behavior.

---


#### Running the Project

1. **Setup Application Insights**:
   - Add your Application Insights connection string in `appsettings.json` under `ApplicationInsights:ConnectionString`.

2. **Run Locally**:
   - Use `dotnet run` to start the application.
   - Open the Swagger UI to test the `WeatherForecast` endpoint.

3. **Monitoring Logs**:
   - View logs and telemetry in the **Azure Application Insights** dashboard.

--- 

## Future Enhancements

1. Add more detailed log categories (e.g., `DEBUG`, `CRITICAL`).
2. Extend the `LoggingMessages` class with dynamic metadata.
3. Implement middleware for centralized logging.

---

## Contact

For questions or support, please contact:  
**Nuchit Atjanawat**  
**Email**: nuchit@outlook.com  
**GitHub**: [nuchit2019](https://github.com/nuchit2019)

--- 
