<p align="center">
    <img src="logo.png" width="100" height="100">
</p>

# Moravian Star - Demo

A multi-project .NET 10 application demonstrating a simple demo system with web, mobile, maintenance and job components using [Moravian Star](https://github.com/lneshev/MoravianStar) library.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/lneshev/MoravianStar-Demo/blob/main/LICENSE)

## Table of Contents

- [Overview](#overview)
- [Technology stack](#technology-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Architecture](#architecture)
- [License](#license)
- [Contributing](#contributing)
- [Support](#support)

## Overview

Moravian Star - Demo is a modular .NET 10 solution featuring multiple layers including core business logic, services, data access, and web APIs for web, mobile, maintenance and job operations.

## Technology stack
- ASP.NET Core 10 Web API
- Entity Framework Core 10
- SQL Server 
- Hangfire
- GraphQL via Hot Chocolate
- Moravian Star
- NetTopologySuite
- LinqKit
- Swashbuckle
- Elmah Core
- Newtonsoft.Json

## Project Structure

### Common Projects
These projects provide shared functionality across the solution:

- **MoravianStar-Demo.Common.Core** - Shared core business logic and entities
- **MoravianStar-Demo.Common.Services** - Shared service implementations
- **MoravianStar-Demo.Common.DataAccess** - Data access layer and repositories
- **MoravianStar-Demo.Common.Jobs** - Job scheduling and background task implementations

### Web Projects
Web-focused modules for web applications:

- **MoravianStar-Demo.Web.Core** - Web-specific core logic
- **MoravianStar-Demo.Web.Services** - Web service implementations
- **MoravianStar-Demo.Web.WebAPI** - Web API endpoints

### Mobile Projects
Mobile application modules:

- **MoravianStar-Demo.Mobile.Core** - Mobile-specific core logic
- **MoravianStar-Demo.Mobile.Services** - Mobile service implementations
- **MoravianStar-Demo.Mobile.WebAPI** - Mobile API endpoints

### Maintenance Projects
Administrative and maintenance operations:

- **MoravianStar-Demo.Maintenance.Core** - Maintenance core logic
- **MoravianStar-Demo.Maintenance.Services** - Maintenance service implementations
- **MoravianStar-Demo.Maintenance.WebAPI** - Maintenance API endpoints

### Job Projects
Background job processing:

- **MoravianStar-Demo.Job.WebAPI** - Job service API endpoints

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 (or later) or Visual Studio Code

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/lneshev/MoravianStar-Demo.git
   cd MoravianStar-Demo
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run tests (if applicable):
   ```bash
   dotnet test
   ```

## Architecture

The solution follows a layered architecture pattern:
- **Core** layers contain business logic and domain entities
- **Services** layers implement business operations and orchestration
- **DataAccess** layer handles database interactions
- **WebAPI** layers expose REST endpoints
- **Job** layer handles background job processing

Each module (Web, Mobile, Maintenance and Job) can be deployed and scaled independently.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/lneshev/MoravianStar-Demo/blob/main/LICENSE) file for details.

## Contributing

Contributions, issues and feedbacks are welcome! Please feel free to submit an issue, pull request or start a discussion in the corresponding repository. If it is about the demo, please use this [repository](https://github.com/lneshev/MoravianStar-Demo). If it is about the library, please use the [Moravian Star](https://github.com/lneshev/MoravianStar) repository.

## Support

- 📖 [Documentation](https://github.com/lneshev/MoravianStar-Demo)
- 🐛 [Issue Tracker](https://github.com/lneshev/MoravianStar-Demo/issues)