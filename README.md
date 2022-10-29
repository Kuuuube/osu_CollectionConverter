# osu! Collection Converter

Converts between different osu! collection formats.

## Downloads

Downloads are available in [Releases](https://github.com/Kuuuube/osu_CollectionConverter/releases).

## Usage

Either run with or without command line args.

## Command line args:

```
CollectionConverter {Input} {Output} {Input Format} {Output Format} {osu!.db Path} {Number of Header Rows}
```

### Input

The relative or absolute file path for the input file or folder.

### Output

The relative or absolute file path for the output file or folder.

### Input Format

The format of the input file or folder.

`1`: DB (osu! collection format)

`2`: OSDB (Collection Manager format)

`3`: CSV (CSV in Collection Converter format)

`4`: Folder (All collections in the folder will be parsed based on extension)

### Output Format

The format of the output file or folder.

`1`: DB (osu! collection format)

`2`: OSDB (Collection Manager format)

`3`: CSV (CSV in Collection Converter format)

`41`: Folder using DB (All collections individually exported in DB format)

`42`: Folder using OSDB (All collections individually exported in OSDB format)

`43`: Folder using CSV (All collections individually exported in CSV format)

### osu!.db Path

The relative or absolute file path for an osu!.db file to get map info from.

`{relative or absolute path}`: Loads the osu!.db from the path given.

`0`: Skips loading the osu!.db.

### Number of Header Rows

The amount of header rows for CSV files. This value is unused when input or output is not CSV.

`0`: No header row

`1`: One header row

## Collection Converter CSV Format

Filling all cells is not required. Blank cells will result in the collection missing data for the blank cell only.

Raw Format:

```
"CollectionName","MapId","MapSetId","Md5","PlayMode","ArtistRoman","ArtistUnicode","TitleRoman","TitleUnicode","DiffName","StarsNomod"
```

Format in table:

| CollectionName | MapId	| MapSetId	| Md5	| PlayMode	| ArtistRoman	| ArtistUnicode	| TitleRoman	| TitleUnicode	| DiffName	| StarsNomod |
| :------------- | :------- | :-------- | ----: |---------: |-------------: |-------------: |-------------: |-------------: |---------: |----------: |
|                |          |           |       |           |               |               |               |               |           |            |

## Dependencies

.Net Runtime 6.0 x64: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Building:

Run `build.ps1` or manually run:

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish CollectionConverter $options --runtime win-x64 --framework net6.0 -o build/win-x64
dotnet publish CollectionConverter $options --runtime linux-x64 --framework net6.0 -o build/linux-x64
```