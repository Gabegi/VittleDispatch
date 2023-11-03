

resource "azurerm_service_plan" "this" {
  name                = "asp-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  os_type             = "Windows"
  sku_name            = "F1"
}

resource "azurerm_windows_web_app" "app-frontend" {
  name                = "app-frontend-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  service_plan_id     = azurerm_service_plan.this.id

  virtual_network_subnet_id = azurerm_subnet.subnet-app-services.id

  site_config {
    always_on = false

              virtual_application {
              physical_path = "site\\wwwroot"
              preload       = false
              virtual_path  = "/"
            }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"                  = "3a437649-558f-4bb3-ad24-0a12359a7d57"
    "APPINSIGHTS_PROFILERFEATURE_VERSION"             = "1.0.0"
    "APPINSIGHTS_SNAPSHOTFEATURE_VERSION"             = "1.0.0"
    "APPLICATIONINSIGHTS_CONNECTION_STRING"           = "InstrumentationKey=3a437649-558f-4bb3-ad24-0a12359a7d57;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/"
    "ApplicationInsightsAgent_EXTENSION_VERSION"      = "~2"
    "DiagnosticServices_EXTENSION_VERSION"            = "~3"
    "InstrumentationEngine_EXTENSION_VERSION"         = "disabled"
    "SnapshotDebugger_EXTENSION_VERSION"              = "disabled"
    "XDT_MicrosoftApplicationInsights_BaseExtensions" = "disabled"
    "XDT_MicrosoftApplicationInsights_Java"           = "1"
    "XDT_MicrosoftApplicationInsights_Mode"           = "recommended"
    "XDT_MicrosoftApplicationInsights_NodeJS"         = "1"
    "XDT_MicrosoftApplicationInsights_PreemptSdk"     = "disabled"
  }

         sticky_settings {
          app_setting_names       = [
              "APPINSIGHTS_INSTRUMENTATIONKEY",
              "APPLICATIONINSIGHTS_CONNECTION_STRING ",
              "APPINSIGHTS_PROFILERFEATURE_VERSION",
              "APPINSIGHTS_SNAPSHOTFEATURE_VERSION",
              "ApplicationInsightsAgent_EXTENSION_VERSION",
              "XDT_MicrosoftApplicationInsights_BaseExtensions",
              "DiagnosticServices_EXTENSION_VERSION",
              "InstrumentationEngine_EXTENSION_VERSION",
              "SnapshotDebugger_EXTENSION_VERSION",
              "XDT_MicrosoftApplicationInsights_Mode",
              "XDT_MicrosoftApplicationInsights_PreemptSdk",
              "APPLICATIONINSIGHTS_CONFIGURATION_CONTENT",
              "XDT_MicrosoftApplicationInsightsJava",
              "XDT_MicrosoftApplicationInsights_NodeJS",
            ]
          
       }
}

resource "azurerm_windows_web_app" "app-backend" {
  name                = "app-backend-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  service_plan_id     = azurerm_service_plan.this.id
  
  virtual_network_subnet_id = azurerm_subnet.subnet-app-services.id

  site_config {
    always_on = false
              virtual_application {
              physical_path = "site\\wwwroot"
              preload       = false
              virtual_path  = "/"
            }
  }

  app_settings = {

    "APPINSIGHTS_INSTRUMENTATIONKEY"                  = "3a437649-558f-4bb3-ad24-0a12359a7d57"
    "APPINSIGHTS_PROFILERFEATURE_VERSION"             = "1.0.0"
    "APPINSIGHTS_SNAPSHOTFEATURE_VERSION"             = "1.0.0"
    "APPLICATIONINSIGHTS_CONNECTION_STRING"           = "InstrumentationKey=3a437649-558f-4bb3-ad24-0a12359a7d57;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/"
    "ApplicationInsightsAgent_EXTENSION_VERSION"      = "~2"
    "DiagnosticServices_EXTENSION_VERSION"            = "~3"
    "InstrumentationEngine_EXTENSION_VERSION"         = "disabled"
    "SnapshotDebugger_EXTENSION_VERSION"              = "disabled"
    "XDT_MicrosoftApplicationInsights_BaseExtensions" = "disabled"
    "XDT_MicrosoftApplicationInsights_Java"           = "1"
    "XDT_MicrosoftApplicationInsights_Mode"           = "recommended"
    "XDT_MicrosoftApplicationInsights_NodeJS"         = "1"
    "XDT_MicrosoftApplicationInsights_PreemptSdk"     = "disabled"
  }

       sticky_settings {
          app_setting_names       = [
              "APPINSIGHTS_INSTRUMENTATIONKEY",
              "APPLICATIONINSIGHTS_CONNECTION_STRING ",
              "APPINSIGHTS_PROFILERFEATURE_VERSION",
              "APPINSIGHTS_SNAPSHOTFEATURE_VERSION",
              "ApplicationInsightsAgent_EXTENSION_VERSION",
              "XDT_MicrosoftApplicationInsights_BaseExtensions",
              "DiagnosticServices_EXTENSION_VERSION",
              "InstrumentationEngine_EXTENSION_VERSION",
              "SnapshotDebugger_EXTENSION_VERSION",
              "XDT_MicrosoftApplicationInsights_Mode",
              "XDT_MicrosoftApplicationInsights_PreemptSdk",
              "APPLICATIONINSIGHTS_CONFIGURATION_CONTENT",
              "XDT_MicrosoftApplicationInsightsJava",
              "XDT_MicrosoftApplicationInsights_NodeJS",
            ]
          
       }
}

resource "azurerm_log_analytics_workspace" "workspace" {
  name                = "workspace-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "app-insights" {
  name                = "app-insights-${var.app-name}"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  workspace_id        = azurerm_log_analytics_workspace.workspace.id
  application_type    = "web"
}