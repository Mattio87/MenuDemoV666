CREATE TABLE [dbo].[Menus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(400) NULL, 
    [RestaurantID] INT NULL, 
    CONSTRAINT [FK_Menus_ToRestaurants] FOREIGN KEY ([RestaurantID]) REFERENCES [Restaurants]([Id])
)
