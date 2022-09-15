# CSV Join SQL
A command-line tool for performing full outer joins on CSV files in C# .NET Core using SQL:
```
CsvJoin.CsvJoin.SqlGenerator.exe Data sales.csv new_sales.csv > SqlQuery.sql
CsvJoin.exe SqlQuery.sql Data > joined_sales.csv
```

## Features:
- Execute SQL against CSV files
- Save results to CSV
- Save auto-generated SQL

## Build and run:
- Build CsvJoin and CsvJoin.SqlGenerator projects
- Generate sql script:
```
CsvJoin.CsvJoin.SqlGenerator.exe Data sales.csv new_sales.csv > SqlQuery.sql
```
- Verify that the generated script does not contain SQL injection
- Execute the script:
```
CsvJoin.exe SqlQuery.sql Data > joined_sales.csv
```

## Prerequisites:
- Microsoft Access Database Engine 2016 Redistributable
- .NET Core 3.1
- Visual Studio 2019

## Acknowledgements:
- Microsoft.Extensions.DependencyInjection by https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection (MIT license)
- System.Data.OleDb by https://www.nuget.org/packages/System.Data.OleDb (MIT license)
- ServiceStack.Text by https://www.nuget.org/packages/ServiceStack.Text ([license](https://github.com/ServiceStack/ServiceStack.Text/blob/master/license.txt))
