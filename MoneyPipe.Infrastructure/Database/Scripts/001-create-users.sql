CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    DefaultCurrency VARCHAR(10) NOT NULL,
    CreatedAt TIMESTAMPTZ NOT NULL,
    UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
    EmailConfirmed boolean DEFAULT FALSE NOT NULL,
    EmailConfirmationToken TEXT NULL,
    EmailConfirmationExpiry TIMESTAMPTZ NULL
);
