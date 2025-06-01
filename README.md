## Short description

A modern .NET 9 Web API for managing, sharing, and discovering sheet music. Features user authentication, PDF upload & download, favorites, search, and more.

## Features

- User registration, login, and profile management
- Upload, download, and delete sheet music (PDF)
- Generate PDF thumbnails automatically
- Search, filter, and sort sheet music by genre, instrument, difficulty, and more
- Favorite/unfavorite sheet music
- Track downloads and views
- Secure endpoints with JWT authentication

## Tech Stack

- .NET 9 (ASP.NET Core Web API)
- Entity Framework Core
- MS SQL Server
- RESTful API
- PDF processing for thumbnails
- JWT Authentication

## Prerequisites

- .NET SDK 9
- MSSQL Server

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/andrejkoller/NotoriumAPI.git
cd NotoriumAPI
```

### 2. Configure the database connection

Edit the connection string in `appsettings.json` or `appsettings.Development.json`:

```bash
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=NotoriumDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply database migration

Make sure the Entity Framework Core CLI is installed:

 ```bash
dotnet tool install --global dotnet-ef
```

Then apply the migrations:

 ```bash
dotnet ef database update
```

### 4. Run the API

 ```bash
dotnet run --project NotoriumAPI
```

The API will be available at `https://localhost:7189`.
