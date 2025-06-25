QA Project Level6 - Backend

This is the backend for the application. A RESTful API built with **.NET 8 Web API** and **SQLite**. It handles user authentication and CRUD operations for feeds.

## Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Local Setup

### Configuration
Make sure your app settings have the following:
```
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localTest.db"
  },
  "JwtSettings": {
    "Key": "this-is-a-fake-secret-key-just-for-development-purposes_X82v5QPZs1L7JkUq4Zt3gyCiZxv",
    "Issuer": "https://localhost:44324",
    "Audience": "https://localhost:44324",
    "ExpireMinutes": "60"
  }
```

### Running the web API locally
```
cd .\backend\
dotnet build
dotnet run
```

Then visit: https://localhost:44324/swagger/

## Run Tests
```
dotnet test
```
