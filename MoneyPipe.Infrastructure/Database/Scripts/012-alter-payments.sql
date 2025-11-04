ALTER TABLE payments
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';

ALTER TABLE payments
ALTER COLUMN paidat TYPE timestamptz USING paidat AT TIME ZONE 'UTC';