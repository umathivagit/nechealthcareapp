IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'portal' )
    EXEC('CREATE SCHEMA [portal]');
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'logging' )
    EXEC('CREATE SCHEMA [logging]');
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'reporting' )
    EXEC('CREATE SCHEMA [reporting]');
GO
IF OBJECT_ID(N'[portal].[Doctor]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Doctor](
        [DoctorID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[IMA_No] int Unique NOT NULL,
		[FullName] [nvarchar](100) NOT NULL,
		DOB date NOT NULL,
		Gender [nvarchar](100) NOT NULL,
		Email [nvarchar](100) NOT NULL,
		YearsOfExperience int NOT NULL,
		Created_Date datetime NOT NULL,
		Modified_Date datetime NOT NULL
    );
END
GO
IF OBJECT_ID(N'[portal].[Services]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Services](
        [Service_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Service_Name] [nvarchar](100) Unique NOT NULL,
		[Description] [nvarchar](100) NOT NULL
    );
END
GO
IF OBJECT_ID(N'[portal].[Doctor_Services]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Doctor_Services](
        [Doc_Service_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Service_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Services](Service_ID),
		[Doctor_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Doctor](DoctorID)
    );
END
GO
IF OBJECT_ID(N'[portal].[Qualification]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Qualification](
        [Qualification_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Qualification_Name] [nvarchar](100) NOT NULL
    );
END
GO
IF OBJECT_ID(N'[portal].[Doctor_Qualification]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Doctor_Qualification](
        [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Qualification_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Qualification](Qualification_ID),
		[Doctor_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Doctor](DoctorID)
    );
END
GO
IF OBJECT_ID(N'[portal].[Patient]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Patient](
        [PatientID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[FullName] [nvarchar](100) NOT NULL,
		[Gender] [nvarchar](100) NOT NULL,
		[BloodGroup] [nvarchar](100) NOT NULL,
		[Weight] float NOT NULL,
		[Height] float NOT NULL,
		[DOB] date NOT NULL,
		[Email] [nvarchar](100) NOT NULL,
		[ContactNumber] [nvarchar](100) NOT NULL,
		[Address] [nvarchar](100) NOT NULL,
		[HealthInsuranceID] [nvarchar](100) NOT NULL,
		[EmergencyContactPersonDetails] [nvarchar](100)  NOT NULL,
		[Created_At] datetime NOT NULL,
		[Modified_At] datetime
    );
END
GO

IF OBJECT_ID(N'[portal].[Appointment]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Appointment](
        [Appointment_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Patient_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Patient](PatientID),
		[Service_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Services](Service_ID),
		[Doctor_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Doctor](DoctorID),
		[Time] time NOT NULL,
		[Date] date NOT NULL,
		[Symptoms] [nvarchar](255) NOT NULL,
		[Appointment_Status] [nvarchar](255) NOT NULL,
		[Created_At] datetime NOT NULL,
		[Modified_At] datetime,
    );
END
GO


IF OBJECT_ID(N'[portal].[Role]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Role](
        [RoleID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[RoleName] [nvarchar](255) NOT NULL,
		[Active] tinyint NOT NULL
    );
END
GO

IF OBJECT_ID(N'[portal].[User]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[User](
        [User_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Password] [nvarchar](255) NOT NULL,
		[Role_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Role](RoleID),
		[Active] tinyint NOT NULL,
    );
END
GO

IF OBJECT_ID(N'[portal].[Permission]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Permission](
        [Permission_ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Permission_Name] [nvarchar] (255) NOT NULL
    );
END
GO

IF OBJECT_ID(N'[portal].[PatientDoctorDetails]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[PatientDoctorDetails](
	    [Patient_Doctor_ID] Int IDENTITY(1,1) PRIMARY KEY,
        [Patient_ID] INT NOT NULL FOREIGN KEY REFERENCES [portal].[Patient](PatientID),
		[Doctor_ID] int NOT NULL FOREIGN KEY REFERENCES [portal].[Doctor](DoctorID)
    );
END
GO

IF OBJECT_ID(N'[portal].[ModeOfPayment]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[ModeOfPayment](
        [PaymentModeID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[PaymentModeName] [nvarchar] NOT NULL ,
    );
END
GO

IF OBJECT_ID(N'[portal].[Payments]', N'U') IS NULL
BEGIN
    CREATE TABLE [portal].[Payments](
        [PaymentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Patient_ID] INT NOT NULL FOREIGN KEY REFERENCES [portal].[Patient](PatientID),
		[ModeOfPayment] int NOT NULL FOREIGN KEY REFERENCES [portal].[ModeOfPayment](PaymentModeID),
		[Amount] float NOT NULL,
		[TransactionID] [nvarchar](255) NOT NULL,
		[Created] datetime NOT NULL,
    );
END
GO


IF OBJECT_ID(N'[reporting].[GetDoctorListByServiceID]', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [reporting].[GetDoctorListByServiceID]
END
GO

create proc [reporting].[GetDoctorListByServiceID]
(@Service_ID int)
as 
Begin
Set NoCount On
select d.* from portal.Doctor as d
inner join portal.Doctor_Services as ds on d.DoctorID = ds.Doctor_ID
inner join portal.Services as s on s.Service_ID = ds.Service_ID where s.Service_ID =@Service_ID
END
GO

IF OBJECT_ID(N'[reporting].[GetServiceListByDoctorID]', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE [reporting].[GetServiceListByDoctorID]
END
GO

create proc [reporting].[GetServiceListByDoctorID]
(@Doctor_ID int)
as 
Begin
Set NoCount On
select s.* from portal.Services as s
inner join portal.Doctor_Services as ds on s.Service_ID = ds.Service_ID
inner join portal.Doctor as d on d.DoctorID = ds.Doctor_ID where d.DoctorID =@Doctor_ID
END
GO