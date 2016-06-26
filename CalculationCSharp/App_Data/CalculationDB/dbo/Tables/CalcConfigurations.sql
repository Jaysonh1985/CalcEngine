CREATE TABLE [dbo].[CalcConfigurations] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [User]          NVARCHAR (MAX) NULL,
    [Configuration] XML            NULL,
    [UpdateDate]    DATETIME       NOT NULL,
    [Scheme]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.CalcConfigurations] PRIMARY KEY CLUSTERED ([ID] ASC)
);

