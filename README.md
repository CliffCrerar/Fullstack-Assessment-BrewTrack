# BrewTrack Readme

## Introduction

This is a mock assesment project that provides a single source of information about brew pubs for customers.

## Stack

1. React Front End
2. .Net backend
3. MySQL Database
4. Redis

## Prerequisites

In order to run this project an environment will require the following as prereqs:
 
1. Nodejs v18.16.0 or later
2. .Net sdk 7.0.203

For the app to run locally container apps were used for MySQL and Redis.

## Application Local Dev Infrastructure

To run this app on a local machine these optional steps may be followed to start required application infrastructure as container services.

1. MySql

```bash
docker run --name some-mysql \
-e MYSQL_ROOT_PASSWORD=secret-root-password \
-e MYSQL_DATABASE=BrewPub \
-e MYSQL_USER=BrewPub -e MYSQL_PASSWORD=brewpub-secret-password \
-d mysql:tag
```
