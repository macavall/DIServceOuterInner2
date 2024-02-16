using System;
using Microsoft.Extensions.DependencyInjection;

// Interface for the inner service
public interface IInnerService
{
    void InnerServiceMethod();
}

// Implementation of the inner service
public class InnerService : IInnerService
{
    public void InnerServiceMethod()
    {
        Console.WriteLine("Inner service method called.");
    }
}

// Class that depends on the inner service
public class OuterService
{
    private readonly IInnerService _innerService;

    // Constructor injection
    public OuterService(IInnerService innerService)
    {
        _innerService = innerService;
    }

    public void OuterServiceMethod()
    {
        Console.WriteLine("Outer service method called.");

        // Creating a new DI container within OuterService
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var innerService = serviceProvider.GetService<IInnerService>();
        innerService.InnerServiceMethod();
    }

    // Configures services for the inner DI container
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IInnerService, InnerService>();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Setup DI container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // Build ServiceProvider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve OuterService and call methods
        var outerService = serviceProvider.GetRequiredService<OuterService>();
        outerService.OuterServiceMethod();

        Console.ReadLine();
    }

    // Configures services for the main DI container
    static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<OuterService>();
    }
}