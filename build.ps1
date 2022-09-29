$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish CollectionOSDBtoCSV $options --runtime win-x64 --framework net6.0 -o build/win-x64
dotnet publish CollectionOSDBtoCSV $options --runtime linux-x64 --framework net6.0 -o build/linux-x64