-- ===================================================================
-- 01_create_database_and_tables.sql
-- Propósito: Crear la base de datos de práctica y la tabla de Productos.
-- Didáctico: Para alumnos principiantes.
-- ===================================================================

-- 1. Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EscuelaDb')
BEGIN
    CREATE DATABASE EscuelaDb;
END
GO

USE EscuelaDb;
GO

-- 2. Crear tabla Productos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
BEGIN
    CREATE TABLE Productos (
        Id INT IDENTITY(1,1) PRIMARY KEY,      -- Identificador autonumérico
        Nombre NVARCHAR(100) NOT NULL,          -- Nombre del producto
        Descripcion NVARCHAR(250) NULL,         -- Descripción opcional
        Precio DECIMAL(10,2) NOT NULL,          -- Precio unitario con 2 decimales
        Stock INT NOT NULL DEFAULT 0,           -- Cantidad disponible en inventario
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
GO
