# Prueba Técnica Fullstack – Sistema de Gestión de Tareas

## Descripción

Se desarrolló una aplicación fullstack para la gestión interna de tareas de una empresa. El sistema permite crear usuarios, asignar tareas y realizar seguimiento del estado de cada tarea, incluyendo información adicional almacenada en formato JSON en SQL Server.

La solución está compuesta por:

- Backend en .NET 8 Web API
- Frontend en Angular
- Base de datos SQL Server
- Integración continua con GitHub Actions

---

## Arquitectura

### Backend (.NET 8)

Se implementó una arquitectura por capas inspirada en Clean Architecture:

- Domain
- Application
- Infrastructure
- API

Principales decisiones técnicas:

- Uso de Dapper para acceso a datos.
- Implementación de autenticación JWT.
- Middleware global para manejo estructurado de errores.
- Validaciones con FluentValidation.
- Documentación con Swagger/OpenAPI incluyendo autenticación Bearer.

---

### Frontend (Angular)

Desarrollado con:

- Standalone Components
- Lazy Loading
- Guards de autenticación
- Interceptor para inyección automática del JWT
- Formularios reactivos
- Bootstrap 5 (diseño responsive)
- Componentes reutilizables (modal de confirmación, modal de detalle y notificaciones)

---



### Base de Datos (SQL Server)

Se utilizó SQL Server con:

- Claves primarias y foráneas
- Índices
- Validación de JSON con ISJSON
- Uso de JSON_VALUE para filtrado por prioridad
- Constraint que garantiza que ExtraData sea JSON válido

La información adicional de las tareas se almacena en la columna:

ExtraData NVARCHAR(MAX)

Ejemplo de contenido almacenado:

```json

{
  "priority": "High",
  "estimatedFinishDate": "2026-02-27",
  "tags": ["urgente", "cliente"],
  "metadata": {
    "module": "tasks"
  }
}
```

Ejecución y Compilación del Proyecto

Base de Datos

Ejecutar el script ubicado en:

database/SQLQuery1_Gestion_tareas.sql

Este script crea la base de datos, tablas, relaciones, índices y la validación del campo JSON.

---

Compilación del Backend

Desde la carpeta:

backend/TaskManagementAPI

Compilar el proyecto ejecutando:


dotnet restore
dotnet build

Swagger quedará disponible en:

https://localhost:7137/swagger

Credenciales de prueba:

Usuario: admin
Password: admin123

---

Compilación del Frontend

Desde la carpeta:

frontend/task-management-ui

Instalar dependencias y compilar:

npm install
npm run build

Para ejecutar en entorno de desarrollo:

ng serve

La aplicación estará disponible en:

http://localhost:4200

---
