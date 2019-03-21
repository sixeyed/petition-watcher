# escape=`
FROM microsoft/dotnet:2.2-sdk-nanoserver-1809 as builder

WORKDIR C:\src
COPY src\PetitionWatcher.Core\PetitionWatcher.Core.csproj .\PetitionWatcher.Core\PetitionWatcher.Core.csproj
COPY src\PetitionWatcher.Messaging\PetitionWatcher.Messaging.csproj .\PetitionWatcher.Messaging\PetitionWatcher.Messaging.csproj
COPY src\PetitionWatcher.SchedulerApi\PetitionWatcher.SchedulerApi.csproj .\PetitionWatcher.SchedulerApi\PetitionWatcher.SchedulerApi.csproj

WORKDIR C:\src\PetitionWatcher.SchedulerApi
RUN dotnet restore

COPY src C:\src
RUN dotnet publish -c Release -o C:\out PetitionWatcher.SchedulerApi.csproj

# app image
FROM  microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1809

EXPOSE 80
WORKDIR /scheduler-api
ENTRYPOINT ["dotnet", "PetitionWatcher.SchedulerApi.dll"]

COPY --from=builder C:\out\ .