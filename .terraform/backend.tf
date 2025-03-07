terraform {
  backend "azurerm" {
    resource_group_name  = ""
    storage_account_name = ""
    container_name       = "statefile"
    key                  = "infrastructure/gate/local/shared.tfstate"
  }
}
