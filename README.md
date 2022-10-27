# osu! CollectionConverter

Converts between different osu! collection formats.

## Downloads

Downloads are available in [Releases](https://github.com/Kuuuube/osu_CollectionConverter/releases).

## Usage

Either run with or without command line args.

## Command line args:

```
CollectionConverter {Input} {Output} {Conversion Option} {Number of Header Rows}
```

### Input

The relative or absolute file path for the input file.

### Output

The relative or absolute file path for the output file.

### Conversion Option

The method to use for conversion.

`1`: OSDB to CSV

`2`: CSV to OSDB

### Number of Header Rows

The amount of header rows for CSV files. 

When using CSV input this amount of rows will be ignored. When using CSV output and value above 1 will generate a header and 0 will generate no header.

## Dependencies

.Net Runtime 6.0 x64: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Building:

Run `build.ps1` or manually run:

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish CollectionConverter $options --runtime win-x64 --framework net6.0 -o build/win-x64
dotnet publish CollectionConverter $options --runtime linux-x64 --framework net6.0 -o build/linux-x64
```