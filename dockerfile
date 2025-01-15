# Use .NET SDK image for build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining application files
COPY . ./

# Build the application in Release mode
RUN dotnet publish -c Release -o /app/out

# Use .NET Runtime image for runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Install dependencies and Chrome
RUN apt-get update && apt-get install -y \
    wget \
    unzip \
    xvfb \
    libxi6 \
    libgconf-2-4 \
    libnss3 \
    libxss1 \
    libappindicator1 \
    libindicator7 \
    fonts-liberation \
    libasound2 \
    libgbm-dev \
    libgtk-3-0 && \
    wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb && \
    dpkg -i google-chrome-stable_current_amd64.deb || apt-get -fy install

# Install ChromeDriver
RUN wget -O /tmp/chromedriver.zip https://chromedriver.storage.googleapis.com/110.0.5481.3000/chromedriver_linux64.zip && \
    unzip /tmp/chromedriver.zip -d /usr/local/bin/ && \
    chmod +x /usr/local/bin/chromedriver

# Copy the output from the build stage
COPY --from=build /app/out .

# Expose the application on port 5004
EXPOSE 5004

# Set the entry point for the application
ENTRYPOINT ["dotnet", "QC_FetchAPI.dll"]
