resource "azurerm_resource_group" "this" {
  name     = "rg-${var.app-name}"
  location = "West Europe"
}