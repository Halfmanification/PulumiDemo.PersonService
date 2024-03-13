# Azure Setup

To deploy your infrastructure to Azure using GitHub Actions and Pulumi, you need to set up an Azure Active Directory app registration and assign appropriate roles to it.

## 1. Create Azure Active Directory App Registration

1. **Navigate to Azure Active Directory**: Go to the [Azure portal](https://portal.azure.com/) and navigate to Azure Active Directory.

2. **Create App Registration**: Under the "App registrations" section, create a new registration.

3. **Record Application (client) ID**: After the registration is created, note down the "Application (client) ID". This will be used as part of the authentication process. You can find it in the app registration overview.

4. **Generate Client Secret**: Generate a client secret for the app registration. Note down the generated secret as it will be required for authenticating with Azure services.

5. **Record Directory (tenant) ID**: Note down the "Directory (tenant) ID" from the app registration overview. This ID is unique to your Azure Active Directory instance.

6. **Record Subscription ID**: Note down the "Subscription ID" from your Azure subscription settings. This ID is associated with your Azure subscription.

7. **Create Azure Credentials JSON**: Create a JSON file containing the following fields:
```json
{
  "clientId": "YOUR_CLIENT_ID",
  "clientSecret": "YOUR_CLIENT_SECRET",
  "tenantId": "YOUR_TENANT_ID",
  "subscriptionId": "YOUR_SUBSCRIPTION_ID"
}
```
## 2. Assign Roles to App Registration

1. **Navigate to Access Control (IAM)**: In your Azure subscription, navigate to "Access Control (IAM)".

2. **Add Role Assignment**: Click on "Add a role assignment" to assign a role to your app registration.

3. **Choose the Contributor Role**: Select the "Contributor" role from the list of roles. This role grants permissions to manage resources within Azure.

4. **Save the Role Assignment**: Once you've selected the role and app registration, save the role assignment to apply the permissions.

5. **Repeat for Storage Account Contributor Role**: Navigate to your storage account in the Azure portal. Go to the "Access control (IAM)" section within the storage account.

6. **Add Role Assignment**: Click on "Add a role assignment" to assign a role to your app registration.

7. **Choose the Storage Blob Data Contributor Role**: Select the "Storage Blob Data Contributor" role from the list of roles. This role grants permissions to read and write blobs in the storage account.

8. **Save the Role Assignment**: Once you've selected the role and app registration, save the role assignment to apply the permissions.

By granting both the "Contributor" and "Storage Blob Data Contributor" roles to your app registration, you ensure that it has the necessary permissions to manage resources within Azure and access blobs within the storage account, including the Pulumi state files needed for managing your infrastructure.

### 2.1 Alternative way using az cli
As an alternative to manually set this up in the Azure Portal, this is how you can do the same via command line.

1. Ensure you are logged in
```shell
az account show || az login # This will only run az login if you are not already logged in
```
2. Create Service Principal (*You can name this whatever you want, and can use a single service principal for all repos, or create individual for each repository*)
```shell
az ad sp create-for-rbac \
   --name "<your-sp-name>" \ # i.e. 'pulumi-deploy-dev'
   --role "Contributor" \
   --scopes "subscriptions/<subscription-id>"

# Returns
# {
#   "appId": "<app-id (guid)>",
#   "displayName": "<your-sp-name>",
#   "password": "<service-principle-password>",
#   "tenant": "<tenant-id>"
# }
```
3. Copy the entire json result from the previous step and use the values from it to create this json:
```json
{
  "clientId": "<app-id (guid)>",
  "clientSecret": "<service-principle-password>",
  "tenantId": "<tenant-id>",
  "subscriptionId": "<subscription-id>"
}
```
4. This is the json you need in the next section, under the "GitHub Setup" guide.
5. We also need to give the service principal access to write files to the central storage account where all the pulumi stacks and metadata files are stored.
```shell
az role assignment create \
   --role "Storage Blob Data Contributor" \
   --assignee "<app-id (guid)>" \
   --scope "subscriptions/<subscription-id>/resourceGroups/<pulumi-storage-account>/providers/Microsoft.Storage/storageAccounts/<pulumi-container>"
```
6. That's it for az cli commands! Take the json from step 3 and continue to the "GitHub Setup" section.

# GitHub Setup

To deploy your infrastructure to Azure using GitHub Actions and Pulumi, you need to set up any relevant secrets and/or variables in your GitHub repository.

## Azure Credentials Secret(s)

1. **Create Azure Credentials JSON**: Create a JSON file containing the following fields:
```json
{
  "clientId": "YOUR_CLIENT_ID",
  "clientSecret": "YOUR_CLIENT_SECRET",
  "tenantId": "YOUR_TENANT_ID",
  "subscriptionId": "YOUR_SUBSCRIPTION_ID"
}
```
2. **Add GitHub secret**: In your repository settings, under 'Secrets and variables -> Actions' set up a new Secret called 'AZURE_CREDENTIALS'. This can either be a 'Repository Secret' if you only intend to deploy to a single azure subscription/environment, or you can do this whole process once per environment you intend to deploy to and store the azure credentials as multiple 'Environment Secrets'.

