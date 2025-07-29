CREATE TABLE [User]
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Active BIT NOT NULL,
    IsDeleted BIT NOT NULL,
    PersonId INT UNIQUE NOT NULL
);

CREATE TABLE Person
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Address NVARCHAR(50) NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE RolUser
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    UserId INT NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE Rol
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(1000) NOT NULL,
    IsDeleted BIT NOT NULL,
    CreateAt DATETIME2 NOT NULL,
    DeleteAt DATETIME2 NULL
);

CREATE TABLE RolFormPermission
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    FormId INT NOT NULL,
    PermissionId INT NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE Permission
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE Form
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE FormModule
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ModuleId INT NOT NULL,
    FormId INT NOT NULL,
    IsDeleted BIT NOT NULL
);

CREATE TABLE Module
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    IsDeleted BIT NOT NULL
);

-- Relaciones
-- User - Person
ALTER TABLE [User] ADD CONSTRAINT FK_User_Person FOREIGN KEY (PersonId) REFERENCES Person(Id);
-- RolUser - User
ALTER TABLE RolUser ADD CONSTRAINT FK_RolUser_User FOREIGN KEY (UserId) REFERENCES [User](Id);
-- RolUser - Rol
ALTER TABLE RolUser ADD CONSTRAINT FK_RolUser_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id);
-- Module - Form
ALTER TABLE FormModule ADD CONSTRAINT FK_FormModule_Module FOREIGN KEY (ModuleId) REFERENCES Module(Id);
ALTER TABLE FormModule ADD CONSTRAINT FK_FormModule_Form FOREIGN KEY (FormId) REFERENCES Form(Id);
-- RolFormPermission - Rol - Form - Permission
ALTER TABLE RolFormPermission ADD CONSTRAINT FK_RolFormPermission_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id);
ALTER TABLE RolFormPermission ADD CONSTRAINT FK_RolFormPermission_Form FOREIGN KEY (FormId) REFERENCES Form(Id);
ALTER TABLE RolFormPermission ADD CONSTRAINT FK_RolFormPermission_Permission FOREIGN KEY (PermissionId) REFERENCES Permission(Id);