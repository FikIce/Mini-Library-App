📚Mini Library MVC

This project is a complete, data-driven web application for managing a small library's catalog and lending system. It provides a public-facing interface for users to browse books and an administrative backend for librarians to manage the collection and member loans.

Core Features

📖 Public Book Catalog: A searchable and filterable public-facing page where users can browse all available books in the library's collection.

👤 Member Management: A system for library staff to register new members and manage existing ones, including their borrowing history.

⚙️ Full Catalog Control: Provides administrators with full CRUD (Create, Read, Update, Delete) functionality for the book catalog, including managing titles, authors, and stock levels.

📤 Checkout & Return System: A core feature that allows a librarian to check a book out to a member and process its return, automatically updating the book's availability.

Key Technical Features & Architecture

🧱 ASP.NET MVC Core: Built on the .NET MVC framework to ensure a clean and maintainable separation of concerns between the data models, business logic, and user interface.

🗃️ Entity Framework Core: Uses EF Core with a code-first approach to manage the application's complex relational database.

🔗 Relational Data Model: The architecture is built on a sound relational model that correctly links `Books`, `Members`, and `Loans`. This ensures that a book's availability is always in sync with its loan records.

📦 ViewModels: Deploys ViewModels to act as a clean data contract between the controllers and the views, ensuring that only necessary and formatted data is sent to the frontend.

Tech Stack

🖥️ Backend: .NET 8 (MVC)
🗄️ Database: Entity Framework Core 6, Microsoft SQL Server
🎨 Frontend: Razor Views, Bootstrap 5
🏗️ Architecture: MVC, ViewModels
