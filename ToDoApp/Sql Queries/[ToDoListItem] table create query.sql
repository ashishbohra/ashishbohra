USE [ToDoAppDB]
GO

/****** Object:  Table [dbo].[ToDoListItem]    Script Date: 27-03-2021 14:10:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ToDoListItem](
	[ToDoListId] [int] IDENTITY(1,1) NOT NULL,
	[ToDoListName] [nvarchar](50) NOT NULL,
	[ToDoListItem] [nvarchar](100) NOT NULL,
	[priority] [int] NOT NULL
) ON [PRIMARY]
GO


