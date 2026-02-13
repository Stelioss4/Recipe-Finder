# Recipe Finder 🍽️

## Overview

RecipeFinder is a full-stack web application built with ASP.NET Core and Blazor.

The application aggregates recipes from external sources, stores them in a structured relational database and generates a personalized weekly meal plan for each user.
The focus of the project is not simple content display but real application logic such as data processing, background tasks and user specific functionality.

Live application:
https://www.recipefinderwebapp.com/

---

## Main Features ✨

* Recipe collection and storage from external sources
* Favorite recipes management
* Personalized weekly meal plan generation
* Ingredient handling and shopping list creation
* User authentication and accounts
* Ratings and reviews
* Automatic weekly plan expiration handling
* Email notification when a new weekly plan can be created

---

## Tech Stack 🛠️

**Backend:** ASP.NET Core (.NET, C#)
**Frontend:** Blazor
**Database:** MySQL with Entity Framework Core
**Authentication:** ASP.NET Core Identity
**Version Control:** Git / GitHub
**Hosting:** Remote Linux server

---

## Architecture

The application follows a service-based structure with separation of concerns.

Business logic is implemented in services instead of UI components to keep components lightweight and maintainable.

Core services:

`DataService`
Handles database communication and user related data retrieval.

`ScrapperService`
Collects and parses recipe data from external websites and converts it into structured entities.

`WeeklyPlanService`
Creates, stores and manages weekly meal plans per user.

`FavoriteService`
Manages favorite recipes and user selections.

This structure allows the UI to remain simple while most logic is handled in reusable backend services.

---

## Challenges & Technical Decisions

During development several non-trivial problems had to be solved:

* Preventing duplicate recipes when scraping from different sources
* Designing a consistent ingredient and amount structure
* Deciding between caching and database queries for performance
* Persisting user specific weekly plans over time
* Handling expiration of weekly plans
* Triggering user notifications when a new plan becomes available

The project required repeated refactoring, debugging and data model changes while features were added.

---

## Getting Started 🚀

### Prerequisites

* .NET 8 SDK
* MySQL Server
* Visual Studio (recommended)

### Setup

1. Clone the repository

```
git clone https://github.com/Stelioss4/Recipe-Finder.git
cd Recipe-Finder
```

2. Configure the database connection string inside:

```
appsettings.json
```

3. Apply database migrations

```
dotnet ef database update
```

4. Run the application

```
dotnet run
```

The database will be created automatically after migrations are applied.

---

## Purpose of the Project

This project was created as a hands-on learning project to simulate real world software development.
The goal was to gain practical experience in database design, debugging, background logic, user management and maintaining a growing codebase.

Instead of focusing on tutorials, the project was built by solving actual problems that appeared during development.

---

## Demo Video

You can watch a demonstration of the application here:
https://www.youtube.com/@stylianosboursanidis5247

---

## Contact

LinkedIn: https://www.linkedin.com/in/stylianos-boursanidis-1502b32aa/
GitHub: https://github.com/Stelioss4
Portfolio: https://www.steliosboursanidis.com/
