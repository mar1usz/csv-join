    SELECT [sales].[Customer]
          ,[sales].[Product]
          ,[sales].[Price]
          ,[sales].[Quantity]
          ,[new_sales].[Cost]
      FROM [sales.csv] AS [sales]
 LEFT JOIN [new_sales.csv] AS [new_sales]
        ON [sales].[Customer] = [new_sales].[Customer]
       AND [sales].[Product] = [new_sales].[Product]
       AND [sales].[Quantity] = [new_sales].[Quantity]
     UNION
    SELECT [new_sales].[Customer]
          ,[new_sales].[Product]
          ,[sales].[Price]
          ,[new_sales].[Quantity]
          ,[new_sales].[Cost]
      FROM [sales.csv] AS [sales]
RIGHT JOIN [new_sales.csv] AS [new_sales]
        ON [sales].[Customer] = [new_sales].[Customer]
       AND [sales].[Product] = [new_sales].[Product]
       AND [sales].[Quantity] = [new_sales].[Quantity]
     WHERE [sales].[Customer] IS NULL
       AND [sales].[Product] IS NULL
       AND [sales].[Quantity] IS NULL