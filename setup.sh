#!/bin/bash

resourceGroup="robazresourcegroup3"
location="eastus"
redisCacheName="robazrediscachebasic"
location="eastus"

# Careful here because this is charged monthly and can get quite expensive.
sku="Basic"
vmSize=c0
capacity=1

# Check if you're logged in to Azure
if [ -z "$(az account show --query user.name)" ]; then
  # If you're not logged in, perform az login
  az login
else
  # If you're logged in, display a message
  echo "You're already logged in to Azure as $(az account show --query user.name -o tsv)."
fi


az group create --name $resourceGroup --location $location

# Create Redis cache (capacity in Gigabytes)
az redis create \
  --name $redisCacheName \
  --resource-group $resourceGroup \
  --location $location \
  --sku $sku \
  --vm-size $vmSize

# Get Redis cache primary key -- connection string not easily available... needs more research
connectionString=$(az redis list-keys --name $redisCacheName --resource-group $resourceGroup --query 'primaryKey' -o tsv)

echo "Azure Cache for Redis resource created successfully."
echo "Connection string (not really... more like the primary key): $connectionString"
