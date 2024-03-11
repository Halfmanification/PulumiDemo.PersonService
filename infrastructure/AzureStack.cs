using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

using StorageSkuArgs = Pulumi.AzureNative.Storage.Inputs.SkuArgs;
using StorageSkuName = Pulumi.AzureNative.Storage.SkuName;
using StorageKind = Pulumi.AzureNative.Storage.Kind;
using AspDescriptionArgs = Pulumi.AzureNative.Web.Inputs.SkuDescriptionArgs;

namespace PulumiDemo.PersonService.Infrastructure;

public class AzureStack : Stack
{
    private readonly String _env;
    private readonly Names _names;

    public AzureStack()
    {
        _env = Environment.GetEnvironmentVariable("DEPLOY_ENVIRONMENT") ?? throw new ArgumentNullException("DEPLOY_ENVIRONMENT is not set.");
        _names = new Names("PulumiDemo.PersonService", "PDPerSer", _env);

        CreateResources();
    }

    public void CreateResources()
    {
        var resourceGroup = new ResourceGroup(_names.ResourceGroup, new()
        {
            ResourceGroupName = _names.ResourceGroup,
            Location = Locations.WestEurope,
        });

        var storageAccount = new StorageAccount(_names.StorageAccount, new()
        {
            ResourceGroupName = resourceGroup.Name,
            AccountName = _names.StorageAccount,
            Location = resourceGroup.Location,
            Kind = StorageKind.StorageV2,
            Sku = new StorageSkuArgs
            {
                Name = StorageSkuName.Standard_LRS
            },
            EnableHttpsTrafficOnly = true,
            AllowBlobPublicAccess = false,
            MinimumTlsVersion = TLS_Versions.TLS1_2,
        }, new() { DependsOn = { resourceGroup } });

        var storageAccountConnectionString = storageAccount.GetConnectionString(_names.ResourceGroup);

        var appServicePlan = new AppServicePlan(_names.AppServicePlan, new()
        {
            Kind = "FunctionApp",
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Name = _names.AppServicePlan,
            Sku = new AspDescriptionArgs
            {
                Name = "Y1",
                Tier = "Dynamic",
            }
        }, new() { DependsOn = { resourceGroup } });

        var functionApp = new WebApp(_names.FunctionApp, new()
        {
            Kind = "functionapp",
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            ServerFarmId = appServicePlan.Id,
            HttpsOnly = true,
            Name = _names.FunctionApp,
            SiteConfig = new SiteConfigArgs
            {
                AppSettings = new Dictionary<Input<string>, Input<string>>()
                {
                    { "AzureWebJobsStorage", storageAccountConnectionString },
                    { "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING", storageAccountConnectionString },
                    { "WEBSITE_CONTENTSHARE", _names.FunctionApp },
                    { "FUNCTIONS_WORKER_RUNTIME", "dotnet" },
                    { "FUNCTIONS_EXTENSION_VERSION", "~4" }
                }.ToNameValueList()
            }
        }, new() { DependsOn = { storageAccount, appServicePlan } });
    }
}