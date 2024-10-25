USE [KironTest]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 2024/10/25 17:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[HolidayTitleId] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Notes] [varchar](max) NULL,
	[Bunting] [bit] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HolidayTitles]    Script Date: 2024/10/25 17:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HolidayTitles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_HolidayTitles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Navigation]    Script Date: 2024/10/25 17:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Navigation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [varchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
 CONSTRAINT [PK_Navigation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reqions]    Script Date: 2024/10/25 17:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reqions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Division] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Reqions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/10/25 17:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Username]  DEFAULT (N'ERR') FOR [Username]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Password]  DEFAULT (N'ERR') FOR [Password]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_HolidayTitles] FOREIGN KEY([HolidayTitleId])
REFERENCES [dbo].[HolidayTitles] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_HolidayTitles]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Reqions] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Reqions] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Reqions]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBankHolidaysById]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllBankHolidaysById] 	
	@RegionId int
AS 
BEGIN

  BEGIN
    SELECT 
		rg.Description as Region,
		rg.Division,
		ht.Description as Title,
		ev.Date,
		ev.Notes,
		ev.Bunting
	FROM Events ev
	INNER JOIN Reqions rg on rg.Id = ev.RegionId
	INNER JOIN HolidayTitles ht on ht.Id = ev.HolidayTitleId
	WHERE RegionId = @RegionId
  END
      
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllRegions]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllRegions] 
AS 
BEGIN

  SELECT Id, Description, Division FROM Reqions
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetSitemap]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Quentin Engelbrecht
-- 2024/10/19
-- Get all navigation routes.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetSitemap]
AS
BEGIN
SET NOCOUNT ON
 
SELECT ID, Text, ParentID FROM Navigation ORDER BY ParentID
 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUser]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetUser] 
	@Username varchar(50)
AS
BEGIN
SET NOCOUNT ON
 
SELECT Id, Username, Password FROM Users WHERE Username = @Username
 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_HasAnyHolidays]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HasAnyHolidays] 
	@HasAny bit output
AS 
BEGIN

  SET @HasAny = 0;

  IF(EXISTS(SELECT 1 FROM dbo.Events))
    BEGIN
      SET @HasAny = 1;
	  RETURN @HasAny
    END
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SetEvent]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SetEvent] 
	@RegionId int,
	@HolidayTitleId int,
	@Date Date,
	@Notes varchar(max),
	@Bunting bit
AS 
BEGIN

  BEGIN
      INSERT INTO Events (RegionId, HolidayTitleId, Date, Notes, Bunting)
      VALUES (@RegionId, @HolidayTitleId, @Date, @Notes, @Bunting)           
  END
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SetHolidayTitle]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SetHolidayTitle] 
	@Description varchar(50), 
	@Id int output
AS 
BEGIN

  IF EXISTS(SELECT * FROM HolidayTitles WHERE Description = @Description)

    BEGIN
      SELECT @Id = Id FROM HolidayTitles WHERE Description = @Description
	  RETURN  @Id
    END

  ELSE

    BEGIN
      INSERT INTO HolidayTitles (Description)
      VALUES (@Description)     
      SET @Id=SCOPE_IDENTITY()
      RETURN  @Id    
    END
      
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SetRegion]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SetRegion] 
	@Name varchar(50), 
	@Division varchar(50),
	@Id int output
AS 
BEGIN

  IF EXISTS(SELECT * FROM Reqions WHERE Description = @Name)

    BEGIN
      SELECT @Id = Id FROM Reqions WHERE Description = @Name
	  RETURN  @Id
    END

  ELSE

    BEGIN
      INSERT INTO Reqions (Description, Division)
      VALUES (@Name, @Division)     
      SET @Id=SCOPE_IDENTITY()
      RETURN  @Id    
    END
      
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SetUser]    Script Date: 2024/10/25 17:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SetUser] 
	@Username varchar(50), 
	@Password varchar(MAX),
	@Id int output
AS 
BEGIN
  INSERT INTO Users (Username, Password)
  VALUES (@Username, @Password)   
  
  SET @Id=SCOPE_IDENTITY()
  RETURN  @Id
END

GO
