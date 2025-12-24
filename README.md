# Hotel Reservation System

Web application for searching and booking hotel rooms. The project is implemented using Clean Architecture.

**Live Demo:** [http://hotel-booking-app.runasp.net/](http://hotel-booking-app.runasp.net/)

## Key Features

### For Clients:
* **Search:** Find hotels by city and number of guests.
* **Booking:** Book rooms with date selection.
* **History:** View personal booking history.
* **Account:** Registration and login to personal cabinet.

### For Administrators:
* **Management:** CRUD operations for hotels and rooms (create, edit, delete).
* **Analytics:** View hotel profitability statistics (implemented via Dapper + Chart.js graphs).

## Technology Stack

* **Backend:** ASP.NET Core 8 (MVC)
* **Database:** MySQL
* **ORM:** Entity Framework Core (main database work) + Dapper (for complex analytics)
* **Auth:** ASP.NET Core Identity (Admin/Client roles)
* **Frontend:** Bootstrap 5, Chart.js
* **Hosting:** MonsterASP.NET

## Architecture
The project is built according to **Clean Architecture** principles:

* **Domain:** Entities (`Hotel`, `Room`, `Booking`) and business rules.
* **Application:** Interfaces and DTOs.
* **Infrastructure:** Implementation of data access (`ApplicationDbContext`, migrations).
* **Web:** Presentation layer (Controllers, Views).

## Admin Access
The system has a pre-configured administrator for managing hotels and viewing statistics.

| Role | Email | Password |
| :--- | :--- | :--- |
| **Admin** | `admin@hotel.com` | `admin123` |


## How to Run Locally

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/your-username/hotel-booking-app.git](https://github.com/your-username/hotel-booking-app.git)
    ```

2.  **Configure the connection string** to MySQL in `appsettings.json` or `docker-compose.yml`.

3.  **Run Docker** (optional, if using a container for the DB):
    ```bash
    docker-compose up -d
    ```

4.  **Run the project** via Visual Studio or CLI:
    ```bash
    dotnet run --project Web
    ```

> On the first launch, the application will automatically create the database and populate it with test data via `DbInitializer`.
