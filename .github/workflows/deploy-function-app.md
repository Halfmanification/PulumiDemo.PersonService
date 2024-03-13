# Deploy Function App Workflow

This GitHub Actions workflow automates the deployment of a Function App to Azure. It facilitates the deployment process by utilizing Dotnet CLI and Azure CLI commands to package and deploy the Function App to the specified Azure resource group.

## Trigger

This workflow is triggered when it is called. It requires the following input parameters:
- `environment`: Specifies the environment for deployment.
- `azure_functionapp_name`: Specifies the name of the Azure Function App.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group.
- `azure_subscription_id`: Specifies the Azure subscription ID.

## Jobs

### Deploy Function App

This job executes the deployment process on a Windows environment.

1. **Checkout code**:
   - Uses the GitHub Actions action `checkout@v4` to checkout the repository's source code.

2. **Login to Azure**:
   - Utilizes the `azure/login@v2` action to log in to Azure using the specified credentials stored in GitHub Secrets.
   - Enables AzPSSession for PowerShell Session.

3. **Build source code with Release configuration**:
   - Changes directory to the specified `azure_functionapp_package_path`.
   - Builds the source code with the `dotnet build` command using the Release configuration.
   - Publishes the build output to the './output' directory with the `dotnet publish` command.
   - Compresses the published output into a zip file named after the Azure Function App.
   - Changes directory back to the original location.

4. **Deploy Function App**:
   - Deploys the packaged Function App to Azure using Azure CLI commands.
   - Utilizes the `az functionapp deployment source config-zip` command to deploy the zip file to the specified Azure Function App name, resource group, and subscription ID.

5. **Logout**:
   - Logs out from the Azure CLI session.

This workflow automates the deployment process, ensuring that the Function App is deployed correctly to the specified Azure environment.
