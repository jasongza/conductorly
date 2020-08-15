# Conductorly
Simple .NET command query dispatching service.

![Nuget](https://img.shields.io/nuget/v/Conductorly)
![test](https://github.com/jasongza/Conductorly/workflows/test/badge.svg)

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
    public Task<string> Handle(MyQuery query)
    {
        ...
    }
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
    public Task Handle(MyCommand request)
    {
        ...
    }
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

### Use IConductorly.Send(...) to call your handler
```csharp
// Conductorly.Abstractions.IConductorly
string result = await conductorly.Send(new MyQuery());
await conductorly.Send(new MyCommand());
```

### Use .With(...), .Decorate(...) & .Start() to chain logic to your calls

#### Query
```csharp
var result = await conductorly
    .With<MyQuery, string>(new MyQuery())
    .Decorate(async (query, next) =>
    {
        // Wrapped handler
        return await next.Handle(query);
    })
    .Decorate(async (query, next) =>
    {
        // Wrapped decorator
        return await next.Handle(query);
    })
    .Start();
```

#### Command
```csharp
await conductorly
    .With(new MyCommand())
    .Decorate(async (command, next) => 
    {
        // Wrapped handler
        await next.Handle(command);
    })
    .Decorate(async (command, next) =>
    {
        // Wrapped decorator
        await next.Handle(command);
    })
    .Start();
```
