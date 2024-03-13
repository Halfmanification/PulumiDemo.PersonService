# Deploy To Environment Workflow

This GitHub Actions workflow orchestrates the deployment of a Function App along with its required infrastructure to Azure. It is part of a broader deployment process that involves deploying infrastructure using Pulumi and then deploying the Function App.

## Trigger

This workflow is triggered when it is called. It requires the following input parameters:
- `environment`: Specifies the environment for deployment.
- `azure_functionapp_name`: Specifies the name of the Azure Function App.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group.
- `azure_subscription_id`: Specifies the Azure subscription ID.

## Jobs

### Deploy Infrastructure

This job deploys the infrastructure required for the Function App using Pulumi. It depends on the `deploy-infrastructure` reusable workflow, which is not detailed here. 

Inputs for this job:
- `environment`: Specifies the environment for deployment.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.

### Deploy Service

This job deploys the Function App. It depends on the successful completion of the `deploy-infrastructure` job before proceeding.

Inputs for this job:
- `environment`: Specifies the environment for deployment.
- `azure_functionapp_name`: Specifies the name of the Azure Function App.
- `azure_functionapp_package_path`: Specifies the path to the package containing the Azure Function App code.
- `azure_resourcegroup_name`: Specifies the name of the Azure resource group.
- `azure_subscription_id`: Specifies the Azure subscription ID.
- `pulumi_azure_storage_account`: Specifies the name of the Azure Storage Account used by Pulumi for state storage.

Both jobs call other YAML workflows to perform their tasks. The `deploy-infrastructure` job calls a reusable workflow to deploy infrastructure using Pulumi, while the `deploy-service` job calls a workflow to deploy the Function App.