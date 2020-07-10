
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/10/2020 09:57:41
-- Generated from EDMX file: C:\Users\a.volodin\source\repos\DepoResult\DepoResult\Connect.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [RealBase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Depo_Result]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Depo_Result];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Depo_Result'
CREATE TABLE [dbo].[Depo_Result] (
    [SN] int  NOT NULL,
    [ResultFileName] nvarchar(50)  NULL,
    [ResultData] varbinary(500)  NULL,
    [RegDate] datetime  NULL,
    [Count] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [SN] in table 'Depo_Result'
ALTER TABLE [dbo].[Depo_Result]
ADD CONSTRAINT [PK_Depo_Result]
    PRIMARY KEY CLUSTERED ([SN] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------