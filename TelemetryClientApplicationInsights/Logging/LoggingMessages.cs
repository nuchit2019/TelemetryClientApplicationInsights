namespace LoggingAppInsights.Logging
{
    public static class LoggingMessages
    {

        public static string ApplicationName { get; private set; } = "DefaultApp";

        public static void Initialize(IConfiguration configuration)
        {
            ApplicationName = configuration["ApplicationLogName"] ?? "DefaultApp";
        }
         
        public static string FILTER_PROCESS => $"{ApplicationName} Process";
        public static string START_PROCESS => $"{ApplicationName} Process Start: ";
        public static string WARNING_PROCESS => $"{ApplicationName} Process Warning: ";
        public static string SUCCESS_PROCESS => $"{ApplicationName} Process Success: ";
        public static string EXCEPTION_PROCESS => $"{ApplicationName} Process Exception:";
    }

}
