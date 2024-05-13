CREATE TABLE [User] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [Username] VARCHAR(255) NOT NULL,
  [Password] VARCHAR(255) NOT NULL,
  [Email] VARCHAR(255) NOT NULL,
  [Avatar] VARCHAR(255) NOT NULL,
  [Gender] BIT
)

CREATE TABLE [Role] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [RoleName] VARCHAR(255)
  --admin, teacher, student
)

CREATE TABLE [UserRole] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [UserID] integer,
  [RoleID] integer
)

CREATE TABLE [Courses] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [CourseName] VARCHAR(255) NOT NULL,
  [TeacherID] integer,
  [LinkCourse] varchar(255)
)

CREATE TABLE [RegisteredCourse] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [UserID] int,
  [CourseID] integer,
  [Rating] float,
  [ContentRating] text,
  [Save] bit,
  [CreatedAt] time,
  [CreatedBy] time,
  [UpdatedAt] time,
  [UpdatedBy] time
)

CREATE TABLE [Session] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [SessionName] VARCHAR(255) NOT NULL,
  [Order] integer,
  [CourseID] integer
)
select * from [RatingCourses]

CREATE TABLE [Status] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [StatusName] nvarchar(255)
  --Completd, inprogress, pending
)

CREATE TABLE [StudentSession] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [UserID] int,
  [SessionID] int,
  [CreatedAt] datetime,
  [StatusID] int,
  [Progress] float
)

CREATE TABLE [SessionContent] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [Video] varchar(255),
  [VideoDuration] time,
  [SessionID] int,
  [Summary] text,
  [Document] VARCHAR(255) NOT NULL,
  [Trascript] text
)

CREATE TABLE [Discussion] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [SessionID] int,
  [UserID] int,
  [Content] text NOT NULL,
  [AttachedFile] VARCHAR(255),
  [CreatedAt] datetime
)

CREATE TABLE [Reply] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [DiscussionID] int,
  [UserID] int,
  [Content] text  NOT NULL,
  [AttachedFile] VARCHAR(255),
  [CreatedAt] datetime,
  [ParentReplyID] int
)

CREATE TABLE [LikeComment] (
  [ID] INT IDENTITY(1,1) PRIMARY KEY,
  [UserID] int,
  [CommentID] int
)

-- Name Course
SELECT c.CourseName FROM Courses c
JOIN RegisteredCourse rc ON rc.CourseID = c.ID
JOIN [User] u ON (u.ID = rc.UserID)
WHERE c.ID = 1 AND u.ID = 2

-- Name and avatar Teacher
SELECT username, Avatar FROM [User]
JOIN Courses c ON c.TeacherID = [User].ID
WHERE c.ID = 1

-- Average votes of the course - Total number of votes
SELECT AVG(Rating) AvgRating, COUNT(Rating) Review 
FROM RegisteredCourse
WHERE CourseID = 1

-- Share Course
SELECT LinkCourse FROM Courses 
WHERE ID = 1

-- Save Course
UPDATE [RegisteredCourse]
SET [Save] = '1'
WHERE UserID = 2 AND CourseID = 1;

-- Show Video of Session
SELECT s.[Order], sc.Video 
FROM [SessionContent] sc
JOIN [Session] s ON s.ID = sc.SessionID
JOIN [StudentSession] ss ON ss.SessionID = s.ID
WHERE ss.UserID = 2 AND s.ID = 2

-- Show Progress of Session
SELECT CONCAT(COUNT(CASE WHEN StatusID = 1 THEN 1 END), 
	   '/', COUNT(SessionID)) AS Progress
FROM [StudentSession]
WHERE UserID = 2;

-- Sessions of Course
SELECT s.[Order], s.[SessionName], st.StatusName
FROM [Session] s
JOIN StudentSession ss ON ss.SessionID = s.ID
JOIN [Status] st ON st.ID = ss.StatusID
WHERE ss.UserID = 2 AND s.CourseID = 1 
ORDER BY s.[Order]


-- Session Contents
SELECT sc.Summary, sc.Document, sc.Trascript 
FROM SessionContent sc
JOIN Session s ON s.ID = sc.SessionID
WHERE s.ID = 1

-- Discussions and Reply Decussions
SELECT u.Avatar, u.Username, Content, AttachedFile, CreatedAt FROM Discussion
JOIN [User] u ON u.ID = Discussion.UserID
WHERE Discussion.ID BETWEEN 1 AND 2
ORDER BY CreatedAt DESC

SELECT u.Avatar, u.Username, r.Content, r.AttachedFile, r.CreatedAt FROM Reply r
JOIN Discussion d ON d.ID = r.ID
JOIN [User] u ON u.ID = r.UserID
WHERE (d.ID BETWEEN 1 AND 2) AND (r.ID BETWEEN 1 AND 2)
ORDER BY d.CreatedAt DESC

-- Show all Discussions and Reply Decussions
SELECT u.Avatar, u.Username, Content, AttachedFile, CreatedAt FROM Discussion
JOIN [User] u ON u.ID = Discussion.UserID
ORDER BY CreatedAt DESC

SELECT u.Avatar, u.Username, r.Content, r.AttachedFile, r.CreatedAt FROM Reply r
JOIN Discussion d ON d.ID = r.ID
JOIN [User] u ON u.ID = r.UserID
ORDER BY d.CreatedAt DESC

-- Add CommentSession
INSERT INTO [Discussion] (SessionID, UserID, 
			Content, AttachedFile, CreatedAt)
VALUES (3, 2, 'Can anyone explain?', 
	   '1.png', GETUTCDATE())

-- Like Comment
INSERT INTO [LikeComment] (CommentID, UserID)
VALUES (3, 2)

-- Add CommentReply
INSERT INTO [Reply] (DiscussionID, UserID, Content, 
					AttachedFile, CreatedAt, ParentReplyID)
VALUES (4, 3, 'You so funny!', 
	   NULL, GETUTCDATE(), NULL)