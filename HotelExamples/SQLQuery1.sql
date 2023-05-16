CREATE TABLE PaymentMethods(
	 P_id   int IDENTITY(1,1)   NOT NULL,
     P_method   VARCHAR(50)  NOT NULL,
 	 Primary KEY (P_id)
);

CREATE TABLE Hotel_PaymentsMethods(
    P_id   int  NOT NULL,
    Hotel_No  int  NOT NULL,
    FOREIGN KEY (P_id) REFERENCES PaymentMethods (P_id),
    FOREIGN KEY (Hotel_No) REFERENCES Hotel (Hotel_No),
	Primary KEY (P_id, Hotel_No)
);


Create TABLE Hotel(
     Hotel_No  int  NOT NULL PRIMARY KEY,
     Name      VARCHAR(30)     NOT NULL,
     Address   VARCHAR(50)  NOT NULL, 
     VIP  BIT  NULL, 
     HotelType int NOT NULL,
     HotelImage VARCHAR(60) Null
);
