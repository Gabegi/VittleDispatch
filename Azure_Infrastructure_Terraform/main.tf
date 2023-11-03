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
    key                  = "EVaiLs6prh0wZzJA/4Z6oPJzcdkbBZ2QP5WGSODAjn8t6t3Db3ADrwH0U8jlbpIxitqT8RH9M4zo+ASt1pWEww=="
  }
}

provider "azurerm" {
  features {

  }
}


