CREATE TABLE KycProfiles (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    PhoneNumber VARCHAR(25) NOT NULL,
    Street TEXT NOT NULL,
    City TEXT NOT NULL,
    State TEXT NOT NULL,
    Country TEXT NOT NULL,
    PostalCode VARCHAR(12) NOT NULL,
    RejectionReason TEXT,
    Status VARCHAR(20) CHECK(Status IN ('NotStarted','InProgress',
    'Submitted','PartiallyVerified','Verified','Rejected','Suspended')),
    VerifiedAt TIMESTAMPTZ
);
