# On Merge To Main Workflow

This GitHub Actions workflow triggers when there's a push to the `main` branch or manually triggered via workflow_dispatch. It orchestrates a series of jobs including building and testing code, and deploying to different environments (dev, test, prod) based on the main branch.

## Trigger

This workflow is triggered:
- Automatically when there's a push to the `main` branch.
- Manually via workflow_dispatch.

## Jobs

### Build and Test

This job builds and tests the codebase. It uses the `build-and-test.yml` workflow located in the `.github/workflows` directory.

### DEV - Deploy

This job deploys the code to the development environment. It depends on the successful completion of the `build-and-test` job.

Inputs for this job:
- `environment`: Specifies the environment for deployment (dev).
- `azure_functionapp_name`: Specifies the name of the Azure Function App for the dev environment.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group for the dev environment.
- `azure_subscription_id`: Specifies the Azure subscription ID.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.

### TEST - Deploy

This job deploys the code to the test environment. It depends on the successful completion of the `deploy-to-dev` job.

Inputs for this job:
- `environment`: Specifies the environment for deployment (test).
- `azure_functionapp_name`: Specifies the name of the Azure Function App for the test environment.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group for the test environment.
- `azure_subscription_id`: Specifies the Azure subscription ID.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.

### PROD - Deploy

This job deploys the code to the production environment. It depends on the successful completion of the `deploy-to-test` job.

Inputs for this job:
- `environment`: Specifies the environment for deployment (prod).
- `azure_functionapp_name`: Specifies the name of the Azure Function App for the prod environment.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group for the prod environment.
- `azure_subscription_id`: Specifies the Azure subscription ID.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.

Each job relies on the successful completion of the previous job before execution, ensuring a controlled deployment process from development to production environments.
