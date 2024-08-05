## Real Estate Management System (REMS)

The Real Estate Management System (REMS) is designed to facilitate the buying, selling, and renting of properties. This system allows users to manage property listings, transactions, appointments, and communications efficiently. Below is a summary of the domain logic and the corresponding database table structures.

### Domain Logic Summary

1. **Users**: Manages user information including their role in the system.
2. **Properties**: Stores details about properties available for sale or rent.
3. **Agents**: Contains information about agents who manage property listings.
4. **Clients**: Holds details about clients looking to buy, rent, or sell properties.
5. **Listings**: Manages property listings created by agents.
6. **Transactions**: Tracks transactions related to property sales and rentals.
7. **Appointments**: Manages appointments for property viewings.
8. **Property Images**: Stores images of properties.
9. **Reviews**: Contains user reviews for properties.
10. **Messages**: Manages communication between users and clients.

### Database Table Structures

#### Users
```sql
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(100) NOT NULL,
    phone NVARCHAR(15),
    role NVARCHAR(50) NOT NULL,
    date_created DATETIME DEFAULT GETDATE()
);
```

#### Properties
```sql
CREATE TABLE Properties (
    property_id INT PRIMARY KEY IDENTITY(1,1),
    agent_id INT FOREIGN KEY REFERENCES Agents(agent_id),
    address NVARCHAR(200) NOT NULL,
    city NVARCHAR(100) NOT NULL,
    state NVARCHAR(50) NOT NULL,
    zip_code NVARCHAR(10) NOT NULL,
    property_type NVARCHAR(50) NOT NULL,
    price DECIMAL(18, 2) NOT NULL,
    size DECIMAL(18, 2) NOT NULL,
    number_of_bedrooms INT,
    number_of_bathrooms INT,
    year_built INT,
    description NVARCHAR(MAX),
    status NVARCHAR(50) NOT NULL,
    availability_type NVARCHAR(50) NOT NULL,
    min_rental_period INT NULL,
    approved_by NVARCHAR(50) NULL,
    add_date DATETIME DEFAULT GETDATE(),
    edit_date DATETIME DEFAULT GETDATE()
);
```

#### Agents
```sql
CREATE TABLE Agents (
    agent_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    agency_name NVARCHAR(100) NOT NULL,
    license_number NVARCHAR(50) NOT NULL,
    phone NVARCHAR(15),
    email NVARCHAR(100),
    address NVARCHAR(200)
);
```

#### Clients
```sql
CREATE TABLE Clients (
    client_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    first_name NVARCHAR(100) NOT NULL,
    last_name NVARCHAR(100) NOT NULL,
    phone NVARCHAR(15),
    email NVARCHAR(100),
    address NVARCHAR(200)
);
```

#### Transactions
```sql
CREATE TABLE Transactions (
    transaction_id INT PRIMARY KEY IDENTITY(1,1),
    property_id INT FOREIGN KEY REFERENCES Properties(property_id),
    client_id INT FOREIGN KEY REFERENCES Clients(client_id),
    transaction_date DATETIME DEFAULT GETDATE(),
    rental_period INT NULL,
    sale_price DECIMAL(18, 2) NOT NULL,
    commission DECIMAL(18, 2),
    status NVARCHAR(50) NOT NULL
);
```

#### Appointments
```sql
CREATE TABLE Appointments (
    appointment_id INT PRIMARY KEY IDENTITY(1,1),
    client_id INT FOREIGN KEY REFERENCES Clients(client_id),
    property_id INT FOREIGN KEY REFERENCES Properties(property_id),
    appointment_date DATE NOT NULL,
    appointment_time TIME NOT NULL,
    status NVARCHAR(50) NOT NULL,
    notes NVARCHAR(MAX)
);
```

#### Property Images
```sql
CREATE TABLE PropertyImages (
    image_id INT PRIMARY KEY IDENTITY(1,1),
    property_id INT FOREIGN KEY REFERENCES Properties(property_id),
    image_url NVARCHAR(200) NOT NULL,
    description NVARCHAR(MAX),
    date_uploaded DATETIME DEFAULT GETDATE()
);
```

#### Reviews
```sql
CREATE TABLE Reviews (
    review_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    property_id INT FOREIGN KEY REFERENCES Properties(property_id),
    rating INT CHECK (rating BETWEEN 1 AND 5) NOT NULL,
    comments NVARCHAR(MAX),
    date_created DATETIME DEFAULT GETDATE()
);
```

#### Messages
```sql
CREATE TABLE Messages (
    message_id INT PRIMARY KEY IDENTITY(1,1),
    sender_id INT FOREIGN KEY REFERENCES Users(user_id),
    receiver_id INT FOREIGN KEY REFERENCES Users(user_id),
    property_id INT FOREIGN KEY REFERENCES Properties(property_id),
    message_content NVARCHAR(MAX) NOT NULL,
    date_sent DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50)
);
```
#### LoginTable
```sql
USE [REMS]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 8/4/2024 9:48:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nchar](120) NOT NULL,
    [Role] [nchar](120) NOT NULL,
    [AccessToken] [nchar](500) NOT NULL,
    [LoginDate] [datetime] NOT NULL,
    [Email] [nchar](120) NOT NULL,
    [LogoutDate] [datetime] NULL,
    CONSTRAINT [PK_Tbl_Login] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO

```
### Database First Approach

To scaffold the database using the Database First approach with Entity Framework Core, run the following commands:

```bash
dotnet ef dbcontext scaffold "Server=.;Database=REMS;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```

Alternatively, using the Package Manager Console:

```bash
Scaffold-DbContext "Server=.;Database=AdminPortal;User ID=sa; Password=sa@123;Integrated Security=True;Trusted_Connection=true;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir AppDbContext -Tables Tbl_AdminUserLogin -f
```

### CI

GitHub Actions