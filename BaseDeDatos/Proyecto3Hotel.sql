/* Base de Datos del Proyecto 3 - Hospedajes de Hotel Guanacaste. SQL Server 2022 Express.
   Programador: Felipe Brenes Conejo 
   Segundo Cuatrimestre 2026
*/
IF DB_ID(N'HotelProyecto3') IS NULL CREATE DATABASE HotelProyecto3;
GO
USE HotelProyecto3;
GO

CREATE TABLE Clientes (
    Identificacion NVARCHAR(50) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
    TipoIdentificacion NVARCHAR(30) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    PrimerApellido NVARCHAR(75) NOT NULL,
    SegundoApellido NVARCHAR(75) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    CONSTRAINT CK_Clientes_FechaNacimiento CHECK (FechaNacimiento >= '18000101' AND FechaNacimiento < CAST(GETDATE() AS DATE))
);

CREATE TABLE Empleados (
    Identificacion NVARCHAR(20) NOT NULL CONSTRAINT PK_Empleados PRIMARY KEY,
    TipoIdentificacion NVARCHAR(30) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Apellidos NVARCHAR(150) NOT NULL,
    FechaNacimiento DATE NOT NULL CONSTRAINT CK_Empleados_FechaNacimiento CHECK (FechaNacimiento >= '18000101' AND FechaNacimiento < CAST(GETDATE() AS DATE)),
    Salario DECIMAL(12,2) NOT NULL CONSTRAINT CK_Empleados_Salario CHECK (Salario >= 0),
    FechaIngreso DATE NOT NULL,
    Categoria NVARCHAR(60) NOT NULL,
    Ubicacion NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(150) NOT NULL
);

CREATE TABLE Habitaciones (
    Numero INT NOT NULL CONSTRAINT PK_Habitaciones PRIMARY KEY,
    Tipo NVARCHAR(50) NOT NULL,
    Tarifa DECIMAL(12,2) NOT NULL CONSTRAINT CK_Habitaciones_Tarifa CHECK (Tarifa >= 50 AND Tarifa <= 800),
    TieneTV BIT NOT NULL,
    Mantenimiento NVARCHAR(500) NOT NULL CONSTRAINT DF_Habitaciones_Mantenimiento DEFAULT N''
);

CREATE TABLE Reservaciones (
    CodigoReservacion NVARCHAR(20) NOT NULL CONSTRAINT PK_Reservaciones PRIMARY KEY,
    ClienteId NVARCHAR(50) NOT NULL,
    HabitacionNumero INT NOT NULL,
    FechaReservacion DATE NOT NULL CONSTRAINT DF_Reservaciones_Fecha DEFAULT CAST(GETDATE() AS DATE),
    FechaIngreso DATE NOT NULL,
    FechaSalida DATE NOT NULL,
    TarifaReservacion DECIMAL(12,2) NOT NULL,
    SolicitudesEspeciales NVARCHAR(300) NULL,
    PorcentajeDescuento DECIMAL(5,2) NOT NULL CONSTRAINT CK_Reservaciones_Descuento CHECK (PorcentajeDescuento BETWEEN 0 AND 100),
    MontoTotal DECIMAL(12,2) NOT NULL,
    CantidadPersonas INT NOT NULL CONSTRAINT CK_Reservaciones_Personas CHECK (CantidadPersonas BETWEEN 1 AND 10),
    Estado NVARCHAR(20) NOT NULL CONSTRAINT CK_Reservaciones_Estado CHECK (Estado IN (N'Reservada', N'En Proceso', N'Cancelada', N'Finalizada')),
    CONSTRAINT CK_Reservaciones_Fechas CHECK (FechaSalida > FechaIngreso),
    CONSTRAINT FK_Reservaciones_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Identificacion),
    CONSTRAINT FK_Reservaciones_Habitaciones FOREIGN KEY (HabitacionNumero) REFERENCES Habitaciones(Numero)
);
GO
CREATE INDEX IX_Reservaciones_FechaIngreso ON Reservaciones(FechaIngreso);
GO
INSERT INTO Clientes VALUES (N'1-1234-5678', N'Cedula', N'Ana', N'Pérez', N'Mora', '1995-05-10');
-- Recepcionista pertenece al catálogo de categorías definido por el Proyecto 1.
INSERT INTO Empleados VALUES (N'2-2234-4228', N'Cedula', N'Luis', N'Ramírez', '1990-02-15', 850000, '2022-01-10', N'Recepcionista', N'San José', N'Centro');
INSERT INTO Habitaciones (Numero, Tipo, Tarifa, TieneTV, Mantenimiento) VALUES (101, N'Start Junior', 90, 1, N''), (201, N'Master Start', 150, 1, N'');
/* Reservación de prueba para el reporte. @ProximoLunes siempre pertenece a la semana entrante. */
DECLARE @ProximoLunes DATE = DATEADD(DAY, (DATEDIFF(DAY, 0, CAST(GETDATE() AS DATE)) / 7 + 1) * 7, 0);
INSERT INTO Reservaciones (CodigoReservacion, ClienteId, HabitacionNumero, FechaReservacion, FechaIngreso, FechaSalida, TarifaReservacion, SolicitudesEspeciales, PorcentajeDescuento, MontoTotal, CantidadPersonas, Estado)
VALUES (N'RES001', N'1-1234-5678', 101, CAST(GETDATE() AS DATE), @ProximoLunes, DATEADD(DAY, 2, @ProximoLunes), 180.00, N'Reservación de prueba', 0, 203.40, 2, N'Reservada');
GO
