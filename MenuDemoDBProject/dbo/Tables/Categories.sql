CREATE TABLE [dbo].[Categories]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(400) NULL, 
    [MenuId] INT NOT NULL, 
    CONSTRAINT [FK_Categories_ToMenu] FOREIGN KEY ([MenuId]) REFERENCES [Menus]([Id])
)
