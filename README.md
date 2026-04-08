#Gestion de Inventario

Hice este proyecto con .NET 8 para el backend, SQL Server para la base de datos y Angular para el frontend. Tambien implemente autenticacion con JWT.

## Instalado

- .NET 8
- Node.js
- Angular CLI (npm install -g @angular/cli)
- SQL Server con SSMS

## Pasos para correrlo

**Base de datos**

Primero abre SSMS y ejecuta el script.sql que esta en la carpeta database. Ese script crea la base de datos, las tablas y los stored procedures que use.

Despues a InventarioAPI/appsettings.json y cambia el Server por el nombre de tu instancia de SQL Server:


**Backend**

```
cd InventarioAPI
dotnet run
```

**Frontend**

```
cd inventario-frontend
npm install
ng serve --no-ssr
```

Luego abre http://localhost:4200 en el navegador.

## Como usarlo

Te registras con un usuario, inicias sesion y ya puedes ver, agregar y editar productos y categorias.
