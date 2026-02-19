CREATE DATABASE TaskManagementDB;
GO

-- Tabla de usuarios
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    CreateDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);
GO


-- Tabla de tareas
CREATE TABLE dbo.Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500) NULL,
    Status NVARCHAR(20) NOT NULL,
    UserId INT NOT NULL,
    ExtraData NVARCHAR(MAX) NULL,
    CreateDate DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),

    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId)
        REFERENCES dbo.Users(Id),

    CONSTRAINT CK_Tasks_Status CHECK (Status IN ('Pending', 'InProgress', 'Done')),

    CONSTRAINT CK_Tasks_ExtraData_JSON CHECK (ExtraData IS NULL OR ISJSON(ExtraData) = 1)
);
GO

-- Índice para consultas 
CREATE INDEX IX_Tasks_UserId_Status_CreateDate
ON dbo.Tasks(UserId, Status, CreateDate DESC);
GO



-- Inserción de datos usuarios
INSERT INTO dbo.Users (FullName, Email)
VALUES 
('Juan Perez', 'juan.perez@pruebas.com'),
('Maria Lopez', 'maria.lopez@pruebas.com');
GO

-- Inserción de tareas por usuario
INSERT INTO dbo.Tasks (Title, Description, Status, UserId, ExtraData)
VALUES
(
  'Implementación API para usuarios',
  'Crear enpoints para usuarios',
  'Pending',
  1,
  '{"priority":"High","estimatedFinishDate":"2026-02-20","tags":["backend","api"],"metadata":{"module":"users"}}'
),
(
  'Implementación API para tareas',
  'Crear enpoints para tareas',
  'InProgress',
  2,
  '{"priority":"Medium","estimatedFinishDate":"2026-02-21","tags":["backend"],"metadata":{"module":"tasks"}}'
);
GO


-- Consulta de usuario y tareas
DECLARE @UserId INT = 1;
DECLARE @Status NVARCHAR(20) = 'Pending';

SELECT 
    t.Id,
    t.Title,
    t.Description,
    t.Status,
    t.CreateDate,
    u.FullName,
    u.Email
FROM dbo.Tasks t
INNER JOIN dbo.Users u ON u.Id = t.UserId
WHERE t.UserId = @UserId
  AND t.Status = @Status
ORDER BY t.CreateDate DESC;


-- Consulta del JSON al varchar por la prioridad
SELECT 
    Id,
    Title,
    JSON_VALUE(ExtraData, '$.priority') AS Priority
FROM dbo.Tasks;


-- Consulta JSON por prioridad alta 
SELECT 
    Id,
    Title,
    Status,
    ExtraData
FROM dbo.Tasks
WHERE JSON_VALUE(ExtraData, '$.priority') = 'High';


-- Consulta JSON traer etiquetas
SELECT 
    Id,
    Title,
    JSON_QUERY(ExtraData, '$.tags') AS Tags
FROM dbo.Tasks;

-- Consulta de etiquetas por lista
SELECT 
    t.Id,
    t.Title,
    tags.[value] AS Tag
FROM dbo.Tasks t
CROSS APPLY OPENJSON(t.ExtraData, '$.tags') tags;

-- Consulta donde se filtra por etiqueta que tengas tag=backend
SELECT 
    t.Id,
    t.Title,
    t.Status
FROM dbo.Tasks t
WHERE EXISTS (
    SELECT 1
    FROM OPENJSON(t.ExtraData, '$.tags') j
    WHERE j.[value] = 'backend'
);


-- Cosnulta previa para saber el estado de las tareas
SELECT * FROM dbo.Tasks

-- Consulta donde se actualiza la prioridad de una tarea
UPDATE dbo.Tasks
SET ExtraData = JSON_MODIFY(ExtraData, '$.priority', 'High')
WHERE Id = 2;

-- consulta que me permite null para la prioridad
ALTER TABLE dbo.Tasks
ADD CONSTRAINT CK_Tasks_ExtraData_JSON
CHECK (ExtraData IS NULL OR ISJSON(ExtraData) = 1);
GO

-- Consulta para que quede columna calculada de tamaño fijo
ALTER TABLE dbo.Tasks
ADD Priority AS CAST(JSON_VALUE(ExtraData, '$.priority') AS NVARCHAR(20));
GO



