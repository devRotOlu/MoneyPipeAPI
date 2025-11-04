CREATE TABLE Users (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Role VARCHAR(20) DEFAULT 'User' CHECK (Role IN ('User', 'Admin')),
    DefaultCurrency VARCHAR(10) DEFAULT 'NGN',
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW(),
    EmailConfirmed boolean DEFAULT FALSE NOT NULL,
    EmailConfirmationToken TEXT NULL,
    EmailConfirmationExpiry TIMESTAMP NULL
);
