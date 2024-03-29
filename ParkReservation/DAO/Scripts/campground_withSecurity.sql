USE master;
GO

DROP DATABASE IF EXISTS npcampground;
GO

CREATE DATABASE npcampground;
GO

USE npcampground;
GO

/****** Object:  Table [dbo].[campground]    Script Date: 6/19/2019 3:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[campground](
	[campground_id] [int] IDENTITY(1,1) NOT NULL,
	[park_id] [int] NOT NULL,
	[name] [varchar](80) NOT NULL,
	[open_from_mm] [int] NOT NULL,
	[open_to_mm] [int] NOT NULL,
	[daily_fee] [money] NOT NULL,
 CONSTRAINT [pk_campground_campground_id] PRIMARY KEY CLUSTERED 
(
	[campground_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[park]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[park](
	[park_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NOT NULL,
	[location] [varchar](50) NOT NULL,
	[establish_date] [date] NOT NULL,
	[area] [int] NOT NULL,
	[visitors] [int] NOT NULL,
	[description] [varchar](500) NOT NULL,
 CONSTRAINT [pk_park_park_id] PRIMARY KEY CLUSTERED 
(
	[park_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[reservation]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reservation](
	[reservation_id] [int] IDENTITY(1,1) NOT NULL,
	[site_id] [int] NOT NULL,
	[name] [varchar](80) NOT NULL,
	[from_date] [date] NOT NULL,
	[to_date] [date] NOT NULL,
	[create_date] [datetime] NULL,
 CONSTRAINT [pk_reservation_reservation_id] PRIMARY KEY CLUSTERED 
(
	[reservation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleItem]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleItem](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[site]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site](
	[site_id] [int] IDENTITY(1,1) NOT NULL,
	[campground_id] [int] NOT NULL,
	[site_number] [int] NOT NULL,
	[max_occupancy] [int] NOT NULL,
	[accessible] [bit] NOT NULL,
	[max_rv_length] [int] NOT NULL,
	[utilities] [bit] NOT NULL,
 CONSTRAINT [pk_site_site_number_campground_id] PRIMARY KEY CLUSTERED 
(
	[site_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VendUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserReservation]    Script Date: 6/19/2019 3:38:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserReservation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[reservation_id] [int] NOT NULL,
 CONSTRAINT [PK_UserReservation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[reservation] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((6)) FOR [max_occupancy]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [accessible]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [max_rv_length]
GO
ALTER TABLE [dbo].[site] ADD  DEFAULT ((0)) FOR [utilities]
GO
ALTER TABLE [dbo].[campground]  WITH CHECK ADD FOREIGN KEY([park_id])
REFERENCES [dbo].[park] ([park_id])
GO
ALTER TABLE [dbo].[reservation]  WITH CHECK ADD FOREIGN KEY([site_id])
REFERENCES [dbo].[site] ([site_id])
GO
ALTER TABLE [dbo].[site]  WITH CHECK ADD FOREIGN KEY([campground_id])
REFERENCES [dbo].[campground] ([campground_id])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserReservation]  WITH CHECK ADD  CONSTRAINT [FK_UserReservation_Reservation] FOREIGN KEY([reservation_id])
REFERENCES [dbo].[reservation] ([reservation_id])
GO
ALTER TABLE [dbo].[UserReservation] CHECK CONSTRAINT [FK_UserReservation_Reservation]
GO
ALTER TABLE [dbo].[UserReservation]  WITH CHECK ADD  CONSTRAINT [FK_UserReservation_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserReservation] CHECK CONSTRAINT [FK_UserReservation_User]
GO

Begin Transaction;

-- Acadia
INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Acadia', 'Maine', '1919-02-26', 47389, 2563129, 'Covering most of Mount Desert Island and other coastal islands, Acadia features the tallest mountain on the Atlantic coast of the United States, granite peaks, ocean shoreline, woodlands, and lakes. There are freshwater, estuary, forest, and intertidal habitats.');

-- Arches
INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Arches',	'Utah', '1929-04-12', 76518,	1284767, 'This site features more than 2,000 natural sandstone arches, including the famous Delicate Arch. In a desert climate, millions of years of erosion have led to these structures, and the arid ground has life-sustaining soil crust and potholes, which serve as natural water-collecting basins. Other geologic formations are stone columns, spires, fins, and towers.');

-- Cuyahoga
INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('Cuyahoga Valley', 'Ohio', '2000-10-11',32860,	2189849, 'This park along the Cuyahoga River has waterfalls, hills, trails, and exhibits on early rural living. The Ohio and Erie Canal Towpath Trail follows the Ohio and Erie Canal, where mules towed canal boats. The park has numerous historic homes, bridges, and structures, and also offers a scenic train ride.');

-- Acadia Campgrounds
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 'Blackwoods', 1, 12, 35.00);
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 'Seawall', 5, 9, 30.00);
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (1, 'Schoodic Woods', 5, 10, 30.00);

-- Arches Campgrounds
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (2, 'Devil''s Garden', 1, 12, 25.00);
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (2, 'Canyon Wren Group Site', 1, 12, 160.00);
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (2, 'Juniper Group Site', 1, 12, 250.00);

-- Cuyahoga Campgrounds
INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (3, 'The Unnamed Primitive Campsites', 5, 11, 20.00);

-- Acadia Blackwoods Sites (Tent, Camper)
INSERT INTO site (site_number, campground_id) VALUES (1, 1);
INSERT INTO site (site_number, campground_id) VALUES (2, 1);
INSERT INTO site (site_number, campground_id) VALUES (3, 1);
INSERT INTO site (site_number, campground_id, utilities) VALUES (4, 1, 1);
INSERT INTO site (site_number, campground_id, accessible) VALUES (5, 1, 1);
INSERT INTO site (site_number, campground_id, accessible, utilities) VALUES (6, 1, 1, 1);
-- Acadia Blackwoods Sites (RV/Trainler 20ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (7, 1, 20);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (8, 1, 20, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (9, 1, 20, 1, 1);
-- Acadia Blackwoods Sites (RV/Trainler 35ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (10, 1, 35);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (11, 1, 35, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (12, 1, 35, 1, 1);

-- Acadia Seawall Sites (Tent)
INSERT INTO site (site_number, campground_id) VALUES (1, 2);
INSERT INTO site (site_number, campground_id) VALUES (2, 2);
INSERT INTO site (site_number, campground_id, utilities) VALUES (3, 2, 1);
INSERT INTO site (site_number, campground_id, accessible) VALUES (4, 2, 1);
INSERT INTO site (site_number, campground_id, accessible) VALUES (5, 2, 1);
INSERT INTO site (site_number, campground_id, accessible, utilities) VALUES (6, 2, 1, 1);
-- Acadia Seawall Sites (RV/Trailer 20ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (7, 2, 20);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (8, 2, 20, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (9, 2, 20, 1, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (10, 2, 20, 1, 1);
-- Acadia Seawall Sites (RV/Trailer 35ft)
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (11, 2, 35, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (12, 2, 35, 1, 1);

-- Acadia Schoodic Sites (Tent and Camper)
INSERT INTO site (site_number, campground_id) VALUES (1, 3);
INSERT INTO site (site_number, campground_id) VALUES (2, 3);
INSERT INTO site (site_number, campground_id, utilities) VALUES (3, 3, 1); -- 31 - 35
INSERT INTO site (site_number, campground_id, accessible, utilities) VALUES (4, 3, 1, 1);
-- Acadia Schoodic Sites (RV/Trailer 20ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (5, 3, 20);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (6, 3, 20, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (7, 3, 20, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (8, 3, 20, 1, 1); -- 21 - 22
-- Acadia Schoodic Sites (RV/Trailer 35ft)
INSERT INTO site (site_number, campground_id, max_rv_length) VALUES (9, 3, 35); -- 1 - 10
INSERT INTO site (site_number, campground_id, max_rv_length, utilities) VALUES (10, 3, 35, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible) VALUES (11, 3, 35, 1);
INSERT INTO site (site_number, campground_id, max_rv_length, accessible, utilities) VALUES (12, 3, 35, 1, 1);

-- Arches Devil's Garden Sites (Tent)
INSERT INTO site (site_number, campground_id, max_occupancy) VALUES (1, 4, 10);
INSERT INTO site (site_number, campground_id, max_occupancy) VALUES (2, 4, 10);
INSERT INTO site (site_number, campground_id, max_occupancy, utilities) VALUES (3, 4, 10, 1);
INSERT INTO site (site_number, campground_id, max_occupancy, utilities) VALUES (4, 4, 10, 1);
INSERT INTO site (site_number, campground_id, max_occupancy, accessible, utilities) VALUES (5, 4, 10, 1, 1);
INSERT INTO site (site_number, campground_id, max_occupancy, accessible, utilities) VALUES (6, 4, 10, 1, 1);
-- Arches Devil's Garden Sites (RV/Trailer 20ft)
INSERT INTO site (site_number, campground_id, max_occupancy, max_rv_length, utilities) VALUES (7, 4, 7, 20, 1);
INSERT INTO site (site_number, campground_id, max_occupancy, max_rv_length, utilities) VALUES (8, 4, 7, 20, 1);
 -- Arches Canyon Wren Group Site (Tent)
INSERT INTO site (site_number, campground_id, max_occupancy, accessible) VALUES (1, 5, 35, 1);
-- Arches Juniper Group Site (Tent)
INSERT INTO site (site_number, campground_id, max_occupancy, accessible) VALUES (1, 6, 55, 1);

-- Cuyahoga Unnamed Primitive Campsites Sites (Tent)
INSERT INTO site (site_number, campground_id) VALUES (1, 7);
INSERT INTO site (site_number, campground_id) VALUES (2, 7);
INSERT INTO site (site_number, campground_id) VALUES (3, 7);
INSERT INTO site (site_number, campground_id) VALUES (4, 7);
INSERT INTO site (site_number, campground_id) VALUES (5, 7);



-- Reservations
-- Acadia Blackwoods Sites (1 - 12)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (1, 'Smith Family Reservation', GETDATE()-2, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (1, 'Lockhart Family Reservation', GETDATE()-6, GETDATE()-3);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (2, 'Jones Reservation', GETDATE()-2, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (3, 'Bauer Family Reservation', GETDATE(), GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (4, 'Eagles Family Reservation', GETDATE()+5, GETDATE()+10);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (7, 'Aersomith Reservation', GETDATE()-3, GETDATE()+2);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (9, 'Claus Family Reservation', GETDATE(), GETDATE()+1);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (9, 'Taylor Family Reservation', GETDATE()-7, GETDATE()-5);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (10, 'Astley Family Reservation', GETDATE()+14, GETDATE()+21);

-- Acadia Seawall Sites (13 - 24)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (13, 'Jobs Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (14, 'Cook Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (15, 'Gates Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (16, 'Cue Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (17, 'Ive Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (18, 'Federighi Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (19, 'Schiller Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (20, 'Williams Family Reservation', GETDATE()+1, GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (20, 'Kawasaki Family Reservation', GETDATE()+10, GETDATE()+21);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (20, 'Branson Family Reservation', GETDATE()+22, GETDATE()+28);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (20, 'Zukerberg Family Reservation', GETDATE()+30, GETDATE()+33);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (24, 'Musk Family Reservation', GETDATE()+4, GETDATE()+10);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (24, 'Buffett Family Reservation', GETDATE()-4, GETDATE());

-- Acadia Schoodic Woods Sites (25 - 36)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (25, 'Satya Nedella', GETDATE()+3, GETDATE()+10);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (25, 'Scott Gutherie', GETDATE()+10, GETDATE()+14);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (28, 'Amy Hood', GETDATE()+5, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (29, 'Peggy Johnson', GETDATE()+5, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (31, 'Terry Myerson', GETDATE()+9, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (32, 'Steve Ballmer', GETDATE()+13, GETDATE()+15);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (32, 'Gates Family Reservation', GETDATE()+16, GETDATE()+19);


-- Devil's Garden (37 - 44)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (37, 'Marisa Mayer', GETDATE()-15, GETDATE()-10);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (37, 'Beth Mooney', GETDATE(), GETDATE()+4);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (40, 'William Priemer', GETDATE()+2, GETDATE()+6);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (42, 'Tricia Griffith', GETDATE()+3, GETDATE()+8);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (43, 'Toby Cosgrove', GETDATE()+5, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (43, 'Akram Boutros', GETDATE()+12, GETDATE()+18);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (44, 'Barbara Snyder', GETDATE()+9, GETDATE()+11);


-- Canyon Wren (45)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (45, 'Bill Board', GETDATE()-9, GETDATE()+1);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (45, 'Bill Loney', GETDATE()+1, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (45, 'Cara Van', GETDATE()+17, GETDATE()+21);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (45, 'Forrest Gump', GETDATE()+31, GETDATE()+37);

-- Juniper Group Site (46)
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (46, 'Simpson Family', GETDATE()-6, GETDATE()+1);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (46, 'Smith Family', GETDATE()+2, GETDATE()+11);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (46, 'Leela Family', GETDATE()+14, GETDATE()+15);
INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (46, 'Scott Family', GETDATE()+35, GETDATE()+40);


-- Unnamed Primitive Campsites (47 - 51)
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (1, N'Administrator')
GO
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (2, N'StandardUser')
GO

commit transaction;
