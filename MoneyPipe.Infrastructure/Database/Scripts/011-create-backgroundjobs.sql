CREATE TABLE BackgroundJobs (
    Id UUID PRIMARY KEY,
    Type TEXT NOT NULL,
    Payload TEXT NOT NULL,
    Status TEXT NOT NULL CHECK (LOWER(status) IN ('pending', 'completed', 'failed')),
    Attempts INT NOT NULL,
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW()
);
