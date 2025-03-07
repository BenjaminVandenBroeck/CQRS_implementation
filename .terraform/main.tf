resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_role_assignment" "adgroup_resourcegroupno" {
  scope                = azurerm_resource_group.rg.id
  role_definition_name = "Contributor"
  principal_id         = var.adgroup
}

resource "azurerm_cosmosdb_account" "cosmosDbAccount" {
  name                = var.cosmosdb_account_name
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  offer_type          = var.cosmosdb_account_sku
  kind                = "GlobalDocumentDB"


  automatic_failover_enabled = true

  geo_location {
    location          = var.location
    failover_priority = 0
  }

  geo_location {
    location          = var.failover_location
    failover_priority = 1
  }

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }
}

resource "azurerm_cosmosdb_sql_database" "cosmosDb" {
  name                = var.cosmosdb_name
  resource_group_name = azurerm_resource_group.rg.name
  account_name        = azurerm_cosmosdb_account.cosmosDbAccount.name
  autoscale_settings {
    max_throughput = var.cosmosdb_max_throughput
  }
}


resource "azurerm_cosmosdb_sql_container" "cosmosDbEmployeeEventsContainer" {
  name                  = var.cosmosdb_employee_events_container_name
  resource_group_name   = azurerm_resource_group.rg.name
  account_name          = azurerm_cosmosdb_account.cosmosDbAccount.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosDb.name
  partition_key_paths   = ["/aggregateId"]
  partition_key_version = 1
  autoscale_settings {
    max_throughput = var.cosmosdb_max_throughput
  }
  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }
  }
  unique_key {
    paths = ["/aggregateId", "/version"]
  }
}

resource "azurerm_cosmosdb_sql_container" "cosmosDbEmployeeUniqueChecksContainer" {
  name                  = var.cosmosdb_employee_unique_checks_container_name
  resource_group_name   = azurerm_resource_group.rg.name
  account_name          = azurerm_cosmosdb_account.cosmosDbAccount.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosDb.name
  partition_key_paths   = ["/propertyName"]
  partition_key_version = 1
  autoscale_settings {
    max_throughput = var.cosmosdb_max_throughput
  }
  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }
  }
  unique_key {
    paths = ["/propertyName", "/uniqueValue"]
  }
}

resource "azurerm_cosmosdb_sql_container" "cosmosDbEmployeeReadContainer" {
  name                  = var.cosmosdb_employee_read_container_name
  resource_group_name   = azurerm_resource_group.rg.name
  account_name          = azurerm_cosmosdb_account.cosmosDbAccount.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosDb.name
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 1
  autoscale_settings {
    max_throughput = var.cosmosdb_max_throughput
  }
  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }
  }
}

resource "azurerm_cosmosdb_sql_container" "cosmosDbLeaseContainer" {
  name                  = var.cosmosdb_lease_container_name
  resource_group_name   = azurerm_resource_group.rg.name
  account_name          = azurerm_cosmosdb_account.cosmosDbAccount.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosDb.name
  partition_key_paths   = ["/id"]
  partition_key_version = 1
  autoscale_settings {
    max_throughput = var.cosmosdb_max_throughput
  }
  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }
  }
}
