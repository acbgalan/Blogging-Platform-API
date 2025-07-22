# Blogging-Platform-API 🇬🇧    
Create a RESTful API for a personal blogging platform.

Backend project proposal for [roadmap.sh](https://roadmap.sh). Full project details can be found at [Blogging Platform API](https://roadmap.sh/projects/blogging-platform-api)   

Developed by Carlos Blanco. 📧acbgalan@gmail.com  🌐 www.carlosblanco.dev


### 🚀 Technologies used
- ASP.NET Core 9
- Entity Framework Core
- SQL Server
- AutoMapper v15 (requires a license)


### 🔐 AutoMapper License Configuration

Get your free automapper licence from [automapper.io](https://automapper.io/) 

Provide your license key using User Secrets.

```json
{
  "AutoMapper": {
    "LicenseKey": "your key"
  }
}
```

### 📦 Installation and Execution
- Restore NuGet packages
- Run Entity Framework migrations to create the database
- Make sure the startup project is BloggingPlatform.Server (Web API)
- Run the API from Visual Studio 2022 to test its functionality


### 🏭 Project Structure

📁 BloggingPlatform.Data  
├── 📂 Context          - Entity Framework context  
├── 📂 Entities         - Domain entities  
├── 📂 Migrations       - Database migrations  
└── 📂 Repositories     - Data access repositories  

📁 BloggingPlatform.Server  
├── 📂 Controllers      - Controllers with endpoints  
└── 📂 Mapper           - AutoMapper configurations  

📁 BloggingPlatform.Shared  
├── 📂 Requests         - DTOs for API requests  
└── 📂 Responses        - DTOs for API responses  

---  

# Blogging-Platform-API 🇪🇸    
Crea una API RESTful para una plataforma de blogs personales.

Propuesta de proyecto backend de [roadmap.sh](https://roadmap.sh). Los detalles completos del proyecto pueden consultarse en [Blogging Platform API](https://roadmap.sh/projects/blogging-platform-api)

Desarrollado por Carlos Blanco. 📧acbgalan@gmail.com  🌐 www.carlosblanco.dev


### 🚀 Tecnologías utilizadas
- ASP.NET Core 9
- Entity Framework Core
- SQL Server
- AutoMapper v15 (requiere licencia)


### 🔐 Configuración de licencia automapper

Obten tu licencia gratuita de automapper desde [automapper.io](https://automapper.io/) 

Introduce tu licencia mediante User Secrets.

```json
{
  "AutoMapper": {
    "LicenseKey": "your key"
  }
}
```

### 📦 Instalación y ejecución
- Restaura paquetes NuGet
- Ejecuta las migraciones de Entity Framework para crear la base de datos.
- Asegurate que el proyecto de inicio es BloggingPlatform.Server (Web API)
- Ejecuta la API desde Visual Studio 2022 para probar su funcionamiento

### 🏭 Estructura del proyecto

📁 BloggingPlatform.Data  
├── 📂 Context          - Contexto de Entity Framework  
├── 📂 Entities         - Entidades del dominio  
├── 📂 Migrations       - Migraciones de la base de datos  
└── 📂 Repositories     - Repositorios de acceso a datos

📁 BloggingPlatform.Server  
├── 📂 Controllers      - Controladores con endpoints REST  
└── 📂 Mapper           - Configuraciones de AutoMapper  

📁 BloggingPlatform.Shared  
├── 📂 Requests         - DTOs para solicitudes a la API  
└── 📂 Responses        - DTOs para respuestas desde la API  