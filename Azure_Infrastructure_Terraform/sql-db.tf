resource "azurerm_mssql_server" "sql-server" {
  name                         = "sql-server-${var.app-name}"
  resource_group_name          = azurerm_resource_group.this.name
  location                     = azurerm_resource_group.this.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"
}

resource "azurerm_storage_account" "storage-account" {
  name                     = "storageaccountvittles"
  resource_group_name      = azurerm_resource_group.this.name
  location                 = azurerm_resource_group.this.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_mssql_database" "sql-db" {
  name           = "sql-db-${var.app-name}"
  server_id      = azurerm_mssql_server.sql-server.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  // license_type   = "LicenseIncluded"
  max_size_gb    = 1
  read_scale     = false
  sku_name       = "Basic"
  zone_redundant = false

}

resource "azurerm_mssql_virtual_network_rule" "sqlvnetrule" {
  name                = "sql-vnet-rule"
    server_id         = azurerm_mssql_server.sql-server.id
  subnet_id           = azurerm_subnet.subnet-sql-db.id
}