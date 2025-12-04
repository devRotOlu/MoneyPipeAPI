CREATE TABLE Invoices (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL,
    InvoiceNumber TEXT NOT NULL,
    SubTotal NUMERIC(18,4),
    TaxAmount NUMERIC(18,4),
    TotalAmount NUMERIC(18,4),
    Currency TEXT NOT NULL,
    Status TEXT NOT NULL,
    DueDate TIMESTAMPTZ,
    IssueDate TIMESTAMPTZ NOT NULL,
    PaidAt TIMESTAMPTZ,
    CustomerName TEXT NOT NULL,
    CustomerEmail TEXT NOT NULL,
    CustomerAddress TEXT,
    Notes TEXT,
    PaymentUrl TEXT,
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ NULL
);

-- Create the sequence only if it doesn't exist
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_class WHERE relname = 'invoice_number_seq') THEN
        CREATE SEQUENCE invoice_number_seq START 1;
    END IF;
END$$;