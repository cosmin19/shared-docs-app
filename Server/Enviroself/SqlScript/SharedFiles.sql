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
	[Status] int not null /* (0 - Pending, 1 - Accepted, 2 - Refused) */
)

CREATE TABLE [User_Document_Edit]
(
	[OwnerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[DocumentId] int not null FOREIGN KEY REFERENCES dbo.[Document](Id),
	[EditorId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
)

CREATE TABLE [User_Document_View]
(
	[OwnerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
	[DocumentId] int not null FOREIGN KEY REFERENCES dbo.[Document](Id),
	[ViewerId] int not null FOREIGN KEY REFERENCES dbo.[User](Id),
)