# RabbitMQ - Messages and Queues | Coding Live #006

## Getting Started

These instructions is a part of a live coding video.

### Prerequisites

-   .NET Core 3.1 SDK - https://dotnet.microsoft.com/download
-   RabbitMQ Server - https://hub.docker.com/_/rabbitmq

## Example project

Create a base folder `CodingLive006`.

Create the .gitignore file based on file https://github.com/github/gitignore/blob/master/VisualStudio.gitignore

### Creating projects

```bash
dotnet new console --name Ex1Consumer
dotnet new console --name Ex1Publisher

dotnet new console --name Ex2Consumer
dotnet new console --name Ex2Publisher

dotnet new console --name Ex3Consumer
dotnet new console --name Ex3Publisher

dotnet new console --name Ex4Consumer
dotnet new console --name Ex4Publisher
```

### Add package for RabbitMQ client

```bash
dotnet add package RabbitMQ.Client
```

## References

https://www.rabbitmq.com/getstarted.html
https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
