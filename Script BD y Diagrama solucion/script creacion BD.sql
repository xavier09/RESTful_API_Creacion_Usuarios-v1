USE [master]
GO
CREATE DATABASE [usuarios_api]
ALTER DATABASE [usuarios_api] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [usuarios_api].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [usuarios_api] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [usuarios_api] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [usuarios_api] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [usuarios_api] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [usuarios_api] SET ARITHABORT OFF 
GO
ALTER DATABASE [usuarios_api] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [usuarios_api] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [usuarios_api] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [usuarios_api] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [usuarios_api] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [usuarios_api] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [usuarios_api] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [usuarios_api] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [usuarios_api] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [usuarios_api] SET  DISABLE_BROKER 
GO
ALTER DATABASE [usuarios_api] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [usuarios_api] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [usuarios_api] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [usuarios_api] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [usuarios_api] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [usuarios_api] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [usuarios_api] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [usuarios_api] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [usuarios_api] SET  MULTI_USER 
GO
ALTER DATABASE [usuarios_api] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [usuarios_api] SET DB_CHAINING OFF 
GO
ALTER DATABASE [usuarios_api] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [usuarios_api] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [usuarios_api] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [usuarios_api] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [usuarios_api] SET QUERY_STORE = ON
GO
ALTER DATABASE [usuarios_api] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [usuarios_api]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOS](
	[UUID] [varchar](256) NOT NULL,
	[NAME_FIELD] [varchar](256) NULL,
	[EMAIL] [varchar](256) NULL,
	[PASSWORD_FIELD] [varchar](256) NULL,
	[NUMBER] [int] NULL,
	[CITY_CODE] [int] NULL,
	[COUNTRY_CODE] [int] NULL,
	[CREATED] [datetime] NULL,
	[MODIFIED] [datetime] NULL,
	[LAST_LOGIN] [datetime] NULL,
	[ISACTIVE] [nchar](3) NULL,
	[JWT_TOKEN] [varchar](max) NULL
PRIMARY KEY CLUSTERED 
(
	[UUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[USUARIOS] ADD  DEFAULT (newid()) FOR [UUID],
CONSTRAINT [DF_USUARIOS_CREATED]  DEFAULT (getdate()) FOR [CREATED], 
CONSTRAINT [DF_USUARIOS_LAST_LOGIN]  DEFAULT (getdate()) FOR [LAST_LOGIN],
CONSTRAINT [DF_USUARIOS_ISACTIVE]  DEFAULT (N'Yes') FOR [ISACTIVE]
GO
USE [master]
GO
ALTER DATABASE [usuarios_api] SET  READ_WRITE 
GO
USE usuarios_api
GO
INSERT INTO [dbo].USUARIOS
           ([NAME_FIELD]
           ,[EMAIL]
           ,[PASSWORD_FIELD]
           ,[NUMBER]
           ,[CITY_CODE]
           ,[COUNTRY_CODE])
     VALUES
           ('xavier vasquez'
           ,'admin@admin.org'
           , '123'
           ,587125454
           ,10
           ,41)