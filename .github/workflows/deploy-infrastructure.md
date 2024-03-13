# Deploy Infrastructure Workflow

This GitHub Actions workflow automates the deployment of infrastructure using Pulumi to manage Azure resources. It configures and deploys infrastructure stacks defined in the specified Pulumi project directory.

## Trigger

This workflow is triggered when it is called. It requires the following input parameters:
- `environment`: Specifies the environment for deployment.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used for storing Pulumi state.

## Jobs

### Deploy Infrastructure

This job executes the deployment process on an Ubuntu environment.

1. **Checkout code**:
   - Uses the GitHub Actions action `checkout@v4` to checkout the repository's source code.

2. **Parse Azure Credentials into Environment Variables**:
   - Extracts Azure credentials from the GitHub Secrets and sets them as environment variables using `jq` and `echo`.

3. **Login to Azure**:
   - Utilizes the `azure/login@v2` action to log in to Azure using the specified credentials stored in GitHub Secrets.

4. **Check if Pulumi CLI is installed**:
   - Checks if the Pulumi CLI is installed. If not, proceeds to install it.

5. **Install Pulumi CLI**:
   - Installs the Pulumi CLI if it is not found on the runner.

6. **Set Azure Storage Account environment variable**:
   - Sets the `AZURE_STORAGE_ACCOUNT` environment variable with the provided value of `pulumi_azure_storage_account`.

7. **Pulumi Login**:
   - Logs in to Pulumi using the `pulumi login` command with the specified cloud URL for state storage.

8. **Get pulumi stack name**:
   - Extracts the Pulumi stack name from the `Pulumi.yml` file and sets it as an environment variable.

9. **Ensure Pulumi Stack**:
   - Selects or creates the Pulumi stack using the `pulumi stack select` command.

10. **Pulumi Up (Deploy)**:
    - Executes the `pulumi up` command to deploy the infrastructure defined in the Pulumi project directory.
    - Utilizes the `pulumi/actions@v5` action for deployment, specifying the command, working directory, stack name, cloud URL for state storage, and secrets provider.

This workflow automates the deployment process of infrastructure using Pulumi, ensuring consistent and reliable deployments across different environments.
