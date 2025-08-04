# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 9.0 solution for the EM Comax ShukHerzel system, which manages inventory synchronization between Comax ERP and ESL (Electronic Shelf Labels) systems. The solution consists of multiple projects following a layered architecture pattern.

## Solution Structure

- **Em.Comax.ShukHerzel.Client** - Windows Forms application (.NET 9.0-windows) for manual operations
- **EM.Comax.ShukHerzel.Service** - Background Windows Service (.NET 9.0) with Quartz.NET scheduled jobs
- **EM.Comax.ShukHerzel.Bl** - Business Logic layer with service interfaces and implementations
- **EM.Comax.ShukHerzel.Dal** - Data Access Layer with Entity Framework repositories
- **EM.Comax.ShukHerzel.Models** - Entity models, DTOs, and interfaces (includes auto-generated EF Core DbContext)
- **EM.Comax.ShukHerzel.Infrastructure** - Dependency injection and service registration
- **EM.Comax.ShukHerzel.Integration** - External API clients (Comax and ESL APIs)
- **EM.Comax.ShukHerzl.Common** - Shared constants and utilities
- **Em.Comax.ShukHerzel.installer** - MSI installer project
- **Em.Comax.ShukHerzel.WixInstaller** - WiX installer project

## Development Commands

### Build Commands
```bash
# Build entire solution
dotnet build Em.Comax.ShukHerzel.Client.sln

# Build specific project
dotnet build EM.Comax.ShukHerzel.Service\EM.Comax.ShukHerzel.Service.csproj

# Build for release
dotnet build -c Release
```

### Service Management
```bash
# Install Windows Service (run as administrator)
EM.Comax.ShukHerzel.Service.exe /install

# Uninstall Windows Service (run as administrator)
EM.Comax.ShukHerzel.Service.exe /uninstall

# Run service in console mode for debugging
dotnet run --project EM.Comax.ShukHerzel.Service
```

### Database Operations
- The system uses Entity Framework Core with SQL Server
- Database models are auto-generated using EF Core Power Tools
- Connection string is configured in appsettings.json under "ShukHerzelDb"
- Database schema includes multiple schemas: Management, Temp, Oper, Log

## Architecture Patterns

### Layered Architecture
- **Presentation Layer**: Windows Forms client application
- **Business Logic Layer**: Service classes implementing business rules
- **Data Access Layer**: Repository pattern with Entity Framework Core
- **Integration Layer**: External API clients with HttpClient

### Dependency Injection
All services are registered in `ServiceRegistration.cs`:
- Scoped services for request-scoped operations
- HttpClient services for API communication
- Database context registration with connection string

### Scheduled Jobs (Quartz.NET)
The Windows Service runs multiple scheduled jobs:
- **SyncItemsJob**: Synchronizes items from temporary tables to operational tables
- **TempCatalogJob**: Fetches catalog data from Comax API
- **PromotionJob**: Manages promotion data synchronization  
- **OperativeJob**: Handles operational data processing
- **PriceUpdateJob**: Processes price updates
- **MaintenanceJob**: Cleanup and maintenance tasks

Job schedules are configured in appsettings.json under "QuartzJobs" section using cron expressions.

### Database Schema Organization
- **Management schema**: Configuration, branches, companies
- **Temp schema**: Temporary import tables (allItems, promotions, PriceUpdates)
- **Oper schema**: Operational tables (items)
- **Log schema**: Logging tables (ErrorLog, ServiceLog, TraceLog, BadItemLog)

## Configuration Management

### Environment-Specific Settings
- `appsettings.json` - Development configuration
- `appsettingsDev.json` - Development environment overrides
- `appsettingsProd.json` - Production environment overrides

### Key Configuration Sections
- **ConnectionStrings**: Database connection string
- **OutputSettings**: API response directory path
- **QuartzJobs**: Cron expressions for scheduled jobs
- **MaintenanceSettings**: Retention policies for logs and temporary data
- **BatchSettings**: Batch processing sizes

## API Integration

### External APIs
- **Comax API**: ERP system integration for catalog and pricing data
- **ESL API**: Electronic Shelf Label system integration

### HTTP Clients
- Configured in ServiceRegistration.cs with appropriate timeouts
- API configurations stored in database Configuration table
- Base URLs and API keys managed through configuration services

## Service Installation

The service supports automatic installation with the following features:
- Copies executable to target directory: `C:\Program Files (x86)\EwaveMobile\Em.Comax.ShukHerzel.installer1\`
- Registers as Windows Service with automatic startup
- Configures Serilog for file and console logging
- Uses CliWrap for executing Windows service commands

## Data Flow

1. **Catalog Sync**: TempCatalogJob fetches items from Comax API → stores in Temp.allItems
2. **Item Processing**: SyncItemsJob processes Temp.allItems → transfers valid items to Oper.items
3. **ESL Integration**: OperativeJob sends processed items to ESL system
4. **Price Updates**: PriceUpdateJob handles price changes from Comax
5. **Promotions**: PromotionJob manages promotional pricing and rules

## Error Handling and Logging

- Serilog configured for both file and console logging
- Database logging through IDatabaseLogger interface
- Error logs stored in Log.ErrorLog table
- Bad item tracking in Log.BadItemLog for debugging data issues
- Service lifecycle events logged for monitoring

## Development Notes

- Entity Framework models are auto-generated - avoid manual edits to ShukHerzelEntities.cs
- Use repository pattern for database operations
- All external API calls should go through dedicated service classes
- Jobs use [DisallowConcurrentExecution] attribute to prevent overlapping executions
- Configuration uses Microsoft.Extensions.Configuration for environment-specific settings