using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // builder.Services.AddSingleton<IMyService>((s) => {
        //     return new MyService();
        // });

        // builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();

        // This change should NOT trigger a build
    }
}