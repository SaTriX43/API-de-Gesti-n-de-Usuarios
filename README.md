📌 API de Gestión de Usuarios
JWT + Refresh Token + Roles (sin Identity)
🧾 Descripción

API REST desarrollada en ASP.NET Core (.NET 8) que implementa un sistema de autenticación y autorización completo, utilizando JWT + Refresh Tokens, sin depender de ASP.NET Identity.

El proyecto maneja sesiones reales, con refresh tokens persistidos en base de datos, revocación, rotación y logout efectivo.

🚀 Tecnologías

ASP.NET Core Web API (.NET 8)

Entity Framework Core

SQL Server

JWT (Access Token + Refresh Token)

Serilog

BCrypt

Arquitectura por capas

Result Pattern

Swagger

🔐 Funcionalidades principales
Autenticación

Registro de usuarios

Login con JWT

Refresh token con rotación

Logout real (revocación de sesión)

Seguridad

Access token de corta duración

Refresh token persistido en BD

Uso único de refresh token

Revocación en logout

Manejo de expiraciones

Autorización

Roles: Admin, Usuario

Admin:

Listar todos los usuarios

Usuario:

Ver su propio perfil

📦 Endpoints principales
Auth
Método	Endpoint	Descripción
POST	/api/autenticacion/registro	Registro + sesión
POST	/api/autenticacion/login	Login
POST	/api/autenticacion/refreshToken	Renovar sesión
POST	/api/autenticacion/logout	Cerrar sesión
Usuarios
Método	Endpoint	Rol
GET	/api/usuario/obtener-usuarios	Admin
GET	/api/usuario/perfil	Usuario
🗃️ Modelo de datos

Usuarios

RefreshTokens

Relación 1:N con Usuario

Un refresh token solo puede usarse una vez

🛠️ Configuración

Clonar repositorio

Configurar appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=UsuariosJwtDb;Trusted_Connection=True;"
}


Ejecutar migraciones:

dotnet ef database update


Ejecutar proyecto:

dotnet run

🧠 Notas técnicas

No se utiliza ASP.NET Identity

El refresh token representa la sesión

El access token solo se usa para autorización

El logout invalida la sesión en base de datos

Manejo global de errores con middleware

📌 Estado del proyecto

✅ Funcional
✅ Estable
✅ Listo para portafolio

👤 Autor

Santiago González
Backend .NET Trainee / Junior