# BrewTrack Readme

## Introduction

This is a mock assesment project that provides a single source of information about brew pubs for customers.

## Stack

1. React Front End
2. .Net backend
3. MySQL Database
4. Redis

![Application Stack](./Assets/brew-arch-Stack.jpg)

## Prerequisites

In order to run this project an environment will require the following as prereqs:
 
1. Nodejs v18.16.0 or later
2. .Net sdk 7.0.203

For the app to run locally container apps were used for MySQL and Redis.

## Application Local Dev Infrastructure

To run this app on a local machine these optional steps may be followed to start required application infrastructure as container services.

```bash
cd infra
docker compose -d up
```

## Configuration

To configure the application with its required infrastructure please create a `appsettings.Development.json` file and append these values to it.

```
  "WeatherApiKey": "7f4f8c7c-fe1c-11ed-a26f-0242ac130002-7f4f8ce0-fe1c-11ed-a26f-0242ac130002",
  "ConnectionStrings": {
    "MySql": "Server=localhost;Database=BrewTrack;Uid=BrewTrack;Pwd=<secret password>;Port=3306",
    "Redis": ""
  }
 ```

## Architecture

### ARCHITECTURAL PATTERN

![Arch Pattern](./Assets/brew-arch-Service%20Integration.jpg)