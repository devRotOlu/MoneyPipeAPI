ALTER TABLE refreshtokens
ALTER COLUMN expiresat TYPE timestamptz USING expiresat AT TIME ZONE 'UTC';

ALTER TABLE refreshtokens
ALTER COLUMN createdat TYPE timestamptz USING createdat AT TIME ZONE 'UTC';

ALTER TABLE refreshtokens
ALTER COLUMN revokedat TYPE timestamptz USING revokedat AT TIME ZONE 'UTC';
