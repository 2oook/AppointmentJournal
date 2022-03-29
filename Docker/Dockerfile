FROM mcr.microsoft.com/dotnet/sdk:latest

# Nginx
RUN apt update && \
    apt install -y --no-install-recommends nginx && \
    rm -rf /var/lib/apt/lists/* && \
    apt clean && \
    rm /etc/nginx/nginx.conf

COPY ./nginx.conf /etc/nginx/nginx.conf

WORKDIR /var/www/AppointmentJournal

COPY ./publish .

ENV ASPNETCORE_URLS http://+:5000

ENTRYPOINT ["dotnet", "AppointmentJournal.dll"]