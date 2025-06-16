namespace webapi.Exceptions;

public class ConfigurationException(string appSettings) 
    : InvalidOperationException($"Configuration error: '{appSettings}' is missing or empty in appsettings.json.");