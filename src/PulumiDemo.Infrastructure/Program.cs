using Pulumi;

namespace PulumiDemo.PersonService.Infrastructure;

class Program
{
    static async Task<int> Main() => await Deployment.RunAsync<AzureStack>();

    // Test-comment in infrastructure, should trigger a deploy
}