CREATE TABLE [dbo].[ProjectBoards] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [Group]         NVARCHAR (MAX) NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [User]          NVARCHAR (MAX) NULL,
    [Configuration] XML            NULL,
    [UpdateDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.ProjectBoards] PRIMARY KEY CLUSTERED ([ID] ASC)
);

