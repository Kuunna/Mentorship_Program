CREATE TABLE [User] (
  ID int,
  Name nvarchar(50) NOT NULL, -- Name
  Gender nvarchar(10) NOT NULL, -- Giới tính
  DateOfBirth date NOT NULL, -- Ngày Sinh
  Email nvarchar(50) NOT NULL
)

CREATE TABLE Student (
  ID int PRIMARY KEY,
  UserID int
);

CREATE TABLE Teacher (
  ID int PRIMARY KEY,
  UserID int
);

CREATE TABLE Course (
  ID int PRIMARY KEY, 
  CourseName nvarchar(50) NOT NULL, 
  TeacherID int NOT NULL
);
CREATE TABLE StudentCourse (
  ID int PRIMARY KEY,
  StudentID int NOT NULL,
  CourseID int NOT NULL,
  Score float, -- Điểm
  Registered nvarchar(50), -- Ngày đăng ký
  ActiveAt nvarchar(50), 
  Graduate nvarchar(50) -- Ngày tốt nghiệp
);


------------------------------------------------------------------------------------------------------------------------------------------------------------
-- INSERT User
INSERT INTO [User] (ID, Name, Gender, DateOfBirth, Email)
VALUES
  (1, 'John Smith', 'Male', '2000-01-01', 'john@example.com'),
  (2, 'Jane Doe', 'Female', '2001-02-03', 'jane@example.com'),
  (3, 'Michael Johnson', 'Male', '1999-05-15', 'michael@example.com'),
  (4, 'Emily Williams', 'Female', '1998-08-27', 'emily@example.com'),
  (5, 'Daniel Brown', 'Male', '2002-03-10', 'daniel@example.com'),
  (6, 'Sophia Taylor', 'Female', '2003-07-22', 'sophia@example.com'),
  (7, 'James Wilson', 'Male', '1997-11-14', 'james@example.com'),
  (8, 'Olivia Davis', 'Female', '1999-09-02', 'olivia@example.com'),
  (9, 'Alexander Martinez', 'Male', '2000-04-18', 'alexander@example.com'),
  (10, 'Isabella Anderson', 'Female', '2001-06-05', 'isabella@example.com');
-- INSERT Student
INSERT INTO Student (ID, UserID)
VALUES (1, 1), (2, 2), (3, 3), (4, 4), (5, 5), 
	   (6, 6), (7, 7), (8, 8), (9, 9), (10, 10);
-- INSERT Teacher
INSERT INTO Teacher (ID, UserID)
VALUES (1, 6), (2, 7), (3, 8), (4, 9), (5, 10)
-- INSERT Course
INSERT INTO Course (ID, CourseName, TeacherID)
VALUES
	(1, 'OOP', 1),
	(2, 'Database', 2),
	(3, 'Web Development', 3),
	(4, 'Algorithms', 4),
	(5, 'OOP', 5),
	(6, 'OOP', 2),
	(7, 'Operating Systems', 1),
	(8, 'UI/UX', 2),
	(9, 'Network Security', 1),
	(10, 'Software Engineering', 3),
	(11, 'HTML/CSS', 2),
	(12, 'Data Structures', 4);
-- INSERT StudentCourse
INSERT INTO StudentCourse (ID, StudentID, CourseID, Score, Registered, Graduate)
VALUES
(1, 1, 1, 4.5, '2022-01-01', '2022-06-30'),
(2, 2, 1, 8, '2022-02-03', '2022-07-31'),
(3, 3, 2, 9, '2022-03-05', '2022-08-31'),
(4, 4, 2, 6.2, '2022-04-07', '2022-09-30'),
(5, 5, 3, 7.8, '2022-05-09', '2022-10-31'),
(6, 6, 3, 5.3, '2022-06-11', '2022-11-30'),
(7, 7, 4, 9.6, '2022-07-13', '2022-12-31'),
(8, 8, 4, 4.9, '2022-08-15', '2023-01-31'),
(9, 9, 5, 8.2, '2022-09-17', '2023-02-28'),
(10, 10, 5, 6.5, '2022-10-19', '2023-03-31'),
(11, 1, 6, 5.6, '2022-11-21', '2023-04-30'),
(12, 2, 6, 7.2, '2022-12-23', '2023-05-31'),
(13, 3, 7, 8.8, '2023-01-25', '2023-06-30'),
(14, 4, 7, 6.9, '2023-02-27', '2023-07-31'),
(15, 5, 8, 4.4, '2023-03-29', '2023-08-31'),
(16, 6, 8, 9.3, '2023-04-30', '2023-09-30'),
(17, 7, 9, 5.7, '2023-06-01', '2023-10-31'),
(18, 8, 9, 7.1, '2023-07-03', '2023-11-30'),
(19, 9, 10, 8.6, '2023-08-05', '2023-12-31'),
(20, 10, 11, 6.7, '2023-09-06', '2024-01-31'),
(21, 1, 11, 4.8, '2023-10-08', '2024-02-29'),
(22, 2, 12, 9.1, '2023-11-10', '2024-03-31'),
(23, 3, 12, 7.4, '2023-12-12', '2024-04-30'),
(24, 4, 1, 6.3, '2024-01-14', '2024-05-31'),
(25, 5, 2, 8.9, '2024-02-15', '2024-06-30'),
(26, 6, 3, 5.2, '2024-03-17', '2024-07-31'),
(27, 7, 4, 6.6, '2024-04-18', '2024-08-31'),
(28, 8, 5, 7.7, '2024-05-20', '2024-09-30'),
(29, 9, 6, 4.7, '2024-06-22', '2024-10-31'),
(30, 10, 7, 9.4, '2024-07-24', '2024-11-30')

------------------------------------------------------------------------------------------------------------------------------------------------------------
--danh sách khóa học ,có tổng số sinh viên giỏi, khá cao nhất
SELECT c.CourseName AS CourseName,
       SUM(CASE
               WHEN sc.Score >= 8 THEN 1
               ELSE 0
           END) 
		+ 
		SUM(CASE
				WHEN (sc.Score >= 6.5
					AND sc.Score < 8) THEN 1
				ELSE 0
			END) AS TotalExcellentGood
FROM StudentCourse sc
JOIN Course c ON sc.CourseID = c.ID
GROUP BY c.CourseName
ORDER BY TotalExcellentGood DESC;

-- Thống kê khóa học có bao nhiêu sinh viên giỏi, khá, yếu
SELECT c.CourseName AS CourseName,
       SUM(CASE
               WHEN sc.Score >= 8 
			   THEN 1 ELSE 0
           END) AS Excellence,
       SUM(CASE
               WHEN (sc.Score >= 6.5
                     AND sc.Score < 8) 
			   THEN 1 ELSE 0
           END) AS Good,
       SUM(CASE
               WHEN Score < 6.5 
			   THEN 1 ELSE 0
           END) AS Weak
FROM StudentCourse sc
JOIN Course c ON sc.CourseID = c.ID
GROUP BY CourseName
ORDER BY CourseName;

-- danh sách khóa học có giáo viên phụ trách là nữ dạy môn OOP có tổng điểm trung bình cao nhất
SELECT c.CourseName AS CourseName,
       u.Name AS 'Name Teacher',
       AVG(sc.Score) AS AverageScore
FROM Course c
JOIN Teacher t ON c.TeacherID = t.ID
JOIN [User] u ON t.UserID = u.ID
JOIN StudentCourse sc ON c.ID = sc.CourseID
WHERE u.Gender = 'Female'
  AND c.CourseName = 'OOP'
GROUP BY c.CourseName,
         u.Name
ORDER BY AverageScore DESC;

-- danh sách giáo viên chưa bao giờ dạy OOP (WHERE NOT EXISTS)
SELECT t.ID,
       u.Name AS TeacherName,
	   u.Gender
FROM Teacher t
JOIN [User] u ON t.UserID = u.ID
WHERE NOT EXISTS
    (SELECT 1
     FROM Course c
     WHERE c.TeacherID = t.ID 
		   AND c.CourseName = 'OOP' );

-- danh sách giáo viên chưa bao giờ dạy OOP (LEFT JOIN)
SELECT t.ID as TeacherID,
       u.Name AS TeacherName,
	   u.Gender
FROM Teacher t
JOIN [User] u ON t.UserID = u.ID
LEFT JOIN Course c ON c.TeacherID = t.ID 
		  AND c.CourseName = 'OOP'
WHERE c.ID IS NULL;

-- thống kê tổng số sinh viên đăng ký theo tháng
SELECT YEAR(sc.Registered) AS YEAR,
       MONTH(sc.Registered) AS MONTH,
       COUNT(DISTINCT sc.StudentID) AS TotalStudents
FROM StudentCourse sc
GROUP BY YEAR(sc.Registered),
         MONTH(sc.Registered)
ORDER BY YEAR, MONTH;

-- thống kê tổng số sinh viên đăng ký theo tuần
SELECT YEAR(sc.Registered) AS YEAR,
       DATEPART(WEEK, sc.Registered) AS WEEK,
       COUNT(DISTINCT sc.StudentID) AS TotalStudents
FROM StudentCourse sc
GROUP BY YEAR(sc.Registered),
         DATEPART(WEEK, sc.Registered)
ORDER BY YEAR, WEEK;

-- thống kê tổng số sinh viên đăng ký theo năm
SELECT YEAR(sc.Registered) AS YEAR,
       COUNT(DISTINCT sc.StudentID) 
	   AS TotalStudents
FROM StudentCourse sc
GROUP BY YEAR(sc.Registered)
ORDER BY YEAR;

-- Liệt kê danh sách sinh viên đậu loại giỏi, trung bình, rớt theo từng tháng
SELECT YEAR(sc.Graduate) AS YEAR,
       MONTH(sc.Graduate) AS MONTH,
       u.Name AS StudentName,
       (CASE
           WHEN sc.Score >= 8.5 THEN 'Excellence'  
           WHEN (sc.Score >= 6.5 AND sc.Score < 8) THEN 'Good'
		   WHEN (sc.Score >= 0 AND sc.Score < 6.5) THEN 'week'
           ELSE 'Not graduated'
       END) AS RESULT
FROM StudentCourse sc
JOIN Student s ON sc.StudentID = s.ID
JOIN [User] u ON u.ID = s.UserID
WHERE sc.Graduate IS NOT NULL
GROUP BY MONTH(sc.Graduate),
         YEAR(sc.Graduate),
         u.Name,
		 sc.Score
ORDER BY MONTH, YEAR, StudentName;

-- Danh sách month active students
SELECT
    MONTH(sc.ActiveAt) AS Month,
    COUNT(DISTINCT s.ID) AS ActiveStudent
FROM
    StudentCourse sc
JOIN
    Student s ON sc.StudentID = s.ID
WHERE
    sc.ActiveAt IS NOT NULL
GROUP BY
    MONTH(sc.ActiveAt)
ORDER BY
    Month;