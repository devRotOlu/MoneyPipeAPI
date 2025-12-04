CREATE TABLE InvoiceItems (
    Id UUID PRIMARY KEY,
    InvoiceId UUID NOT NULL REFERENCES invoices(id) ON DELETE CASCADE,
    Description TEXT NOT NULL,
    Quantity INT,
    UnitPrice NUMERIC(18,4),
    TotalPrice NUMERIC(18,4)
);
