# CSV Join SQL
A command-line tool for performing full outer joins on CSV files in C# .NET Core using SQL:
```
CsvJoin.SqlGenerator.exe Data sales.csv new_sales.csv > SQLQuery.sql
```
```
CsvJoin.exe SQLQuery.sql Data > joined_sales.csv
```

## Features:
- Execute SQL against CSV files
- Save auto-generated SQL
- Save results to CSV

## Build and run:
### VS:
- src\CsvJoin.sln > Build > Build Solution
### Cmd:
- `CsvJoin.SqlGenerator.exe Data sales.csv new_sales.csv > SQLQuery.sql`
- Verify that the generated file (SQLQuery.sql) does not contain SQL injection
- `CsvJoin.exe SqlQuery.sql Data > joined_sales.csv`

## Prerequisites:
- Microsoft Access Database Engine 2016 Redistributable
- .NET Core 3.1
- Visual Studio 2022

## Acknowledgements:
- Microsoft.Extensions.DependencyInjection by https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection (MIT license)
- System.Data.OleDb by https://www.nuget.org/packages/System.Data.OleDb (MIT license)
- ServiceStack.Text by https://www.nuget.org/packages/ServiceStack.Text ([license](https://github.com/ServiceStack/ServiceStack.Text/blob/master/license.txt))
