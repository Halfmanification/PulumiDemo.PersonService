using Pulumi;

namespace PulumiDemo.PersonService.Infrastructure;

class Program
{
    static Task<int> Main() => Deployment.RunAsync<AzureStack>();
}