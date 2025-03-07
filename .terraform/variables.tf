variable "resource_group_name" {
  description = "The name of the resource group the variables need to be"
  default     = "CQRS_implementation_rg"
}


variable "location" {
  description = "The location of the Azure resources"
  default     = "northeurope"
}

variable "failover_location" {
  description = "The location for failover of Azure Resoures"
  default     = "northeurope"
}

variable "adgroup" {
  description = "The Ad group you want to give access to all your resources"
}

variable "cosmosdb_account_name" {
  description = "The Cosmos DB account name, max length 44 characters, lowercase"
  default     = "CQRS_implementation_cosmos"
}

variable "cosmosdb_account_sku" {
  description = "The SKU of the Cosmos DB Account"
  default     = "Standard"
}

variable "cosmosdb_name" {
  description = "The Cosmos DB database name"
  default     = "Gate"
}

variable "cosmosdb_employee_events_container_name" {
  description = "The Cosmos DB employee events container name."
  default     = "EmployeeEvents"
}

variable "cosmosdb_employee_unique_checks_container_name" {
  description = "The Cosmos DB employee unique checks container name."
  default     = "EmployeeUniqueChecks"
}

variable "cosmosdb_employee_read_container_name" {
  description = "The Cosmos DB employee read container name."
  default     = "EmployeeRead"
}

variable "cosmosdb_lease_container_name" {
  description = "The Cosmos DB lease container name."
  default     = "Leases"
}


variable "cosmosdb_max_throughput" {
  type        = number
  default     = 1000
  description = "Cosmos DB database max throughput"
  validation {
    condition     = var.cosmosdb_max_throughput >= 1000 && var.cosmosdb_max_throughput <= 1000000
    error_message = "Cosmos db autoscale max throughput should be equal to or greater than 1000 and less than or equal to 1000000."
  }
  validation {
    condition     = var.cosmosdb_max_throughput % 100 == 0
    error_message = "Cosmos db max throughput should be in increments of 100."
  }
}
