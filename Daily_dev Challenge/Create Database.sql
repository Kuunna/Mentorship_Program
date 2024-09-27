CREATE TABLE [Fact_News] (
  [NewID] INT PRIMARY KEY,
  [Title] NVARCHAR(255),
  [Content] TEXT,
  [PublishedDate] INT,
  [SourceID] INT,
  [TopicID] INT,
  [Author] NVARCHAR(100),
  [ImageURL] NVARCHAR(255),
  [ViewCount] INT,
  [LikeCount] INT,
  [CommentCount] INT
)
GO

CREATE TABLE [Dim_Source] (
  [SourceID] INT PRIMARY KEY,
  [SourceName] NVARCHAR(255),
  [RSS_URL] NVARCHAR(500),
  [LastUpdated] INT,
  [ArticleCount] INT
)
GO

CREATE TABLE [Dim_Tag] (
  [TagID] INT PRIMARY KEY,
  [TagName] NVARCHAR(255),
  [TagDescription] TEXT,
  [TagCount] INT
)
GO

CREATE TABLE [Dim_User] (
  [UserID] INT PRIMARY KEY,
  [UserName] NVARCHAR(255),
  [Email] NVARCHAR(255),
  [Password] NVARCHAR(255),
  [JoinDate] INT,
  [LastLogin] INT,
  [Preferences] TEXT,
  [Role] NVARCHAR(50)
)
GO

CREATE TABLE [Dim_Time] (
  [TimeID] INT PRIMARY KEY,
  [Date] DATE,
  [Year] INT,
  [Month] INT,
  [Day] INT,
  [Week] INT,
  [Quarter] INT
)
GO

CREATE TABLE [Fact_Article_Interaction] (
  [InteractionID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [InteractionType] NVARCHAR(50),
  [InteractionDate] INT,
  [CommentText] TEXT,
  [UpvoteCount] INT
)
GO

CREATE TABLE [Fact_Comments] (
  [CommentID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [CommentText] TEXT,
  [CommentDate] INT
)
GO

CREATE TABLE [Fact_Bookmark] (
  [BookmarkID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [BookmarkDate] INT
)
GO

CREATE TABLE [Fact_History] (
  [HistoryID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [ReadDate] INT,
  [ReadDuration] INT
)
GO

CREATE TABLE [Dim_Category] (
  [CategoryID] INT PRIMARY KEY,
  [CategoryName] NVARCHAR(255),
  [CategoryDescription] TEXT
)
GO

CREATE TABLE [News_Tag] (
  [NewsID] INT,
  [TagID] INT
)
GO

CREATE TABLE [User_Source] (
  [UserID] INT,
  [SourceID] INT,
  [FollowDate] INT
)
GO

CREATE TABLE [User_Tag] (
  [UserID] INT,
  [TagID] INT,
  [InterestLevel] NVARCHAR(50),
  [FollowDate] INT
)
GO

ALTER TABLE [Fact_News] ADD FOREIGN KEY ([PublishedDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Fact_News] ADD FOREIGN KEY ([SourceID]) REFERENCES [Dim_Source] ([SourceID])
GO

ALTER TABLE [Fact_News] ADD FOREIGN KEY ([TopicID]) REFERENCES [Dim_Category] ([CategoryID])
GO

ALTER TABLE [Dim_Source] ADD FOREIGN KEY ([LastUpdated]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Dim_User] ADD FOREIGN KEY ([JoinDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Dim_User] ADD FOREIGN KEY ([LastLogin]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Fact_Article_Interaction] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [Fact_Article_Interaction] ADD FOREIGN KEY ([NewsID]) REFERENCES [Fact_News] ([NewID])
GO

ALTER TABLE [Fact_Article_Interaction] ADD FOREIGN KEY ([InteractionDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Fact_Comments] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [Fact_Comments] ADD FOREIGN KEY ([NewsID]) REFERENCES [Fact_News] ([NewID])
GO

ALTER TABLE [Fact_Comments] ADD FOREIGN KEY ([CommentDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Fact_Bookmark] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [Fact_Bookmark] ADD FOREIGN KEY ([NewsID]) REFERENCES [Fact_News] ([NewID])
GO

ALTER TABLE [Fact_Bookmark] ADD FOREIGN KEY ([BookmarkDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [Fact_History] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [Fact_History] ADD FOREIGN KEY ([NewsID]) REFERENCES [Fact_News] ([NewID])
GO

ALTER TABLE [Fact_History] ADD FOREIGN KEY ([ReadDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [News_Tag] ADD FOREIGN KEY ([NewsID]) REFERENCES [Fact_News] ([NewID])
GO

ALTER TABLE [News_Tag] ADD FOREIGN KEY ([TagID]) REFERENCES [Dim_Tag] ([TagID])
GO

ALTER TABLE [User_Source] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [User_Source] ADD FOREIGN KEY ([SourceID]) REFERENCES [Dim_Source] ([SourceID])
GO

ALTER TABLE [User_Source] ADD FOREIGN KEY ([FollowDate]) REFERENCES [Dim_Time] ([TimeID])
GO

ALTER TABLE [User_Tag] ADD FOREIGN KEY ([UserID]) REFERENCES [Dim_User] ([UserID])
GO

ALTER TABLE [User_Tag] ADD FOREIGN KEY ([TagID]) REFERENCES [Dim_Tag] ([TagID])
GO

ALTER TABLE [User_Tag] ADD FOREIGN KEY ([FollowDate]) REFERENCES [Dim_Time] ([TimeID])
GO
