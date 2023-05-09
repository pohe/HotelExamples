CREATE TABLE [dbo].[Hotel] (
    [Hotel_No]   INT          NOT NULL,
    [Name]       VARCHAR (30) NOT NULL,
    [Address]    VARCHAR (50) NOT NULL,
    [VIP]        BIT          NULL,
    [HotelImage] VARCHAR (60) NULL,
	[HotelType] int NOT NULL,
    PRIMARY KEY CLUSTERED ([Hotel_No] ASC)
);

