CREATE TABLE EmailJobs (
    Id UUID PRIMARY KEY,
    Email TEXT NOT NULL,
    Subject TEXT NOT NULL,
    Message TEXT NOT NULL,
    Status TEXT DEFAULT 'pending', -- pending, sent, failed
    Attempts INT DEFAULT 0,
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW()
);

