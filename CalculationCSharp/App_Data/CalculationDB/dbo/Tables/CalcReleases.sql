CREATE TABLE [dbo].[CalcReleases] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [Scheme]        NVARCHAR (MAX) NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [User]          NVARCHAR (MAX) NULL,
    [Configuration] XML            NULL,
    [UpdateDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.CalcReleases] PRIMARY KEY CLUSTERED ([ID] ASC)
);

