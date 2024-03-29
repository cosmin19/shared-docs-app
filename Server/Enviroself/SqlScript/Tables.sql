﻿CREATE TABLE [User]
(
	[Id] int not null IDENTITY(1,1) PRIMARY KEY,
	[Email] nvarchar(300) not null,
	[Password] nvarchar(MAX) not null,
	[UserName] nvarchar(200) not null,
	[FirstName] nvarchar(200),
	[LastName] nvarchar(200),
	[Role] nvarchar(100) not null,
	[IsActivated] bit not null DEFAULT 0,
	[Valid] bit not null DEFAULT 1,
	[CreatedOnUtc] datetime not null,
	[UpdatedOnUtc] datetime
)

CREATE TABLE [Document]
(
	[Id] int not null PRIMARY KEY IDENTITY(1,1),
	[OwnerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[Title] nvarchar(150) not null,
	[Subject] nvarchar(MAX) not null,
	[CreatedOnUtc] datetime not null,
	[UpdatedOnUtc] datetime
)

CREATE TABLE [Invitation]
(
	[Id] int not null PRIMARY KEY IDENTITY(1,1),
	[FromUserId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[ToUserId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[Status] int not null,
	[ActionType] int not null,
	[Message] nvarchar(200),
	[DocumentId] int not null FOREIGN KEY REFERENCES dbo.[Document](Id),
)

CREATE TABLE [User_Document_Edit]
(
	[Id] int not null PRIMARY KEY IDENTITY(1,1),
	[OwnerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[DocumentId] int not null FOREIGN KEY REFERENCES dbo.[Document](Id),
	[EditorId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
)

CREATE TABLE [User_Document_View]
(
	[Id] int not null PRIMARY KEY IDENTITY(1,1),
	[OwnerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[DocumentId] int not null FOREIGN KEY REFERENCES dbo.[Document](Id),
	[ViewerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
)
