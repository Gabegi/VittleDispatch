resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  address_space       = ["10.0.0.0/16"]
  dns_servers         = ["10.0.0.4", "10.0.0.5"]

}


resource "azurerm_subnet" "subnet-app-services" {
  name                 = "subnet-${var.app-name}-app-services"
  resource_group_name  = azurerm_resource_group.this.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]

    delegation {
    name = "delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
      
    }
  }
}

resource "azurerm_subnet" "subnet-sql-db" {
  name                 = "subnet-${var.app-name}-sql-db"
  resource_group_name  = azurerm_resource_group.this.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.2.0/24"]
  service_endpoints    = ["Microsoft.Sql"]
}