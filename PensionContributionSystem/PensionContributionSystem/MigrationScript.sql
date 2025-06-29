IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Employers] (
    [Id] uniqueidentifier NOT NULL,
    [CompanyName] nvarchar(max) NOT NULL,
    [RegistrationNumber] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Employers] PRIMARY KEY ([Id])
);

CREATE TABLE [Members] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Members] PRIMARY KEY ([Id])
);

CREATE TABLE [Benefits] (
    [Id] uniqueidentifier NOT NULL,
    [MemberId] uniqueidentifier NOT NULL,
    [BenefitType] nvarchar(max) NOT NULL,
    [CalculationDate] datetime2 NOT NULL,
    [EligibilityStatus] bit NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Benefits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Benefits_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Contributions] (
    [Id] uniqueidentifier NOT NULL,
    [MemberId] uniqueidentifier NOT NULL,
    [ContributionType] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [ContributionDate] datetime2 NOT NULL,
    [ReferenceNumber] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Contributions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contributions_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Benefits_MemberId] ON [Benefits] ([MemberId]);

CREATE INDEX [IX_Contributions_MemberId_ContributionDate] ON [Contributions] ([MemberId], [ContributionDate]);

CREATE UNIQUE INDEX [IX_Members_Email] ON [Members] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629230724_InitialCreate', N'9.0.6');

COMMIT;
GO

