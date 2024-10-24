# E-commerce API with .NET Core

Este proyecto es una API RESTful desarrollada con .NET Core para gestionar un sistema de e-commerce. Implementa una arquitectura limpia y utiliza las mejores prácticas de desarrollo.

## 🚀 Características

- CRUD completo para productos y usuarios
- Autenticación JWT
- Manejo de roles y permisos
- Stored Procedures con Entity Framework
- Manejo global de errores
- Auditoría automática de entidades
- Soft Delete implementado
- Documentación con Swagger

## 🛠️ Tecnologías

- .NET 8.0
- Entity Framework Core
- SQL Server
- JWT para autenticación
- Swagger/OpenAPI
- AutoMapper

## 📁 Estructura del Proyecto

```
├── src/
│   ├── API/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   │   ├── Entities/
│   │   │   ├── DTOs/
│   │   ├── Data/
│   │   │   ├── Repositories/
│   │   ├── Middleware/
│   │   ├── Extensions/
│   │   ├── Common/
│   │   │   ├── Exceptions/
│   │   │   ├── Constants/
```

## 📋 Prerrequisitos

- .NET 7.0 SDK
- SQL Server
- Visual Studio 2022 o VS Code

## 🔧 Instalación

1. Clonar el repositorio
```bash
git clone https://github.com/jonatanmedina12/Project-App-Net-EcommerceApi.git
```

2. Restaurar dependencias
```bash
dotnet restore
```

3. Actualizar la cadena de conexión en `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=YourDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

4. Aplicar migraciones
```bash
dotnet ef database update
```

5. Ejecutar el proyecto
```bash
dotnet run
```

## 📦 Base de Datos

El proyecto utiliza Entity Framework Core con SQL Server. Las migraciones se aplican automáticamente al iniciar la aplicación.

### Entidades Principales

- Users
- Roles
- Products
- UserRoles

### Stored Procedures

El proyecto utiliza stored procedures para operaciones complejas. Implementados a través de una clase helper personalizada:

```csharp
public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params SqlParameter[] parameters)
```

## 🔒 Autenticación

El sistema utiliza JWT para autenticación. Los tokens se configuran en `Program.cs`:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        // Configuración del token
    });
```

## 🚦 Endpoints Principales

### Productos
- GET /api/products - Obtener todos los productos
- GET /api/products/{id} - Obtener producto por ID
- POST /api/products - Crear nuevo producto
- PUT /api/products/{id} - Actualizar producto
- DELETE /api/products/{id} - Eliminar producto

### Usuarios
- POST /api/auth/register - Registrar usuario
- POST /api/auth/login - Login de usuario
- GET /api/users - Obtener usuarios
- GET /api/users/{id} - Obtener usuario por ID

## 🧪 Testing

Para ejecutar los tests:

```bash
dotnet test
```

## 📄 Logging

El proyecto utiliza el sistema de logging integrado de .NET Core. Los logs se configuran en `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## 🔍 Swagger

La documentación de la API está disponible en `/swagger` cuando se ejecuta en modo desarrollo.



## 📝 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para detalles

