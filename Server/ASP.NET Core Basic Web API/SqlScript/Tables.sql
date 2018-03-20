CREATE TABLE [User]
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