# escape=`
FROM microsoft/dotnet:2.2-sdk-nanoserver-1809 as builder

WORKDIR C:\src
COPY src\PetitionWatcher.Core\PetitionWatcher.Core.csproj .\PetitionWatcher.Core\PetitionWatcher.Core.csproj
COPY src\PetitionWatcher.Messaging\PetitionWatcher.Messaging.csproj .\PetitionWatcher.Messaging\PetitionWatcher.Messaging.csproj
COPY src\PetitionWatcher.IndexHandler\PetitionWatcher.IndexHandler.csproj .\PetitionWatcher.IndexHandler\PetitionWatcher.IndexHandler.csproj

WORKDIR C:\src\PetitionWatcher.IndexHandler
RUN dotnet restore

COPY src C:\src
RUN dotnet publish -c Release -o C:\out PetitionWatcher.IndexHandler.csproj

# app image
FROM microsoft/dotnet:2.2-runtime-nanoserver-1809

WORKDIR /index-handler
ENTRYPOINT ["dotnet", "PetitionWatcher.IndexHandler.dll"]

COPY --from=builder C:\out\ .