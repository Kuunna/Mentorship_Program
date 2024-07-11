# 📕 SQL Challenge - Session 2
<p align="center">
<img src="https://github.com/Kuunna/Mentorship_Program/assets/85633982/ad3bd1d7-b7ab-4b10-9007-ad6b905e9b77" align="center" width="1000" height="370" >

## 🚀 Table of Contents
**1. Create table**
```TSQL
CREATE TABLE runner (
    id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    main_distance INT NOT NULL,
    age INT NOT NULL,
    is_female BIT NOT NULL
)

CREATE TABLE event (
    id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    start_date DATE NOT NULL,
    city VARCHAR(255) NOT NULL
)

CREATE TABLE runner_event (
    runner_id INT NOT NULL,
    event_id INT NOT NULL
)

INSERT INTO runner (id, name, main_distance, age, is_female) VALUES
(1, 'Alice', 5000, 25, 1),
(2, 'Bob', 10000, 32, 0),
(3, 'Charlie', 10000, 28, 0),
(4, 'Daisy', 1500, 22, 1),
(5, 'Eve', 5000, 19, 1),
(6, 'Frank', 5000, 35, 0),
(7, 'Grace', 5000, 42, 1),
(8, 'Hannah', 10000, 38, 1),
(9, 'Ivy', 10000, 45, 1),
(10, 'Jack', 1500, 52, 0);

INSERT INTO event (id, name, start_date, city) VALUES
(1, 'London Marathon', '2023-04-23', 'London'),
(2, 'Warsaw Runs', '2023-05-15', 'Warsaw'),
(3, 'New Year Run', '2023-01-01', 'New York');

INSERT INTO runner_event (runner_id, event_id) VALUES
(1, 1),
(2, 1),
(3, 2),
(4, 3),
(5, 2),
(6, 1),
(7, 3),
(8, 2),
(9, 1),
(10, 3);
```
Table runner

![Screenshot 2024-07-11 075145](https://github.com/Kuunna/Mentorship_Program/assets/85633982/1095823f-cb29-425d-aee1-6bc2d1ebf4a9)

Table event

![Screenshot 2024-07-11 075156](https://github.com/Kuunna/Mentorship_Program/assets/85633982/5ba32f75-fc61-4d7b-87e1-e1b4483d7889)

Table runner_event

![Screenshot 2024-07-11 075203](https://github.com/Kuunna/Mentorship_Program/assets/85633982/db4df6d2-e47b-40f4-bcbb-61a9fd84a604)

##❓ Case Study Questions

**2. Organize Runners Into Groups**
```TSQL
SELECT main_distance, COUNT(id) AS runners_number
FROM runner
GROUP BY main_distance
HAVING COUNT(id) > 3
```
![Screenshot 2024-07-11 075255](https://github.com/Kuunna/Mentorship_Program/assets/85633982/a1e1de64-1813-4bde-8482-f85c2eaa8377)

**3. How Many Runners Participate in Each Event**
```TSQL
SELECT e.name AS event_name, 
       COUNT(re.runner_id) AS runner_count
FROM event e
LEFT JOIN runner_event re ON e.id = re.event_id
GROUP BY e.name
```
![Screenshot 2024-07-11 075305](https://github.com/Kuunna/Mentorship_Program/assets/85633982/75374434-0bd3-420f-8d6d-e5490f0bea1b)

**4. Group Runners by Main Distance and Age**
```TSQL
SELECT main_distance,
       SUM(CASE WHEN age < 20 THEN 1 ELSE 0 END) AS under_20,
       SUM(CASE WHEN age BETWEEN 20 AND 29 THEN 1 ELSE 0 END) AS age_20_29,
       SUM(CASE WHEN age BETWEEN 30 AND 39 THEN 1 ELSE 0 END) AS age_30_39,
       SUM(CASE WHEN age BETWEEN 40 AND 49 THEN 1 ELSE 0 END) AS age_40_49,
       SUM(CASE WHEN age >= 50 THEN 1 ELSE 0 END) AS over_50
FROM runner
GROUP BY main_distance
```
![Screenshot 2024-07-11 075316](https://github.com/Kuunna/Mentorship_Program/assets/85633982/385be863-c8ec-47bd-8ca1-3233d02722c6)
