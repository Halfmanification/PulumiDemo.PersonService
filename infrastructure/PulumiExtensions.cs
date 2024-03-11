using Pulumi;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web.Inputs;

namespace PulumiDemo.PersonService.Infrastructure;

public static class PulumiExtensions
{
    public static InputList<NameValuePairArgs> ToNameValueList(this Dictionary<Input<string>, Input<string>> dict) =>
        dict.Select(x => new NameValuePairArgs
        {
            Name = x.Key,
            Value = x.Value,
        }).ToList();
    
    public static Output<string> GetConnectionString(this StorageAccount storageAccount, string resourceGroupName)
    {
        return storageAccount.Name.Apply(storageAccountName =>
        {
            var primaryKey = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = storageAccountName,
            }).Apply(x => x.Keys[0].Value);

            return Output.Format($"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={primaryKey};EndpointSuffix=core.windows.net");
        });
    }
}