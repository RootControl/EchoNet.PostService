# Posts Service

The Posts Service is a .NET Core microservice designed to manage post-related functionality for a social media application. It provides CRUD operations for posts, integrates with JWT authentication from the User Service, and uses MongoDB as its data store. This service is built using Clean Architecture, Minimal APIs, and Docker for containerization.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [MongoDB](https://www.mongodb.com/try/download/community) (or Dockerized MongoDB instance)

## Project Structure

- `PostsService.Domain`: Contains entities and interfaces (e.g., `Post`, `IPostRepository`).
- `PostsService.Application`: Houses commands, handlers, DTOs, and validators (e.g., `CreatePostCommand`, `PostDto`).
- `PostsService.Infrastructure`: Implements data access and repositories (e.g., `MongoDbContext`, `PostRepository`).
- `PostsService.Presentation`: Defines the Minimal API endpoints and configuration (e.g., `Program.cs`).

## Features

- Create, read, update, and delete posts.
- Authentication and authorization using JWT (integrated with User Service).
- Health check endpoint for monitoring.
- Swagger UI for API documentation and testing.

## Running Locally

### Prerequisites
Ensure MongoDB is running locally or via Docker.

### Steps
1. **Clone the Repository**:
   ```bash
   git clone <your-repo-url>
   cd PostsService