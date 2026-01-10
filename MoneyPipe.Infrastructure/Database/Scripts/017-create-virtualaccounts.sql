CREATE TABLE VirtualAccounts (
    Id UUID PRIMARY KEY,
    WalletId UUID NOT NULL,
    BankName TEXT NOT NULL,
    AccountName TEXT NOT NULL,
    ProviderName VARCHAR(30) NOT NULL,
    ProviderAccountId TEXT NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt TIMESTAMPTZ NOT NULL,
    UpdatedAt TIMESTAMPTZ NOT NULL,
    Currency CHAR(3) NOT NULL DEFAULT 'NGN',
    IsPrimaryForInvoice BOOLEAN NOT NULL DEFAULT FALSE
);
