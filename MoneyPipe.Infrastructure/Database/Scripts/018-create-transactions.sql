CREATE TABLE Transactions (
    Id UUID PRIMARY KEY,
    WalletId UUID NOT NULL REFERENCES Wallets(Id) ON DELETE CASCADE,
    Direction VARCHAR(6) NOT NULL CHECK(Direction IN ('Credit','Debit')),
    Amount NUMERIC(18,4) NOT NULL,
    Currency CHAR(3) NOT NULL,
    Created  At TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Status VARCHAR(8) NOT NULL CHECK(Status IN ('Pending','Settled','Failed','Reversed')),
    Type VARCHAR(10) NOT NULL CHECK(Type IN ('Deposit','Withdrawal','Payment')),  
    ProviderName VARCHAR(30) NOT NULL,
    ProviderReference TEXT NOT NULL,
    RelatedTransactionId UUID REFERENCES Transactions(Id),
    CONSTRAINT unique_provider_reference UNIQUE (ProviderName, ProviderReference)
);