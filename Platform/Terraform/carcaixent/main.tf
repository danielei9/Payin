# PROVIDER
provider "azurerm" {
  # whilst the `version` attribute is optional, we recommend pinning to a given version of the Provider
  version = "1.23.0"
  subscription_id = "227b7f68-70d9-4ebe-9595-8b0006b616c8"
}
# VARIABLES
variable "name" { default = "carcaixent" }
variable "administrator_sql" { default = "Administrador" }
variable "password" { }
# RESOURCE GROUP
resource "azurerm_resource_group" "resource_gp" {
    name = "${var.name}",
    location = "westeurope"
}
# SQL AZURE
resource "azurerm_sql_server" "sql_server" {
  name                         = "payin-${var.name}"
  resource_group_name          = "${azurerm_resource_group.resource_gp.name}"
  location                     = "${azurerm_resource_group.resource_gp.location}"
  version                      = "12.0"
  administrator_login          = "${var.administrator_sql}"
  administrator_login_password = "${var.password}"
}
resource "azurerm_sql_database" "database" {
  name                = "PayIn"
  resource_group_name = "${azurerm_resource_group.resource_gp.name}"
  location            = "${azurerm_resource_group.resource_gp.location}"
  server_name         = "${azurerm_sql_server.sql_server.name}"
}
resource "azurerm_storage_account" "storage_account" {
  name                     = "payin${var.name}"
  resource_group_name      = "${azurerm_resource_group.resource_gp.name}"
  location                 = "${azurerm_resource_group.resource_gp.location}"
  account_tier             = "Standard"
  account_kind             = "StorageV2"
  account_replication_type = "LRS"
}
# AZURE STORAGE
resource "azurerm_storage_container" "versions" {
  name                  = "versions"
  resource_group_name   = "${azurerm_resource_group.resource_gp.name}"
  storage_account_name  = "${azurerm_storage_account.storage_account.name}"
  container_access_type = "private"
}
# CLOUD SERVICE
# Deprecated