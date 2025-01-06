# Use .NET SDK image for build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining application files
COPY . ./

# Build the application in Release mode
RUN dotnet publish -c Release -o /app/out

# Use .NET Runtime image for runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the output from the build stage
COPY --from=build /app/out .

# Expose the application on port 5004
EXPOSE 5004

# Set the entry point for the application
ENTRYPOINT ["dotnet", "QC_FetchAPI.dll"]
