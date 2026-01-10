ALTER TABLE Invoices
ADD COLUMN IF NOT EXISTS PaymentMethod VARCHAR(15) CHECK(PaymentMethod IN ('BankTransfer','Card'));

ALTER TABLE Invoices
ADD COLUMN IF NOT EXISTS PaymentReference TEXT;

ALTER TABLE Invoices
ADD COLUMN IF NOT EXISTS PaymentProvider VARCHAR(30);