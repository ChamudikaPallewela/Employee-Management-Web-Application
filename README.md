# Employee Management Web Application

## Description

This project is an ASP.NET MVC web application designed as a demonstration of C# web development skills. The application enables users to manage a list of employees and perform calculations related to working days between two dates, excluding weekends and public holidays.

## Features

- **Employee Management**
  - Create, update, and delete employee records.
  - Employee details include ID, Name, Email, and Job Position.

- **Working Days Calculator**
  - Calculate the number of working days between two given dates.
  - Exclude weekends and public holidays stored in the database.

## Technical Details

- **Frameworks & Tools**
  - ASP.NET MVC (Version 4+)
  - Entity Framework (ADO.NET Database First)
  - Bootstrap and custom CSS for UI
  - jQuery for client-side scripting

- **Architecture**
  - Service and Repository Layers
    - Service layer handles business logic.
    - Repository layer manages database operations.
  - Caching Mechanism
    - Long-term cache for generic results.
    - Short-term (5 minutes) cache for frequently accessed data.

- **Constraints**
  - Public holidays stored in a dedicated database table.
  - No direct database operations in controllers or views.
    
## Development Plan

1. **Understanding Requirements**
   - Develop a comprehensive understanding of the application functionalities.

2. **Project Plan**
   - Outline steps for development, including potential challenges and solutions.

3. **Implementation**
   - Follow a structured development process using MVC best practices.
   - Ensure the application adheres to coding standards and includes comments for clarity.

4. **Testing**
   - Validate employee management features and working days calculator logic.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/ChamudikaPallewela/Employee-Management-Web-Application.git
