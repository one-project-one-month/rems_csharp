USE [REMS]
GO
/****** Object:  Table [dbo].[Agents]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agents](
	[agent_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[agency_name] [nvarchar](100) NOT NULL,
	[license_number] [nvarchar](50) NOT NULL,
	[phone] [nvarchar](15) NULL,
	[email] [nvarchar](100) NULL,
	[address] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[agent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[appointment_id] [int] IDENTITY(1,1) NOT NULL,
	[agent_id] [int] NULL,
	[client_id] [int] NULL,
	[property_id] [int] NULL,
	[appointment_date] [date] NOT NULL,
	[appointment_time] [time](7) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[notes] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[appointment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[client_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[agent_id] [int] NULL,
	[first_name] [nvarchar](100) NOT NULL,
	[last_name] [nvarchar](100) NOT NULL,
	[phone] [nvarchar](15) NULL,
	[email] [nvarchar](100) NULL,
	[address] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[client_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listings]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listings](
	[listing_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[agent_id] [int] NULL,
	[date_listed] [datetime] NULL,
	[listing_price] [decimal](18, 2) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[listing_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Properties]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Properties](
	[property_id] [int] IDENTITY(1,1) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[city] [nvarchar](100) NOT NULL,
	[state] [nvarchar](50) NOT NULL,
	[zip_code] [nvarchar](10) NOT NULL,
	[property_type] [nvarchar](50) NOT NULL,
	[price] [decimal](18, 2) NOT NULL,
	[size] [decimal](18, 2) NOT NULL,
	[number_of_bedrooms] [int] NULL,
	[number_of_bathrooms] [int] NULL,
	[year_built] [int] NULL,
	[description] [nvarchar](max) NULL,
	[status] [nvarchar](50) NOT NULL,
	[date_listed] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[property_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyImages]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyImages](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[image_url] [nvarchar](200) NOT NULL,
	[description] [nvarchar](max) NULL,
	[date_uploaded] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[review_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[property_id] [int] NULL,
	[rating] [int] NOT NULL,
	[comments] [nvarchar](max) NULL,
	[date_created] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[review_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[property_id] [int] NULL,
	[buyer_id] [int] NULL,
	[seller_id] [int] NULL,
	[agent_id] [int] NULL,
	[transaction_date] [datetime] NULL,
	[sale_price] [decimal](18, 2) NOT NULL,
	[commission] [decimal](18, 2) NULL,
	[status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/8/2024 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[email] [nvarchar](100) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[phone] [nvarchar](15) NULL,
	[role] [nvarchar](50) NOT NULL,
	[date_created] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__AB6E6164A21008DA]    Script Date: 7/8/2024 8:51:44 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Listings] ADD  DEFAULT (getdate()) FOR [date_listed]
GO
ALTER TABLE [dbo].[Properties] ADD  DEFAULT (getdate()) FOR [date_listed]
GO
ALTER TABLE [dbo].[PropertyImages] ADD  DEFAULT (getdate()) FOR [date_uploaded]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [transaction_date]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[Agents]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([agent_id])
REFERENCES [dbo].[Agents] ([agent_id])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([client_id])
REFERENCES [dbo].[Clients] ([client_id])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([property_id])
REFERENCES [dbo].[Properties] ([property_id])
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD FOREIGN KEY([agent_id])
REFERENCES [dbo].[Agents] ([agent_id])
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Listings]  WITH CHECK ADD FOREIGN KEY([agent_id])
REFERENCES [dbo].[Agents] ([agent_id])
GO
ALTER TABLE [dbo].[Listings]  WITH CHECK ADD FOREIGN KEY([property_id])
REFERENCES [dbo].[Properties] ([property_id])
GO
ALTER TABLE [dbo].[PropertyImages]  WITH CHECK ADD FOREIGN KEY([property_id])
REFERENCES [dbo].[Properties] ([property_id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([property_id])
REFERENCES [dbo].[Properties] ([property_id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([agent_id])
REFERENCES [dbo].[Agents] ([agent_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([buyer_id])
REFERENCES [dbo].[Clients] ([client_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([property_id])
REFERENCES [dbo].[Properties] ([property_id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([seller_id])
REFERENCES [dbo].[Clients] ([client_id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD CHECK  (([rating]>=(1) AND [rating]<=(5)))
GO
