CREATE TABLE [dbo].[Codes] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Group] NVARCHAR (MAX) NULL,
    [Code]  NVARCHAR (MAX) NULL,
    [Value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Codes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

