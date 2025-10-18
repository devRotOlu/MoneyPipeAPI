CREATE TABLE Invoices (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    InvoiceNumber VARCHAR(50) NOT NULL UNIQUE,
    Amount DECIMAL(18,2) NOT NULL,
    Currency VARCHAR(10) NOT NULL,
    Description TEXT,
    PaymentUrl TEXT,
    Status VARCHAR(20) DEFAULT 'Pending',
    DueDate DATE,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
