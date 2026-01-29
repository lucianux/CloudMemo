# CloudMemo

Lightweight .NET 8 service for persistent string storage in Azure. Reliable, serverless-style persistence for small data snippets.

## Key Features
* **Built with .NET 8**: Leveraging the latest Minimal API features.
* **Azure App Configuration**: Uses Azure's native configuration store for 100% data persistence.
* **Cloud-Ready**: Optimized for Azure App Service (Free and Shared tiers).
* **Memory Efficient**: Avoids local storage or complex DB setups.

## How it works
The application uses the `Azure.Data.AppConfiguration` SDK to write and read values directly from the Azure Cloud, ensuring the data survives app restarts or plan idling.

## Environment Variables
To run this project, you will need to add the following variable to your environment:
`ConnectionStrings__AzureAppConfig`: Your Azure App Configuration connection string.
