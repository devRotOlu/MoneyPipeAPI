ALTER TABLE password_reset_tokens
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';

ALTER TABLE password_reset_tokens
ALTER COLUMN expiresat TYPE timestamptz USING expiresat AT TIME ZONE 'UTC';