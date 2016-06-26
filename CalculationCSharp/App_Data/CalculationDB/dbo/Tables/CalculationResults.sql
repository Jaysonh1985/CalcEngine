CREATE TABLE [dbo].[CalculationResults] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [User]      NVARCHAR (100) NOT NULL,
    [Scheme]    NVARCHAR (100) NOT NULL,
    [Type]      NVARCHAR (100) NOT NULL,
    [RunDate]   DATETIME       NOT NULL,
    [Reference] NVARCHAR (MAX) NULL,
    [Input]     XML            NULL,
    [Output]    XML            NULL,
    CONSTRAINT [PK_dbo.CalculationResults] PRIMARY KEY CLUSTERED ([Id] ASC)
);

