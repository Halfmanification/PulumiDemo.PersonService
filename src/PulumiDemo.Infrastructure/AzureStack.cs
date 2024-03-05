using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

using StorageSkuArgs = Pulumi.AzureNative.Storage.Inputs.SkuArgs;
using StorageSkuName = Pulumi.AzureNative.Storage.SkuName;
using StorageKind = Pulumi.AzureNative.Storage.Kind;

namespace PulumiDemo.PersonService.Infrastructure;

public class AzureStack : Stack
{
    private readonly Names _names;

    public AzureStack()
    {
        _names = new Names("PulumiDemo.PersonService", "PDPerSer", "dev");

        CreateResources();
    }

    public void CreateResources()
    {
        var resourceGroup = new ResourceGroup(_names.ResourceGroup, new()
        {
            ResourceGroupName = _names.ResourceGroup,
            Location = Locations.WestEurope,
        });

        // var storageAccount = new StorageAccount(_names.StorageAccount, new()
        // {
        //     ResourceGroupName = resourceGroup.Name,
        //     AccountName = _names.StorageAccount,
        //     Location = resourceGroup.Location,
        //     Kind = StorageKind.StorageV2,
        //     Sku = new StorageSkuArgs
        //     {
        //         Name = StorageSkuName.Standard_LRS
        //     },
        //     EnableHttpsTrafficOnly = true,
        //     AllowBlobPublicAccess = false,
        //     MinimumTlsVersion = TLS_Versions.TLS1_2,
        // }, new() { DependsOn = { resourceGroup } });
    }
}