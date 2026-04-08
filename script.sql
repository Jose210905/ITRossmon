CREATE DATABASE InventarioDB;
GO

USE InventarioDB;
GO

-- Tabla de usuarios 
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL DEFAULT 'User'
);
GO

-- Tabla de categorias
CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);
GO

-- Tabla de productos
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);
GO


--  Categorias

CREATE PROCEDURE sp_ObtenerCategorias
AS BEGIN
    SELECT Id, Nombre, Descripcion FROM Categorias;
END
GO

CREATE PROCEDURE sp_InsertarCategoria
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255)
AS BEGIN
    INSERT INTO Categorias (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO


--  Productos

CREATE PROCEDURE sp_ObtenerProductos
AS BEGIN
    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock,
           p.CategoriaId, c.Nombre AS CategoriaNombre, p.FechaCreacion
    FROM Productos p
    INNER JOIN Categorias c ON p.CategoriaId = c.Id;
END
GO

CREATE PROCEDURE sp_ObtenerProductoPorId
    @Id INT
AS BEGIN
    SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock,
           p.CategoriaId, c.Nombre AS CategoriaNombre, p.FechaCreacion
    FROM Productos p
    INNER JOIN Categorias c ON p.CategoriaId = c.Id
    WHERE p.Id = @Id;
END
GO

CREATE PROCEDURE sp_InsertarProducto
    @Nombre NVARCHAR(150),
    @Descripcion NVARCHAR(500),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @CategoriaId INT
AS BEGIN
    INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId)
    VALUES (@Nombre, @Descripcion, @Precio, @Stock, @CategoriaId);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE PROCEDURE sp_ActualizarProducto
    @Id INT,
    @Nombre NVARCHAR(150),
    @Descripcion NVARCHAR(500),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @CategoriaId INT
AS BEGIN
    UPDATE Productos
    SET Nombre = @Nombre, Descripcion = @Descripcion,
        Precio = @Precio, Stock = @Stock, CategoriaId = @CategoriaId
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_EliminarProducto
    @Id INT
AS BEGIN
    DELETE FROM Productos WHERE Id = @Id;
END
GO


-- Usuarios

CREATE PROCEDURE sp_ObtenerUsuarioPorUsername
    @Username NVARCHAR(50)
AS BEGIN
    SELECT Id, Username, PasswordHash, Rol
    FROM Usuarios
    WHERE Username = @Username;
END
GO

CREATE PROCEDURE sp_InsertarUsuario
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255),
    @Rol NVARCHAR(20)
AS BEGIN
    INSERT INTO Usuarios (Username, PasswordHash, Rol)
    VALUES (@Username, @PasswordHash, @Rol);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

