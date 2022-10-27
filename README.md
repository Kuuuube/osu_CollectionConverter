# osu! Collection Converter

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

### Input Format

The format of the input file.

`1`: DB (osu! collection format)

`2`: OSDB (Collection Manager format)

`3`: CSV (CSV in Collection Converter format)

### Output Format

The format of the output file.

`1`: DB (osu! collection format)

`2`: OSDB (Collection Manager format)

`3`: CSV (CSV in Collection Converter format)

### Number of Header Rows

The amount of header rows for CSV files. This value is unused when input or output is not CSV.

`0`: No header row

`1`: One header row

## Dependencies

.Net Runtime 6.0 x64: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Building:

Run `build.ps1` or manually run:

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish CollectionConverter $options --runtime win-x64 --framework net6.0 -o build/win-x64
dotnet publish CollectionConverter $options --runtime linux-x64 --framework net6.0 -o build/linux-x64
```