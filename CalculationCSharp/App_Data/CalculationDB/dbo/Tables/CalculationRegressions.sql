CREATE TABLE [dbo].[CalculationRegressions] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Scheme]          NVARCHAR (MAX) NULL,
    [Type]            NVARCHAR (MAX) NULL,
    [OriginalRunDate] DATETIME       NOT NULL,
    [LatestRunDate]   DATETIME       NOT NULL,
    [Reference]       NVARCHAR (MAX) NULL,
    [Input]           XML            NULL,
    [OutputOld]       XML            NULL,
    [OutputNew]       XML            NULL,
    [Difference]      XML            NULL,
    [Pass]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.CalculationRegressions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

