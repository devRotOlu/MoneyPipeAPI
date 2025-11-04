ALTER TABLE invoices
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';