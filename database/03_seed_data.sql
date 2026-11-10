-- ===================================================================
-- 03_seed_data.sql
-- Propósito: Insertar registros de prueba para verificar el funcionamiento.
-- ===================================================================

USE EscuelaDb;
GO

-- Insertamos datos de prueba solo si la tabla está vacía
IF NOT EXISTS (SELECT 1 FROM Productos)
BEGIN
    INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, FechaCreacion)
    VALUES 
        ('Cuaderno Profesional', 'Cuaderno cuadro grande 100 hojas', 35.50, 50, GETDATE()),
        ('Bolígrafo Tinta Negra', 'Bolígrafo punto fino 0.7mm', 12.00, 120, GETDATE()),
        ('Calculadora Científica', 'Calculadora con 240 funciones para ingeniería', 280.00, 15, GETDATE()),
        ('Mochila Ergonómica', 'Mochila resistente al agua con compartimento para laptop', 650.00, 8, GETDATE());
END
GO
