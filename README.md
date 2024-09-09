GameCritic: Game Review Management System

GameCritic is a web-based application designed for managing video game reviews. It provides a platform for both users and administrators to manage and review games. The application is built using ASP.NET Core with a multi-tier architecture, featuring a RESTful API and an MVC front-end for user interaction.

Table of Contents

Project Overview
Features
Installation and Setup
Usage
Technologies Used
Project Structure
Contributing
License

Project Overview

GameCritic offers users the ability to review video games, with full CRUD (Create, Read, Update, Delete) functionality for managing games, genres, and reviews. The application consists of two main components:

RESTful API: Handles backend operations and provides endpoints for managing entities such as games, genres, and reviews.

MVC Frontend: Provides an intuitive user interface with role-based access for both administrators and regular users.

The system follows modern software development practices, utilizing multi-tier architecture, JWT-based authentication, and robust validation techniques to ensure data integrity and security.

Features

RESTful API Module

CRUD Operations: Manage games, genres, and reviews via fully functional RESTful endpoints.
JWT Authentication: Secure API endpoints with JWT tokens for user and admin roles.
Paging & Searching: Efficiently handle large datasets with support for pagination and searching.
Logging: API logs all CRUD operations, providing endpoints to retrieve logs and monitor system behavior.

MVC Web Application

User Registration & Authentication: Role-based access for admins and users with secure login functionality.
Game Management: Admins can add, edit, and delete games and genres, while users can browse and review games.
Profile Management: Users can manage their profiles, update personal details, and view their review history.
Asynchronous Operations: Ajax is used for smooth, dynamic interactions, such as updating user details and paginated lists.
Responsive Design: The web interface is fully responsive and accessible from desktop and mobile devices.
Advanced Search & Filters: Search games by title or genre with easy-to-use filters.

Installation and Setup

Prerequisites:

.NET 6 SDK installed
SQL Server installed and running
Visual Studio 2022 or another compatible IDE
Maven for managing dependencies (if applicable)

Steps to Run the Project:

Clone the Repository:

bash
git clone https://github.com/yourusername/GameCritic.git
cd GameCritic

Setup the Database:

Navigate to the Database folder and execute the provided SQL script (Database.sql) to create the necessary tables and seed the initial data.

Configure Database Connection:

Open the appsettings.json file in both the Web API and MVC projects, and update the connection string to point to your SQL Server instance.

Build the Solution:

Open the solution in Visual Studio and build the project.
Ensure that both the Web API and MVC projects are set to start simultaneously.

Run the Application:

Start both the Web API and MVC projects by running the solution. The Web API will be accessible on port 5001 and the MVC app on port 5000 by default.

Access the Application:

Open a browser and navigate to http://localhost:5000 to access the GameCritic application.

Usage

Admin Features:
Manage Games: Admins can add, edit, and delete games from the system. Genres can be added and associated with games.
User Management: Admins can view the list of registered users and manage their access.
Review Monitoring: Admins can monitor game reviews, ensuring content quality.

User Features:
Create Account: Users can register, log in, and manage their accounts.
Review Games: Users can browse games, read reviews, and add their own reviews.
Profile Management: Users can update their profile information and manage their personal review history.

API Endpoints:
Game CRUD: POST /api/games, GET /api/games/{id}, PUT /api/games/{id}, DELETE /api/games/{id}
Review CRUD: POST /api/reviews, GET /api/reviews/{id}, PUT /api/reviews/{id}, DELETE /api/reviews/{id}
JWT Authentication: POST /api/auth/login, POST /api/auth/register
Logs: GET /api/logs

Technologies Used

ASP.NET Core: The main framework for building the Web API and MVC projects.
SQL Server: Database for storing games, genres, reviews, and user data.
Entity Framework Core: For database operations and CRUD functionalities.
AutoMapper: For mapping models across the application's tiers.
JWT (JSON Web Token): For secure authentication of users and administrators.
Ajax: For handling asynchronous operations on the client side.
Bootstrap 5: For building responsive and modern front-end UI.

Project Structure

GameCritic/
│
├── Database/                # SQL script for database creation
├── GameCritic.sln           # Visual Studio Solution file
├── WebAPI/                  # ASP.NET Core Web API project
│   └── Controllers/         # API Controllers for managing entities
├── WebApp/                  # ASP.NET Core MVC project
│   ├── Views/               # Razor Views for the web interface
│   ├── Controllers/         # Controllers for handling web requests
│   └── wwwroot/             # Static files (CSS, JS, images)
└── README.md                # Project documentation (this file)

Contributing
Contributions are welcome! Please open an issue or submit a pull request with your proposed changes. Make sure to follow the contribution guidelines.
