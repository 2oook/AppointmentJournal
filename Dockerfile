FROM mcr.microsoft.com/dotnet/sdk:latest AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY ["./AppointmentJournal/AppointmentJournal.csproj", "."]
RUN dotnet restore "/src/AppointmentJournal.csproj"
COPY ./AppointmentJournal .
RUN dotnet build "AppointmentJournal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AppointmentJournal.csproj" -c Release -o /app

FROM base AS final

# Nginx
RUN apt update && \
    apt install -y --no-install-recommends nginx && \
    #rm -rf /var/lib/apt/lists/* && \
    #apt clean && \
    #rm /etc/nginx/nginx.conf
    ls

COPY ./DeploymentFiles/nginx.conf /etc/nginx/sites-enabled/appointment-journal.conf

# Copy wait-for-it.sh into our image
COPY ./DeploymentFiles/wait-for-it.sh /wait-for-it.sh
# Make it executable, in Linux
RUN chmod +x /wait-for-it.sh

COPY ./DeploymentFiles/start-server.sh /start-server.sh
RUN chmod +x /start-server.sh

WORKDIR /var/www/AppointmentJournal

COPY --from=publish /app .

ENV ASPNETCORE_URLS http://+:5000

#ENTRYPOINT ["/bin/bash", "-c", "while true; do sleep 1; done"]