ALTER TABLE users
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';

ALTER TABLE users
ALTER COLUMN updatedat TYPE timestamptz USING updatedat AT TIME ZONE 'UTC';

ALTER TABLE users
ALTER COLUMN emailconfirmationexpiry TYPE timestamptz USING emailconfirmationexpiry AT TIME ZONE 'UTC';