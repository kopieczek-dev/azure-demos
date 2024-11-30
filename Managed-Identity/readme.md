Demonstration of using managed identity to access a blob. 
Different identity for local development and in the cloud. 

Use *.gif from the Assets folder or feel free to use any other file, but remember to change config.

## Connecting to Blob Storage with Access Key
1. Create a Storage Account
2. Create a Container in this Storage Account (private - no anonymous access)
3. Upload an asset to the Container (from [Assets](/Assets/))
4. Go to the **Security and networking** > **Access keys** and copy the **Connection string** value.
5. Open the [solution](/ManagedIdentityDemo/) and edit `Blob` confguration in `appsettings.json` 
   1. `ConnectionString`
   2. `ContainerName` - from step 2
   3. `BlobName` (`kitty.gif` if using example asset)
   4. `AccountUrl` - "`ttps://<storage-account-name>.blob.core.windows.net`,
6. Run the solution and verify whether image/gif was displayed
7. Change the `AccessKey` inside the `ConnectionString` to validate the connection  

## Connecting to Blob Storage with Managed Identity
1. Remove the `ConnectionString` value from the configuration
2. Comment the line 23 and uncomment lines 26-28.
3. Run the application. It should result in error as the Service Principal is not yet connected.
4. Create an **App Service** in Azure Portal and wait for it to be deployed.
5. Open the newly created **App Service** and go to **Settings** > **Identity** and enable **System assigned** identity.
6. Go to the Storage Account created earlier and open **Access Control (IAM)**
7. **Add** > **Add role assignement** and select **Storage Blob Data Reader** and go **Next**
8. Select **Managed identity** and find the managed identity for the App service created earlier. Select it and finish role assigned. 
9. Deploy the application to the App Service. Application should be able to connected to Blob Store with the `DefaultAzureCredential`

## Use Service Principal for local development
**Prerequsities:** [Azure CLi](https://learn.microsoft.com/en-us/cli/azure/) installed on the maching and authenticated account ([instructions](https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli-interactively)) 
1. In CLI run `az ad sp create-for-rbac -n <your-application-name> --skip-assignment` and provide your unique name iinstead `<your-application-name>`.
2. Paste the configuration into to [`launchSettings.json`](/ManagedIdentityDemo/ManagedIdentityDemo/Properties/launchSettings.json)
3. Finally, add role assignement for the newly created service principal to the Storage Account. Select a **Service Principal** this time.
4. Wait a couple of minutes and run the application locally to verify whether image/gif was displayed