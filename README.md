# Convert2OFX

A .NET application to convert Boursorama bank CSV exports to QIF/OFX formats compatible with Microsoft Money and other financial management software.

## Overview

Convert2OFX processes CSV transaction files from Boursorama bank and converts them to QIF (Quicken Interchange Format) format, making it easy to import your banking transactions into financial management software like Microsoft Money 2005.

## Features

- **Console Application**: Command-line tool for batch processing CSV files
- **WPF Application**: Windows desktop application with graphical interface
- **Automatic Processing**: Processes all CSV files in the `Temp` folder
- **Logging**: Built-in logging system using log4net
- **Transaction Support**: Handles transaction dates, amounts, labels, categories, and account information

## Getting Started

### Prerequisites

- .NET Framework 4.5.2 or higher
- Windows operating system

### Dependencies

- **log4net 2.0.8**: Logging framework
- **LumenWorks.Framework.IO (CsvReader) 3.9**: CSV parsing library

### Installation

1. Clone the repository:
```bash
git clone https://github.com/kristofias/convert2ofx.git
```

2. Open `Convert2OfxConsole.sln` in Visual Studio

3. Restore NuGet packages

4. Build the solution

### Usage

#### Console Application

1. Place your Boursorama CSV export files in the `Temp` folder (created automatically in the application directory)
2. Run `Convert2OfxConsole.exe`
3. The application will:
   - Process all CSV files in the `Temp` folder
   - Generate QIF output files with timestamped filenames
   - Rename processed CSV files with a "traité" (processed) prefix
   - Create logs in the `Logs` folder

#### WPF Application

1. Launch `Convert2OfxWpfApplication.exe`
2. Use the graphical interface to select and convert files

### Output Format

The application generates QIF files with the following format:
- Type: Bank
- Includes transaction dates, amounts, and labels
- Card payments are formatted with date extraction
- UTF-8 encoding without BOM

### Project Structure

- **Convert2OfxConsole**: Console application for command-line processing
- **Export2OfxWpfApplication**: WPF desktop application with GUI
- **packages**: NuGet package dependencies

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## Authors

* **Christophe Barré** - *Initial work* - [kristofias](https://github.com/kristofias)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- OFX format specification: http://www.ofx.net/
- log4net documentation and best practices
