terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.16.0"
    }
  }

    backend "azurerm" {
    resource_group_name  = "terraform_backends"
    storage_account_name = "terraformbackends19874"
    container_name       = "azuredevopsfutures"
    key                  = "containerkey"
  }
}

provider "azurerm" {
  features {

  }
}


