#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:14.11.0-alpine3.10 AS client 
ARG skip_client_build=false 
WORKDIR /app 
COPY ComputersApp.Api/ClientApp . 
RUN [[ ${skip_client_build} = true ]] && echo "Skipping npm install" || npm install 
RUN [[ ${skip_client_build} = true ]] && mkdir -p dist || npm run build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ComputersApp.Api/ComputersApp.Api.csproj", "ComputersApp.Api/"]
RUN dotnet restore "ComputersApp.Api/ComputersApp.Api.csproj"
COPY . .
WORKDIR "/src/ComputersApp.Api"
RUN dotnet build "ComputersApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComputersApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
ENTRYPOINT ["dotnet", "ComputersApp.Api.dll"]