using Pulumi;
using Pulumi.AzureNative.Resources;

namespace PulumiDemo.PersonService.Infrastructure;

public class AzureStack : Stack
{
    private readonly string _env = "dev";

    public AzureStack()
    {

    }

    public void CreateResources()
    {
        var resourceGroup = new ResourceGroup("resourceGroup", new()
        {
            ResourceGroupName = $"zebra-personservice-{_env}-rg",
            Location = "West Europe",
        });
    }
}