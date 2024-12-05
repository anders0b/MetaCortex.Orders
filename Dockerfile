FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env
WORKDIR /App

COPY ./MetaCortex.Orders.sln ./

COPY ./MetaCortex.Orders.DataAccess/MetaCortex.Orders.DataAccess.csproj ./MetaCortex.Orders.DataAccess/
COPY ./MetaCortex.Orders.API/MetaCortex.Orders.API.csproj ./MetaCortex.Orders.API/
RUN dotnet restore MetaCortex.Orders.sln

COPY ./ ./

RUN dotnet publish MetaCortex.Orders.API/MetaCortex.Orders.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
WORKDIR /App
COPY --from=build-env /App/out .

# Expose port and define entry point
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MetaCortex.Orders.API.dll"]