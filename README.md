# CSV Merge
A command-line tool for merging dissimilar CSV files in C# .NET Core:
```
CSV.exe sales.csv new_sales.csv > merged_sales.csv
```

## Features:
- Merge dissimilar CSV files using all header fields
- Merge multiple files
- Specify the culture that you want to use (determines the default delimiter, default line ending and formatting)
- Trim all leading/trailing whitespace characters

## Prerequisites:
- .NET Core 3.1
- Visual Studio 2019

## Credits:
- CsvHelper by https://joshclose.github.io/CsvHelper (MS-PL or Apache-2.0 license)
