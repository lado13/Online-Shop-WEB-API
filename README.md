# Online Shop Web API

Welcome to the Online Shop Web API! This API serves as the backend for an online shopping platform, providing endpoints for managing products, orders, and user authentication.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)
- [Contact](#contact)

## Introduction

The Online Shop Web API is designed to facilitate the development of an e-commerce platform by providing a robust backend infrastructure. Built with .NET Core 8 using C#, it offers a range of functionalities crucial for managing products, processing orders, and authenticating users.

## Features

- **Product Management**: CRUD operations for managing products, including creation, retrieval, updating, and deletion.
- **Order Processing**: Endpoints to create, retrieve, update, and cancel orders.
- **User Authentication**: Secure authentication and authorization mechanisms using JSON Web Tokens (JWT).
- **Flexible Configuration**: Easily configurable settings for database connections, authentication options, and more.
- **Scalable Architecture**: Built on the .NET Core framework, ensuring scalability and performance.

## Installation

1. **Clone the Repository**: 

2. **Navigate to the Project Directory**:

3. **Install Dependencies**:

4. **Database Configuration**:
- Update the database connection string in `appsettings.json` to point to your SQL Server instance.
- To update the connection string in your .NET Core project's appsettings.json file and then apply the migration using the Entity Framework Core's update-database command in the Package Manager Console, you can follow these steps:

- Open the appsettings.json file in your project and locate the section where the connection string is defined

- Replace YOUR_SERVER_NAME with your SQL Server instance name.

Apply Migration:
After updating the connection string, open the Package Manager Console in Visual Studio. Then run the following command to apply the migration to update the database schema:

>>>>: update-database


## Usage

1. **Run the Application**:
