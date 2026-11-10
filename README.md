# 🎓 Plantilla Didáctica: Web API en .NET 10 (Arquitectura N-Capas + ADO.NET & Stored Procedures)

Repositorio plantilla orientado a estudiantes que están dando sus primeros pasos en el desarrollo de APIs profesionales, conectándose a bases de datos relacionales **sin usar Entity Framework**, mediante **ADO.NET (`Microsoft.Data.SqlClient`)** y **Procedimientos Almacenados (Stored Procedures)** en SQL Server.

---

## 🎯 Objetivo Pedagógico

1. **Comprender el ciclo de vida del dato:** En lugar de ocultar las consultas detrás de un ORM (como EF Core), el alumno manipula directamente:
   - Conexiones (`SqlConnection`).
   - Comandos y tipos de ejecución (`SqlCommand`, `CommandType.StoredProcedure`).
   - Parámetros tipados (`SqlParameter`) para evitar inyecciones SQL.
   - Lectura registro a registro (`SqlDataReader`).
2. **Separación de responsabilidades (N-Capas):** Cada proyecto tiene un propósito claro e independiente.
3. **Buenas prácticas profesionales:** Inyección de Dependencias, manejo de configuraciones (`appsettings.json`) y respuestas estandarizadas (`ApiResponse<T>`).

---

## 🏗️ Arquitectura de la Solución (N-Capas)

```text
WebApiNCapas/
│
├── database/                                # Scripts SQL para el docente y alumnos
│   ├── 01_create_database_and_tables.sql    # Creación de base de datos y tablas
│   ├── 02_stored_procedures.sql             # Procedimientos almacenados CRUD
│   └── 03_seed_data.sql                     # Datos semilla de prueba
│
└── src/
    ├── WebApiNCapas.Entities/               # Capa Transversal (Modelos POCO y DTOs)
    │   ├── Producto.cs                      # Entidad central
    │   └── ApiResponse.cs                   # Envoltorio estándar de respuesta HTTP
    │
    ├── WebApiNCapas.Data/                   # Capa de Acceso a Datos (DAL)
    │   ├── DbConnectionFactory.cs           # Fábrica de conexiones SQL Server
    │   ├── IProductoRepository.cs           # Interfaz del repositorio
    │   └── ProductoRepository.cs            # Implementación con ADO.NET nativo
    │
    ├── WebApiNCapas.Business/               # Capa de Lógica de Negocio (BLL)
    │   ├── IProductoService.cs              # Interfaz de servicio
    │   └── ProductoService.cs               # Reglas y validaciones de negocio
    │
    └── WebApiNCapas.Api/                    # Capa de Presentación (REST API)
        ├── Controllers/
        │   └── ProductosController.cs       # Endpoints HTTP
        ├── Program.cs                       # Configuración e Inyección de Dependencias
        ├── appsettings.json                 # Cadena de conexión a SQL Server
        └── test-requests.http               # Archivo de pruebas rápidas en VS Code
```

### Flujo de Datos
```
[Cliente HTTP / Swagger / VS Code]
               ↓
     [WebApiNCapas.Api]          (Controlador: recibe HTTP y valida sintaxis)
               ↓
    [WebApiNCapas.Business]     (Servicio: aplica reglas de negocio / validaciones)
               ↓
      [WebApiNCapas.Data]        (Repositorio: ejecuta ADO.NET + Stored Procedures)
               ↓
    [SQL Server (EscuelaDb)]    (Motor de Base de Datos)
```

---

## 📋 Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (o .NET 9/8 con cambios menores de versión)
- [Visual Studio Code](https://code.visualstudio.com/)
  - Extensión recomendada: **C# Dev Kit** o **C#** (`ms-dotnettools.csdevkit` o `ms-dotnettools.csharp`)
  - Extensión recomendada: **REST Client** (`humao.rest-client`) para usar `test-requests.http`
  - Extensión recomendada: **SQL Server (mssql)** (`ms-mssql.mssql`)
- **SQL Server** (SQL Server Express o LocalDB)

---

## 🚀 Puesta en Marcha Rápida para el Alumno

### Paso 1: Clonar o descargar el repositorio
```bash
git clone https://github.com/tu-usuario/dotnet10-webapi-ncapas-template.git
cd dotnet10-webapi-ncapas-template
```

### Paso 2: Crear la Base de Datos y Stored Procedures
Ejecuta en tu gestor de base de datos (SSMS, Azure Data Studio o la extensión SQL Server de VS Code) los scripts en este orden:
1. `database/01_create_database_and_tables.sql`
2. `database/02_stored_procedures.sql`
3. `database/03_seed_data.sql`

### Paso 3: Configurar la Cadena de Conexión
Abre `src/WebApiNCapas.Api/appsettings.json` y revisa que coincida con tu instancia local:

- **Para SQL Server Express:**
  ```json
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EscuelaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  ```
- **Para LocalDB:**
  ```json
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EscuelaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  ```

### Paso 4: Compilar y Ejecutar la API
Desde la terminal:
```bash
# Compilar la solución
dotnet build

# Ejecutar el proyecto Web API
dotnet run --project src/WebApiNCapas.Api
```

### Paso 5: Probar los Endpoints
Abre el archivo `src/WebApiNCapas.Api/test-requests.http` en VS Code y haz clic en **"Send Request"** sobre cualquiera de las peticiones para probar el CRUD completo en tiempo real.

---

## 🛠️ Guía Didáctica: ¿Cómo se construyó este proyecto desde cero? (Comandos CLI)

Si deseas enseñar a tus alumnos a crear esta estructura desde una terminal en blanco paso a paso, estos son los comandos exactos utilizados:

```bash
# 1. Crear carpeta y solución
mkdir WebApiNCapas
cd WebApiNCapas
dotnet new gitignore
dotnet new sln -n WebApiNCapas

# 2. Crear las 4 capas de proyectos
dotnet new classlib -n WebApiNCapas.Entities -o src/WebApiNCapas.Entities -f net10.0
dotnet new classlib -n WebApiNCapas.Data -o src/WebApiNCapas.Data -f net10.0
dotnet new classlib -n WebApiNCapas.Business -o src/WebApiNCapas.Business -f net10.0
dotnet new webapi -n WebApiNCapas.Api -o src/WebApiNCapas.Api -f net10.0 --use-controllers

# 3. Vincular los proyectos a la solución
dotnet sln add src/WebApiNCapas.Entities/WebApiNCapas.Entities.csproj
dotnet sln add src/WebApiNCapas.Data/WebApiNCapas.Data.csproj
dotnet sln add src/WebApiNCapas.Business/WebApiNCapas.Business.csproj
dotnet sln add src/WebApiNCapas.Api/WebApiNCapas.Api.csproj

# 4. Establecer las dependencias entre capas (Referencias)
# Data conoce a Entities
dotnet add src/WebApiNCapas.Data/WebApiNCapas.Data.csproj reference src/WebApiNCapas.Entities/WebApiNCapas.Entities.csproj

# Business conoce a Entities y a Data
dotnet add src/WebApiNCapas.Business/WebApiNCapas.Business.csproj reference src/WebApiNCapas.Entities/WebApiNCapas.Entities.csproj
dotnet add src/WebApiNCapas.Business/WebApiNCapas.Business.csproj reference src/WebApiNCapas.Data/WebApiNCapas.Data.csproj

# Api conoce a Entities y a Business (Nunca directamente a Data)
dotnet add src/WebApiNCapas.Api/WebApiNCapas.Api.csproj reference src/WebApiNCapas.Entities/WebApiNCapas.Entities.csproj
dotnet add src/WebApiNCapas.Api/WebApiNCapas.Api.csproj reference src/WebApiNCapas.Business/WebApiNCapas.Business.csproj

# 5. Instalar paquete ADO.NET en la capa Data
dotnet add src/WebApiNCapas.Data/WebApiNCapas.Data.csproj package Microsoft.Data.SqlClient
dotnet add src/WebApiNCapas.Data/WebApiNCapas.Data.csproj package Microsoft.Extensions.Configuration.Abstractions

# 6. Compilar
dotnet build
```

---

## 💡 Conceptos Clave para la Clase

| Concepto | Dónde se ve | Explicación para los alumnos |
| :--- | :--- | :--- |
| **`SqlConnection`** | `Data/ProductoRepository.cs` | Representa la conexión física al motor. Debe cerrarse con `using` para no saturar el pool de conexiones. |
| **`CommandType.StoredProcedure`** | `Data/ProductoRepository.cs` | Le avisa al servidor SQL que el texto es el nombre de un SP precompilado y no una consulta en texto plano. |
| **`SqlParameter`** | `Data/ProductoRepository.cs` | Separa el código de los datos. Previene de raíz los ataques de **SQL Injection**. |
| **`SqlDataReader`** | `Data/ProductoRepository.cs` | Cursor de solo avance y solo lectura en memoria para obtener los registros rápidamente. |
| **`ExecuteScalarAsync`** | `Data/ProductoRepository.cs` | Ejecuta la consulta y devuelve únicamente la primera columna de la primera fila (ej: `SCOPE_IDENTITY()`). |
| **Validaciones de Negocio** | `Business/ProductoService.cs` | Evitan viajes innecesarios a la base de datos si los datos son inválidos (precio negativo, nombre vacío, etc.). |
