USE
[REMS]
GO
/****** Object:  Table [dbo].[Agents]    Script Date: 8/28/2024 4:51:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agents]
(
    [
    agent_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [user_id] [int] NULL,
    [agency_name] [nvarchar]
(
    100
) NOT NULL,
    [license_number] [nvarchar]
(
    50
) NOT NULL,
    [address] [nvarchar]
(
    200
) NULL,
    PRIMARY KEY CLUSTERED
(
[
    agent_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Appointments]
(
    [
    appointment_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [client_id] [int] NULL,
    [property_id] [int] NULL,
    [appointment_date] [date] NOT NULL,
    [appointment_time] [time]
(
    7
) NOT NULL,
    [status] [nvarchar]
(
    50
) NOT NULL,
    [notes] [nvarchar]
(
    max
) NULL,
    PRIMARY KEY CLUSTERED
(
[
    appointment_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Clients]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Clients]
(
    [
    client_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [user_id] [int] NULL,
    [first_name] [nvarchar]
(
    100
) NOT NULL,
    [last_name] [nvarchar]
(
    100
) NOT NULL,
    [address] [nvarchar]
(
    200
) NULL,
    PRIMARY KEY CLUSTERED
(
[
    client_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Login]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Login]
(
    [
    Id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [UserId] [nchar]
(
    120
) NOT NULL,
    [Role] [nchar]
(
    120
) NOT NULL,
    [AccessToken] [nchar]
(
    500
) NOT NULL,
    [LoginDate] [datetime] NOT NULL,
    [Email] [nchar]
(
    120
) NOT NULL,
    [LogoutDate] [datetime] NULL,
    CONSTRAINT [PK_Tbl_Login] PRIMARY KEY CLUSTERED
(
[
    Id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Messages]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Messages]
(
    [
    message_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [sender_id] [int] NULL,
    [receiver_id] [int] NULL,
    [property_id] [int] NULL,
    [message_content] [nvarchar]
(
    max
) NOT NULL,
    [date_sent] [datetime] NULL,
    [status] [nvarchar]
(
    50
) NULL,
    PRIMARY KEY CLUSTERED
(
[
    message_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Properties]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Properties]
(
    [
    property_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [agent_id] [int] NULL,
    [address] [nvarchar]
(
    200
) NOT NULL,
    [city] [nvarchar]
(
    100
) NOT NULL,
    [state] [nvarchar]
(
    50
) NOT NULL,
    [zip_code] [nvarchar]
(
    10
) NOT NULL,
    [property_type] [nvarchar]
(
    50
) NOT NULL,
    [price] [decimal]
(
    18,
    2
) NOT NULL,
    [size] [decimal]
(
    18,
    2
) NOT NULL,
    [number_of_bedrooms] [int] NULL,
    [number_of_bathrooms] [int] NULL,
    [year_built] [int] NULL,
    [description] [nvarchar]
(
    max
) NULL,
    [status] [nvarchar]
(
    50
) NOT NULL,
    [availiablity_type] [nvarchar]
(
    50
) NOT NULL,
    [minrental_period] [int] NULL,
    [approvedby] [nvarchar]
(
    50
) NULL,
    [adddate] [datetime] NULL,
    [editdate] [datetime] NULL,
    PRIMARY KEY CLUSTERED
(
[
    property_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[PropertyImages]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[PropertyImages]
(
    [
    image_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [property_id] [int] NULL,
    [image_url] [nvarchar]
(
    200
) NOT NULL,
    [description] [nvarchar]
(
    max
) NULL,
    [date_uploaded] [datetime] NULL,
    PRIMARY KEY CLUSTERED
(
[
    image_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Reviews]
(
    [
    review_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [user_id] [int] NULL,
    [property_id] [int] NULL,
    [rating] [int] NOT NULL,
    [comments] [nvarchar]
(
    max
) NULL,
    [date_created] [datetime] NULL,
    PRIMARY KEY CLUSTERED
(
[
    review_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Session]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Session]
(
    [
    Id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [UserId] [int] NOT NULL,
    [RefreshToken] [varchar]
(
    200
) NOT NULL,
    [ExpiredTime] [datetime] NOT NULL,
    [LastActiveTime] [datetime] NOT NULL,
    CONSTRAINT [PK_Tbl_Session] PRIMARY KEY CLUSTERED
(
[
    Id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Transactions]
(
    [
    transaction_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [property_id] [int] NULL,
    [client_id] [int] NULL,
    [transaction_date] [datetime] NULL,
    [rental_period] [int] NULL,
    [sale_price] [decimal]
(
    18,
    2
) NOT NULL,
    [commission] [decimal]
(
    18,
    2
) NULL,
    [status] [nvarchar]
(
    50
) NOT NULL,
    PRIMARY KEY CLUSTERED
(
[
    transaction_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/28/2024 4:51:49 PM ******/
    SET ANSI_NULLS
    ON
    GO
    SET QUOTED_IDENTIFIER
    ON
    GO
CREATE TABLE [dbo].[Users]
(
    [
    user_id] [
    int]
    IDENTITY
(
    1,
    1
) NOT NULL,
    [name] [nvarchar]
(
    100
) NOT NULL,
    [email] [nvarchar]
(
    100
) NOT NULL,
    [password] [nvarchar]
(
    100
) NOT NULL,
    [phone] [nvarchar]
(
    15
) NULL,
    [role] [nvarchar]
(
    50
) NOT NULL,
    [date_created] [datetime] NULL,
    PRIMARY KEY CLUSTERED
(
[
    user_id]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY],
    UNIQUE NONCLUSTERED
(
[
    email]
    ASC
)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    ON [PRIMARY]
    )
    ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Messages] ADD DEFAULT (getdate()) FOR [date_sent]
    GO
ALTER TABLE [dbo].[Properties] ADD DEFAULT (getdate()) FOR [adddate]
    GO
ALTER TABLE [dbo].[Properties] ADD DEFAULT (getdate()) FOR [editdate]
    GO
ALTER TABLE [dbo].[PropertyImages] ADD DEFAULT (getdate()) FOR [date_uploaded]
    GO
ALTER TABLE [dbo].[Reviews] ADD DEFAULT (getdate()) FOR [date_created]
    GO
ALTER TABLE [dbo].[Transactions] ADD DEFAULT (getdate()) FOR [transaction_date]
    GO
ALTER TABLE [dbo].[Users] ADD DEFAULT (getdate()) FOR [date_created]
    GO
ALTER TABLE [dbo].[Agents] WITH CHECK ADD FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users] ([user_id])
    GO
ALTER TABLE [dbo].[Appointments] WITH CHECK ADD FOREIGN KEY ([client_id])
    REFERENCES [dbo].[Clients] ([client_id])
    GO
ALTER TABLE [dbo].[Appointments] WITH CHECK ADD FOREIGN KEY ([property_id])
    REFERENCES [dbo].[Properties] ([property_id])
    GO
ALTER TABLE [dbo].[Clients] WITH CHECK ADD FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users] ([user_id])
    GO
ALTER TABLE [dbo].[Messages] WITH CHECK ADD FOREIGN KEY ([property_id])
    REFERENCES [dbo].[Properties] ([property_id])
    GO
ALTER TABLE [dbo].[Messages] WITH CHECK ADD FOREIGN KEY ([receiver_id])
    REFERENCES [dbo].[Users] ([user_id])
    GO
ALTER TABLE [dbo].[Messages] WITH CHECK ADD FOREIGN KEY ([sender_id])
    REFERENCES [dbo].[Users] ([user_id])
    GO
ALTER TABLE [dbo].[Properties] WITH CHECK ADD FOREIGN KEY ([agent_id])
    REFERENCES [dbo].[Agents] ([agent_id])
    GO
ALTER TABLE [dbo].[PropertyImages] WITH CHECK ADD FOREIGN KEY ([property_id])
    REFERENCES [dbo].[Properties] ([property_id])
    GO
ALTER TABLE [dbo].[Reviews] WITH CHECK ADD FOREIGN KEY ([property_id])
    REFERENCES [dbo].[Properties] ([property_id])
    GO
ALTER TABLE [dbo].[Reviews] WITH CHECK ADD FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users] ([user_id])
    GO
ALTER TABLE [dbo].[Transactions] WITH CHECK ADD FOREIGN KEY ([client_id])
    REFERENCES [dbo].[Clients] ([client_id])
    GO
ALTER TABLE [dbo].[Transactions] WITH CHECK ADD FOREIGN KEY ([property_id])
    REFERENCES [dbo].[Properties] ([property_id])
    GO
ALTER TABLE [dbo].[Reviews] WITH CHECK ADD CHECK (([rating]>=(1) AND [rating]<=(5)))
    GO




/****** Object:  StoredProcedure [dbo].[sp_Dashboard]    Script Date: 9/18/2024 10:53:10 PM ******/
DROP PROCEDURE [dbo].[sp_Dashboard]
GO

/****** Object:  StoredProcedure [dbo].[sp_Dashboard]    Script Date: 9/18/2024 10:53:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------
-- TEST SCRIPT
---------------------------------------------------------------------------------------------------
/*

EXEC sp_Dashboard

*/
---------------------------------------------------------------------------------------------------
-- Change History
---------------------------------------------------------------------------------------------------
-- 18/Sep/2024	HEIN	- Create New SP for Dashboard
---------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[sp_Dashboard] 

AS

BEGIN
	---------------------------------------------------------------------------------------
	-- Overview 
	---------------------------------------------------------------------------------------
	DECLARE @TotalAgent				INT					SET @TotalAgent = 0
	DECLARE @TotalClients			INT					SET @TotalClients = 0
	DECLARE @Properties				INT					SET @Properties = 0
	DECLARE @PropertySoldIncome		DECIMAL(18,6)		SET @PropertySoldIncome = 0
	DECLARE @PropertyRentedIncome	DECIMAL(18,6)		SET @PropertyRentedIncome = 0
	DECLARE @TotalRevenue			DECIMAL(18,6)		SET @TotalRevenue = 0

	CREATE TABLE #Overview
	(
		Agents					INT,
		Clients					INT,
		Properties				INT,
		PropertySoldIncome		DECIMAL(18,2),
		PropertyRentedIncome	DECIMAL(18,2),
		TotalRevenue			DECIMAL(18,2)
		
	)

	SELECT @TotalAgent = Count(*) FROM Users (NOLOCK) WHERE role='Agent'
	SELECT @TotalClients = Count(*) FROM Users (NOLOCK) WHERE role='Client'
	SELECT @Properties = Count(*) FROM Properties (NOLOCK)

	select @PropertySoldIncome = SUM(ISNULL(TR.Commission,0)) from Transactions AS TR INNER JOIN Properties AS P ON P.property_id = TR.property_id AND P.availiablity_type='Sell'

	select @PropertyRentedIncome = SUM(ISNULL(TR.Commission,0)) from Transactions AS TR INNER JOIN Properties AS P ON P.property_id = TR.property_id AND P.availiablity_type='Rent'

	select @TotalRevenue = SUM(ISNULL(TR.Commission,0)) from Transactions AS TR INNER JOIN Properties AS P ON P.property_id = TR.property_id

	INSERT INTO #Overview (Agents, Clients, Properties, PropertySoldIncome, PropertyRentedIncome, TotalRevenue)
	VALUES (@TotalAgent, @TotalClients, @Properties, @PropertySoldIncome, @PropertyRentedIncome, @TotalRevenue)
	
	SELECT * FROM #Overview
	DROP TABLE #Overview
	---------------------------------------------------------------------------------------
	---------------------------------------------------------------------------------------
	-- AgentActivityTable
	---------------------------------------------------------------------------------------

	CREATE TABLE #AgentActTable
	(   
	    AgentId				INT,
		AgentName			NVARCHAR(100),
		SellProperty		INT,
		RentedProperty		INT,
		TotalSales			DECIMAL(18,2),
		CommissionEarned	DECIMAL(18,2)
	)

	CREATE TABLE #ReturnAgentActTable
	(   
	    AgentId				INT,
		AgentName			NVARCHAR(100),
		SellProperty		INT,
		RentedProperty		INT,
		TotalSales			DECIMAL(18,2),
		CommissionEarned	DECIMAL(18,2)
	)

	INSERT INTO #AgentActTable (AgentId,AgentName,SellProperty, RentedProperty, TotalSales, CommissionEarned)
	SELECT A.agent_id, U.name AS AgentName,
	CASE WHEN P.status = 'Sold' THEN Count(P.agent_id) ELSE 0 END AS SellProperty,
	CASE WHEN P.status = 'Rented' THEN Count(P.agent_id) ELSE 0 END AS RentedProperty,
	CASE WHEN P.status = 'Sold' THEN SUM(ISNULL(TR.commission,0)) ELSE 0 END AS TotalSales,
	CASE WHEN P.status = 'Rented' THEN SUM(ISNULL(TR.commission,0)) ELSE 0 END AS CommissionEarned
	FROM Agents AS A
	INNER JOIN Properties AS P ON P.agent_id = A.agent_id
	INNER JOIN Users AS U ON U.user_id = A.user_id
	LEFT OUTER JOIN Transactions AS TR ON TR.property_id = P.property_id
	GROUP BY A.agent_id,name,P.status Order by A.agent_id

	INSERT INTO #ReturnAgentActTable (AgentId,AgentName,SellProperty, RentedProperty, TotalSales, CommissionEarned)
	SELECT AgentId,AgentName, SUM(ISNULL(SellProperty,0)), SUM(ISNULL(RentedProperty,0)), SUM(ISNULL(TotalSales,0)), SUM(ISNULL(CommissionEarned,0)) FROM #AgentActTable 
	GROUP By AgentId, AgentName

	SELECT AgentName,SellProperty, RentedProperty, TotalSales, CommissionEarned
	FROM #ReturnAgentActTable Order By (ISNULL(SellProperty,0) + ISNULL(RentedProperty,0)) DESC
	
	DROP Table #ReturnAgentActTable
	DROP Table #AgentActTable

	---------------------------------------------------------------------------------------
	-- WeeklyActivityTable
	---------------------------------------------------------------------------------------

	CREATE TABLE #WeeklyAct
	(   
	    Name				NVARCHAR(100),
		Sold				DECIMAL(18,2),
		Rented				DECIMAL(18,2)
	)

	DECLARE @Today			DATE = GETDATE();
	DECLARE @Last7Days		DATE = DATEADD(DAY, -7, @Today);
	DECLARE @CurrentDate	DATE = @Last7Days;
	DECLARE @DATENAME		NVARCHAR(20)
	DECLARE @Sold			DECIMAL(18,2)
	DECLARE @Rented			DECIMAL(18,2)
	-- Loop through the last 7 days
	
	WHILE @CurrentDate <= @Today
	BEGIN
		SET @DATENAME = DATENAME(WEEKDAY, @CurrentDate)
		
	   SELECT @Sold = SUM(ISNULL(commission,0)) FROM Transactions AS TR
	   INNER JOIN Properties AS P ON P.property_id = TR.property_id  
	   WHERE CONVERT(DATE,TR.transaction_date) = CONVERT(DATE,@CurrentDate) AND P.status = 'Sold'
	   GROUP BY TR.transaction_date, P.status

	   SELECT @Rented = SUM(ISNULL(commission,0)) FROM Transactions AS TR
	   INNER JOIN Properties AS P ON P.property_id = TR.property_id  
	   WHERE CONVERT(DATE,TR.transaction_date) = CONVERT(DATE,@CurrentDate) AND P.status = 'Rented'
	   GROUP BY TR.transaction_date, P.status

	   INSERT INTO #WeeklyAct (Name, Sold, Rented)
	   VALUES (@DATENAME, ISNULL(@Sold,0), ISNULL(@Rented,0))
	  
		SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);  -- Move to the next day
	END
	SELECT * from #WeeklyAct
	Drop Table #WeeklyAct
	---------------------------------------------------------------------------------------
	
END

GO


