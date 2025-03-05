
E-Commerce API - Scalable & Secure Shopping Platform 🚀🔒
Overview 📝
This repository contains the backend implementation of a Scalable & Secure E-Commerce API designed to facilitate the management of products, categories, users, and carts in a shopping platform. The API is built using ASP.NET Core, Entity Framework Core, and JWT authentication to ensure secure communication and scalability.

Key Features 🌟
JWT Authentication 🔐: Users can authenticate securely using JSON Web Tokens (JWT) for access control and protection.
CRUD Operations 🛠️:
Product Management 🏷️: Create, update, delete, and list products.
Category Management 📂: Manage product categories for better organization.
User Authentication & Authorization 👤: Register new users and authenticate with JWT-based tokens.
Cart Management 🛒: Users can manage their shopping cart with items, including adding, removing, and viewing cart contents.
Scalability 📈: Designed to scale easily with more products, users, and transactions.
Tech Stack 🖥️
ASP.NET Core 🧑‍💻 for the Web API framework.
Entity Framework Core 🔗 for interacting with the database.
JWT 🔑 for secure user authentication and authorization.
SQL Server 🗄️ as the database.
Swagger 📚 (optional) for API documentation.
API Endpoints 📍
Authentication 🔐
POST /api/account/login → User login to get a JWT token.
POST /api/account/register → Register a new user.
Products 📦
GET /api/product → Get a list of all products.
POST /api/product → Add a new product.
Categories 🏷️
GET /api/category → Get a list of all product categories.
Cart 🛍️
GET /api/cart → Get the current user's cart.
POST /api/cart → Add items to the user's cart.
