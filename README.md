# Conductorly
Simple .NET command query dispatching service.

## Installation
```
Install-Package Conductorly
```

## Example usage

### Create your own handler implementations

#### Query
```csharp
// Conductorly.Abstractions.IQuery
public class MyQuery : IQuery<string>
{
}

// Conductorly.Abstractions.IQueryHandler
public class MyQueryHandler : IQueryHandler<MyQuery, string>
{
}
```

#### Command
```csharp
// Conductorly.Abstractions.ICommand
public class MyCommand : ICommand
{
}

// Conductorly.Abstractions.ICommandHandler
public class MyCommandHandler : ICommandHandler<MyCommand>
{
}
```

### Add .UseConductorly() to IHostBuilder
```csharp 
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseConductorly()
        .ConfigureServices((hostContext, services) =>
        {
           // Register your services
            services.AddScoped<ICommandHandler<MyCommand>, MyCommandHandler>();
            services.AddScoped<IQueryHandler<MyQuery, string>, MyQueryHandler>();

            ...
        });
```

### Use IConductorly.Send to call your handler
```csharp
// Conductorly.Abstractions.IConductorly
string result = await conductorly.Send(new MyQuery());
await conductorly.Send(new MyCommand());
```
