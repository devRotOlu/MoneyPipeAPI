ALTER TABLE transactions
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';