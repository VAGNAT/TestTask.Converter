#ProjectOverview

This repository contains an application with the following architecture:

## 1. Application Architecture

- Microservice architecture.
- Postgres database.
- RabbitMQ message broker.
- Logger using Serilog.
- The `api.gateway` service is implemented using Ocelot, and CORS is configured.
- The `FileData` service is built on a clean architecture using CQRS and Repository patterns. It is designed to store files.
- The `FileService` service processes files, accepting them for processing from the message broker. It interacts with a broker created through MassTransit.
- The client is built using the React framework.

## 2. Running the Application

To run the application, ensure that Postgres and RabbitMQ are running on their standard ports.

## 3. Changes in the future:

- Added a database to the `FileService` service, storing the ID and processing time of files.
- Modified the status field in the main table of the `FileService` service to support statuses such as Error, Success, and Processing. These statuses are returned to the client accordingly.
