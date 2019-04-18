CREATE TABLE [dbo].[MessageLogging]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [timestamps] TIMESTAMP NULL, 
    [actionID] NVARCHAR(MAX) NULL, 
    [owner] NVARCHAR(MAX) NULL, 
    [message] NVARCHAR(MAX) NULL,
	UserID int FOREIGN KEY REFERENCES Users(Id)
)
