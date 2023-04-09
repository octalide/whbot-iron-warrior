# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the .csproj file(s) and restore the required packages
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source files and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image as the runtime environment
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your application will run on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "whbot-iron-warrior.dll"]
