-- ===================================================================
-- 02_stored_procedures.sql
-- Propósito: Definir los Procedimientos Almacenados (CRUD)
-- Didáctico: Los alumnos aprenderán el uso de parámetros de entrada (@)
-- y retorno de identificadores con SCOPE_IDENTITY().
-- ===================================================================

USE EscuelaDb;
GO

-- 1. SP: Obtener todos los productos
CREATE OR ALTER PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        Id,
        Nombre,
        Descripcion,
        Precio,
        Stock,
        FechaCreacion
    FROM Productos
    ORDER BY Id DESC;
END
GO

-- 2. SP: Obtener un producto por su Id
CREATE OR ALTER PROCEDURE sp_ObtenerProductoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        Id,
        Nombre,
        Descripcion,
        Precio,
        Stock,
        FechaCreacion
    FROM Productos
    WHERE Id = @Id;
END
GO

-- 3. SP: Crear un nuevo producto (Retorna el Id generado)
CREATE OR ALTER PROCEDURE sp_CrearProducto
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250) = NULL,
    @Precio DECIMAL(10,2),
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, FechaCreacion)
    VALUES (@Nombre, @Descripcion, @Precio, @Stock, GETDATE());

    -- Retorna el Id autogenerado por IDENTITY
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NuevoId;
END
GO

-- 4. SP: Actualizar un producto existente
CREATE OR ALTER PROCEDURE sp_ActualizarProducto
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250) = NULL,
    @Precio DECIMAL(10,2),
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Precio = @Precio,
        Stock = @Stock
    WHERE Id = @Id;

    -- @@ROWCOUNT indica cuántas filas fueron afectadas
    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- 5. SP: Eliminar un producto
CREATE OR ALTER PROCEDURE sp_EliminarProducto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Productos
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO
