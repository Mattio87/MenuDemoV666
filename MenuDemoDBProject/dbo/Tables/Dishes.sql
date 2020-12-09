CREATE TABLE [dbo].[Dishes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(400) NULL , 
    [Price] FLOAT NULL DEFAULT 0, 
    [Sauce] VARCHAR(200) NULL, 
    [Patty] VARCHAR(100) NULL, 
    [Cheese] VARCHAR(100) NULL, 
    [Bun] VARCHAR(100) NULL, 
    [Toppings] VARCHAR(300) NULL, 
    [Steaktype] VARCHAR(300) NULL, 
    [Side] VARCHAR(100) NULL, 
    [Amount] INT NULL, 
    [Pastatype] VARCHAR(300) NULL, 
    [Meat] VARCHAR(300) NULL, 
    [Dish] NCHAR(10) NULL, 
    [Dishtype] INT NULL, 
    [isAlcoholic] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_Dishes_ToDishTypes] FOREIGN KEY ([Dishtype]) REFERENCES [DishTypes]([Id])
)
