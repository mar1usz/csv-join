# CSV Join SQL
A command-line tool for performing full outer joins on CSV files in C# .NET Core using SQL:
```
CsvJoin.exe Data sales.csv new_sales.csv > joined_sales.csv
```

## Features:
- Execute Access SQL against CSV files
- Save results to CSV
- Save auto-generated SQL

## To implement:
- Protect against SQL injection

## Prerequisites:
- Microsoft Access Database Engine 2016 Redistributable
- .NET Core 3.1
- Visual Studio 2019

## Acknowledgements:
- Microsoft.Extensions.DependencyInjection by https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection (MIT license)
- System.Data.OleDb by https://www.nuget.org/packages/System.Data.OleDb (MIT license)
- ServiceStack.Text by https://www.nuget.org/packages/ServiceStack.Text ([license](https://github.com/ServiceStack/ServiceStack.Text/blob/master/license.txt))
