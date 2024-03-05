using Pulumi;

namespace PulumiDemo.PersonService.Infrastructure;

class Program
{
    static async Task<int> Main() => await Deployment.RunAsync<AzureStack>();
}