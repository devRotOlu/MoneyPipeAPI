CREATE TABLE Transactions (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    WalletId UUID NOT NULL REFERENCES Wallets(Id) ON DELETE CASCADE,
    Type VARCHAR(10) NOT NULL CHECK (Type IN ('Credit', 'Debit')),
    Amount DECIMAL(18,2) NOT NULL,
    Currency VARCHAR(10) NOT NULL,
    Reference VARCHAR(100),
    Description TEXT,
    CreatedAt TIMESTAMP DEFAULT NOW()
);
