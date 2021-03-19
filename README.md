# CSV Join
A command-line tool for performing full outer joins on CSV files in C# .NET Core:
```
CsvJoin.exe Data sales.csv new_sales.csv > joined_sales.csv
```

## Features:
- Execute SQL against CSV files
- Save results to CSV
- Save auto-generated SQL

## Prerequisites:
- Microsoft Access Database Engine 2016 Redistributable
- .NET Core 3.1
- Visual Studio 2019

## Credits:
- Microsoft.Extensions.DependencyInjection by https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection (MIT license)
- System.Data.OleDb by https://www.nuget.org/packages/System.Data.OleDb (MIT license)
- CsvHelper by https://www.nuget.org/packages/CsvHelper (MS-PL or Apache-2.0 license)
