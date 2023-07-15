#!/bin/bash

# Set the name of the resource group you want to delete
resourceGroup="robazresourcegroup"

az group list -o Table

# Check if the resource group exists
if [ $(az group exists --name $resourceGroup) = true ]; then
  # If the resource group exists, delete it
  echo "Deleting resource group $resourceGroup ..."
  az group delete --name $resourceGroup --yes
  echo "Resource group $resourceGroup deleted."
else
  # If the resource group doesn't exist, display a message
  echo "Resource group $resourceGroup does not exist."
fi

