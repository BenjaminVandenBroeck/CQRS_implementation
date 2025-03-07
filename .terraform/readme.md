This is the terraform code for the Azure environment, to use it you must do the following:

1.  Have an active Azure subscription
1.  Have a storage account ready
1.  Have a container prepared

First make sure you authenticate to a user with rights using az login

To initialize terraform, you need to execute the following command

```
 terraform init \
    -backend-config="resource_group_name=RG_NAME" \
    -backend-config="storage_account_name=SA_NAME" \
    -backend-config="container_name=C_NAME"  \
    -backend-config="key=F_NAME"
```

where :

- RG_NAME is the name of your Azure resource group
- SA_NAME is the name of your storage account
- C_NAME is the name of the container where you want to host your statefil
- F_NAME is what you want your statefile to be called (this can include folders)

Once you've done that you can execute the following command to see what changes you want to deploy

```
terraform plan
```

use this command to actually deploy the changes

```
terraform apply
```
