CREATE TABLE Payments (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    InvoiceId UUID NOT NULL REFERENCES Invoices(Id) ON DELETE CASCADE,
    Reference VARCHAR(100) NOT NULL UNIQUE,
    Amount DECIMAL(18,2) NOT NULL,
    Currency VARCHAR(10) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    PaidAt TIMESTAMP,
    Gateway VARCHAR(50) DEFAULT 'Paystack',
    RawResponse JSONB,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
