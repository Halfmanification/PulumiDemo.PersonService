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

# GitHub Setup

To deploy your infrastructure to Azure using GitHub Actions and Pulumi, you need to set up secrets and variables in your GitHub repository.

## 1. Azure Credentials

1. **Create Azure Credentials JSON**: Create a JSON file containing the following fields:
```json
{
  "clientId": "YOUR_CLIENT_ID",
  "clientSecret": "YOUR_CLIENT_SECRET",
  "tenantId": "YOUR_TENANT_ID",
  "subscriptionId": "YOUR_SUBSCRIPTION_ID"
}
```

## 2. Pulumi Storage Account Name

**Define Pulumi Azure Storage Account Name**: Define the name of your Pulumi Azure Storage Account as a GitHub repository variable.
   - Go to your GitHub repository > Settings > Secrets.
   - Create a new repository variable named `PULUMI_AZURE_STORAGE_ACCOUNT`.
   - Set its value to your Pulumi Azure Storage Account name.

# Documentation of deploy-infrastructure.yml

This GitHub Actions workflow automates the deployment of infrastructure to Azure using Pulumi.

## Workflow Trigger

The workflow is triggered by the following events:
- Manual trigger via the "workflow_dispatch" event.
- Push events to the "main" branch, which include changes to the workflow file or files within the `src/**/*.Infrastructure/` directory.

## Environment Variables

- **AZURE_STORAGE_ACCOUNT**: This variable specifies the name of the Azure Storage Account where Pulumi stacks are stored.

## Jobs

### Deploy

This job handles the deployment of infrastructure using Pulumi.

#### Conditions

The job runs only when a push event occurs on the "main" branch in certain files/folders, or if manually triggered in GitHub by a user.

#### Operating System

The job runs on Ubuntu latest.

#### Steps

1. **Checkout code**: Utilizes the `actions/checkout` action to clone the repository into the runner.

2. **Parse Azure Credentials into Environment Variables**:
   - This step extracts the necessary Azure credentials from the `AZURE_CREDENTIALS` secret and sets them as environment variables.
     - `AZURE_CREDENTIALS`: Secret containing the Azure service principal credentials in JSON format.
     - `ARM_CLIENT_ID`: Azure service principal client ID.
     - `ARM_CLIENT_SECRET`: Azure service principal client secret.
     - `ARM_SUBSCRIPTION_ID`: Azure subscription ID.
     - `ARM_TENANT_ID`: Azure tenant ID.

3. **Find Work Directory**:
   - Searches for the directory ending with ".Infrastructure" and sets it as the Pulumi work directory.
   - The directory path is stored in the `PULUMI_WORK_DIR` environment variable.

4. **Setup Dotnet**:
   - Sets up the .NET environment with version 8.0.x.

5. **Login to Azure**:
   - Uses the `azure/login@v2` action to log in to Azure using the provided credentials.

6. **Check if Pulumi CLI is installed**:
   - Checks if the Pulumi CLI is installed on the runner.
   - If the Pulumi CLI is not found, the job continues to the next step to install it.

7. **Install Pulumi CLI**:
   - Installs the Pulumi CLI using the `curl` command.
   - The installation path is added to the GitHub path.

8. **Pulumi Login**:
   - Logs in to Pulumi using the Azure Blob Storage backend URL.

9. **Get pulumi stack name**:
   - Extracts the Pulumi project name from the Pulumi configuration file (`Pulumi.yml`) located in the Pulumi work directory.
   - The project name is stored in the `PULUMI_PROJECT_NAME` environment variable.

10. **Ensure Pulumi Stack**:
    - Selects or initializes the Pulumi stack with the specified project name in the Pulumi work directory.

11. **Pulumi Up (Deploy)**:
    - Uses the `pulumi/actions@v4` action to execute the Pulumi `up` command.
    - Deploys the infrastructure defined in the Pulumi project.
    - Specifies the Pulumi work directory, stack name, cloud URL, and secrets provider.
    - The passphrase for the secrets provider is provided as an environment variable.

The workflow automates the deployment process, ensuring consistent and reliable infrastructure provisioning on Azure with Pulumi.
