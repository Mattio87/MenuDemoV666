CREATE TABLE [dbo].[CategoriesInMenus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryId] INT NOT NULL, 
    [MenuId] INT NOT NULL
)
