#!/usr/bin/env bash
# Use this script to start nginx

STARTNGINX_cmdname=${0##*/}

echo "$STARTNGINX_cmdname: starting server..."

echo  $(service --status-all) 

echo "Starting nginx."
nginx

echo  $(service --status-all) 

export ASPNETCORE_ENVIRONMENT=Docker

echo "Starting AppointmentJournal"
dotnet AppointmentJournal.dll