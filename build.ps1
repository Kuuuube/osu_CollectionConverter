$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish CollectionConverter $options --runtime win-x64 --framework net8.0 -o build/win-x64
dotnet publish CollectionConverter $options --runtime linux-x64 --framework net8.0 -o build/linux-x64