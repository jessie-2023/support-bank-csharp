using SupportBank.SupportBank; 
using Microsoft.Extensions.DependencyInjection; // ServiceCollection()
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

var servicesProvider = new ServiceCollection() // from Microsoft.Extensions.DependencyInjection
    .AddTransient<Entrance>() // Adds a transient dependency for the Bank class. Transient means a new instance will be created every time it's requested
    .AddLogging(loggingBuilder => // Configures logging services
    {
        loggingBuilder.ClearProviders(); // Clears any existing logging providers (useful when you want to replace the default providers)
        loggingBuilder.AddNLog(); // Adds NLog as a logging provider
    })
    .BuildServiceProvider(); // Constructs a service provider based on the configured services

var entrance = servicesProvider.GetRequiredService<Entrance>(); // Retrieves an instance of the App class from the service provider
entrance.Run(); // Calls the Run method on the App instance, presumably initiating your application logic
