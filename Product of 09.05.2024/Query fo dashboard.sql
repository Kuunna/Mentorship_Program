CREATE TABLE [Employee] (
  [ID] integer PRIMARY KEY,
  [FirstName] nvarchar(50),
  [LastName] nvarchar(50),
  [Gender] nvarchar(10),
  [Age] int,
  [BusinessUnitID] integer,
  [SeniorityLevelID] int,
  [DepartmentID] int
)

CREATE TABLE [Department] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50)
)

CREATE TABLE [Time] (
  [ID] integer PRIMARY KEY,
  [Year] integer,
  [Quarter] integer,
  [Month] integer,
  [Date] int
)

CREATE TABLE [Seniority] (
  [ID] integer PRIMARY KEY,
  [SeniorityName] nvarchar(50)
)

CREATE TABLE [AgeBanding] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50),
  [AgeRangeFrom] integer,
  [AgeRangeTo] int
)

CREATE TABLE [TenureBanding] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50),
  [TenureFrom] integer,
  [TenureTo] int
)

CREATE TABLE [BusinessUnit] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50)
)

CREATE TABLE [ContractType] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50)
)

CREATE TABLE [SalaryRange] (
  [ID] integer PRIMARY KEY,
  [Name] nvarchar(50),
  [SalaryFrom] integer,
  [SalaryTo] int
)

CREATE TABLE [Headcount] (
  [ID] integer PRIMARY KEY,
  [EmployeeID] integer,
  [DepartmentID] int,
  [TimeID] int,
  [Gender] nvarchar(10),
  [BaseSalary] float,
  [AgeBandingID] int,
  [TenureBandingID] int,
  [ContractTypeID] int,
  [SeniorityLevelID] int,
  [Headcount] int
)

CREATE TABLE [Hiring] (
  [ID] integer PRIMARY KEY,
  [EmployeeID] integer,
  [TimeID] int,
  [Gender] nvarchar(10),
  [ContractTypeID] int,
  [HireDate] date,
  [AgeBandingID] int
)

CREATE TABLE [Leave] (
  [ID] integer PRIMARY KEY,
  [EmployeeID] integer,
  [BusinessUnitID] integer,
  [TimeID] int,
  [Category] nvarchar(50),
  [Days] float
)

CREATE TABLE [Termination] (
  [ID] integer PRIMARY KEY,
  [EmployeeID] integer,
  [Category] nvarchar(50),
  [TimeID] int,
  [SeniorityLevelID] int,
  [TenureBandingID] int
)



--Employees
SELECT 
	--Headcount 
    SUM(h.Headcount) AS Headcount,
	--Headcount by Contract Type
    SUM(CASE WHEN h.ContractTypeID = 1 THEN 1 ELSE 0 END) AS 'Fixed term', 
    SUM(CASE WHEN h.ContractTypeID = 2 THEN 1 ELSE 0 END) AS Regular,
	--Headcount by Seniority
    SUM(CASE WHEN h.SeniorityLevelID = 1 THEN 1 ELSE 0 END) AS Junior,
    SUM(CASE WHEN h.SeniorityLevelID = 2 THEN 1 ELSE 0 END) AS MidLevel,
    SUM(CASE WHEN h.SeniorityLevelID = 3 THEN 1 ELSE 0 END) AS Senior,
	--Average Tenure
    SUM(CASE WHEN h.TenureBandingID = 1 THEN t.TenureFrom
			 WHEN h.TenureBandingID = 2 THEN t.TenureFrom
			 WHEN h.TenureBandingID = 3 THEN t.TenureFrom
			 WHEN h.TenureBandingID = 4 THEN t.TenureFrom
			 WHEN h.TenureBandingID = 5 THEN t.TenureFrom END) 
	/ COUNT(h.TenureBandingID) AS AverageTenure 
FROM Headcount h
JOIN Department d ON h.DepartmentID = d.ID
JOIN TenureBanding t ON t.ID = h.TenureBandingID
JOIN [Time] ON [Time].ID = h.TimeID
WHERE [Time].[Year] = 2024
GROUP BY h.Headcount, h.ContractTypeID, h.SeniorityLevelID



--Diversity 
SELECT
	--Female % of Seniors
    COUNT(CASE WHEN h.Gender = 'Female' THEN 1 END) * 100.0 / 
	SUM(CASE WHEN h.SeniorityLevelID = 3 THEN 1 END) 
	AS 'Female % of Seniors',
	--Headcount by Gender
    COUNT(CASE WHEN h.Gender = 'Female' THEN 1 END) AS Female,
	COUNT(CASE WHEN h.Gender = 'Male' THEN 1 END) AS Male,
	--Headcount by Age Range
    SUM(CASE WHEN h.AgeBandingID = 1 THEN 1 ELSE 0 END) AS '20-',
    SUM(CASE WHEN h.AgeBandingID = 2 THEN 1 ELSE 0 END) AS '20-30',
    SUM(CASE WHEN h.AgeBandingID = 3 THEN 1 ELSE 0 END) AS '30-40',
	SUM(CASE WHEN h.AgeBandingID = 4 THEN 1 ELSE 0 END) AS '40-50',
    SUM(CASE WHEN h.AgeBandingID = 5 THEN 1 ELSE 0 END) AS '50-60',
    SUM(CASE WHEN h.AgeBandingID = 6 THEN 1 ELSE 0 END) AS '60+',
	-- Average Age
    AVG(e.Age) AS 'Average Age'
FROM Headcount h
JOIN ContractType ct ON ct.ID = h.ContractTypeID
JOIN Employee e ON e.ID = h.EmployeeID
JOIN [Time] ON [Time].ID = h.TimeID
WHERE [Time].[Year] = 2024
GROUP BY h.Gender, h.AgeBandingID

--Hiring
SELECT
	-- Hires
    COUNT(*) AS Hires,
	-- Hires by Contract Type
    SUM(CASE WHEN h.ContractTypeID = 1 THEN 1 ELSE 0 END) AS 'Fixed term', 
    SUM(CASE WHEN h.ContractTypeID = 2 THEN 1 ELSE 0 END) AS Regular,
    --Hires by Age Range
    SUM(CASE WHEN h.AgeBandingID = 1 THEN 1 ELSE 0 END) AS '20-',
    SUM(CASE WHEN h.AgeBandingID = 2 THEN 1 ELSE 0 END) AS '20-30',
    SUM(CASE WHEN h.AgeBandingID = 3 THEN 1 ELSE 0 END) AS '30-40',
	SUM(CASE WHEN h.AgeBandingID = 4 THEN 1 ELSE 0 END) AS '40-50',
    SUM(CASE WHEN h.AgeBandingID = 5 THEN 1 ELSE 0 END) AS '50-60',
    SUM(CASE WHEN h.AgeBandingID = 6 THEN 1 ELSE 0 END) AS '60+',
	--Hires by Gender
    COUNT(CASE WHEN h.Gender = 'Female' THEN 1 END) AS Female,
	COUNT(CASE WHEN h.Gender = 'Male' THEN 1 END) AS Male
FROM Hiring h
JOIN [Time] ON [Time].ID = h.TimeID
WHERE [Time].[Year] = 2024
GROUP BY h.ContractTypeID, h.AgeBandingID

-- Leave Days
SELECT
	-- Leave Taken
    SUM(l.[Days]) AS 'Leave Taken',
	-- Leave Taken by Category
	SUM(CASE WHEN l.Category = 'Sick' THEN 1 ELSE 0 END) 
	AS 'Sick Leave',
    SUM(CASE WHEN l.Category = 'Anual Leave' THEN 1 ELSE 0 END) 
	AS 'Anual Leave',
    SUM(CASE WHEN (l.Category != 'Sick' AND l.Category != 'Anual Leave') 
	THEN 1 ELSE 0 END) AS 'Others', 
	-- Total remaining annual leave (employees được nghỉ phép 12 ngày/năm)
	COUNT(l.[Days]) * 12 - SUM(l.[Days]) AS 'Total remaining annual leave', 
    -- Avg remaining annual leave per person 
	SUM(l.[Days]) / COUNT(l.[Days]) AS 'Avg remaining annual leave per person',
	-- Cost of total remaining annual leave
	SUM(h.BaseSalary / 353) * COUNT(l.[Days]) * 12 - SUM(l.[Days]) 
	AS 'Cost of total remaining annual leave'
FROM Leave l
JOIN Employee e ON e.ID = l.EmployeeID
JOIN Headcount h ON e.ID = h.EmployeeID
JOIN [Time] ON [Time].ID = h.TimeID
WHERE [Time].[Year] = 2024
GROUP BY l.[Days], l.Category 

-- Termination
SELECT
	-- Terminations
    COUNT(*) AS Terminations,
	-- Terminations by Category
	SUM(CASE WHEN t.Category = 'Planned' THEN 1 ELSE 0 END) AS 'Planned',
    SUM(CASE WHEN t.Category = 'Unplaned' THEN 1 ELSE 0 END) AS 'Anual Leave',
	-- Attrition Rate by Seniority
	SUM(CASE WHEN t.SeniorityLevelID = 1 THEN 1 ELSE 0 END) * 100 
	/ SUM(CASE WHEN h.SeniorityLevelID = 1 THEN 1 ELSE 0 END) AS Junior,
    SUM(CASE WHEN t.SeniorityLevelID = 2 THEN 1 ELSE 0 END) * 100 
	/ SUM(CASE WHEN h.SeniorityLevelID = 2 THEN 1 ELSE 0 END) AS MidLevel,
    SUM(CASE WHEN t.SeniorityLevelID = 3 THEN 1 ELSE 0 END) * 100 
	/ SUM(CASE WHEN h.SeniorityLevelID = 3 THEN 1 ELSE 0 END) AS Senior,
	-- Attrition Rate by Tenure Range
    SUM(CASE WHEN h.TenureBandingID = 2 THEN tb.TenureFrom END) 
	/ SUM(CASE WHEN h.TenureBandingID = 2 THEN 1 ELSE 0 END) AS '1-3 Year',
	SUM(CASE WHEN h.TenureBandingID = 3 THEN tb.TenureFrom END) 
	/ SUM(CASE WHEN h.TenureBandingID = 3 THEN 1 ELSE 0 END) AS '3-6 Year',
	SUM(CASE WHEN h.TenureBandingID = 4 THEN tb.TenureFrom END) 
	/ SUM(CASE WHEN h.TenureBandingID = 4 THEN 1 ELSE 0 END) AS '6-10 Year',
	SUM(CASE WHEN h.TenureBandingID = 5 THEN tb.TenureFrom END) 
	/ SUM(CASE WHEN h.TenureBandingID = 2 THEN 1 ELSE 0 END) AS '10+ Year'
FROM Termination t, Headcount h
JOIN TenureBanding tb ON tb.ID = h.TenureBandingID
JOIN [Time] ON [Time].ID = h.TimeID
WHERE [Time].[Year] = 2024
GROUP BY t.Category,t.SeniorityLevelID, h.SeniorityLevelID
