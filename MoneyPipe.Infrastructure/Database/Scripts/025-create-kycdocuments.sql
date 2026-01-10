CREATE TABLE KycDocuments (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL REFERENCES Users(Id),
    KycProfileId UUID NOT NULL REFERENCES KycProfiles(Id) ON DELETE CASCADE,
    Category VARCHAR(12) NOT NULL CHECK(Category IN ('Individual','Business')),
    Type VARCHAR(50) NOT NULL,
    Value TEXT NOT NULL,
    Issuer TEXT NOT NULL,
    IsVerified BOOLEAN DEFAULT false NOT NULL,
    VerifiedAt TIMESTAMPTZ
);
