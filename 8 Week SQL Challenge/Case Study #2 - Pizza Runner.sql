CREATE TABLE customer_orders (
    order_id INT,
    customer_id INT,
    pizza_id INT,
    exclusions VARCHAR(50),
    extras VARCHAR(50),
    order_time DATETIME
);

INSERT INTO customer_orders (order_id, customer_id, pizza_id, exclusions, extras, order_time)
VALUES 
(1, 101, 1, '', '', '2021-01-01 18:05:02.000'),
(2, 101, 1, '', '', '2021-01-01 19:00:52.000'),
(3, 102, 1, '', '', '2021-01-02 23:51:23.000'),
(3, 102, 2, '', '', '2021-01-02 23:51:23.000'),
(4, 103, 1, '4', '', '2021-01-04 13:23:46.000'),
(4, 103, 1, '4', '', '2021-01-04 13:23:46.000'),
(4, 103, 2, '4', '', '2021-01-04 13:23:46.000'),
(5, 104, 1, '', '1', '2021-01-08 21:00:29.000'),
(6, 101, 2, '', '', '2021-01-08 21:03:13.000'),
(7, 105, 2, '', '1', '2021-01-08 21:20:29.000'),
(8, 102, 1, '', '', '2021-01-09 23:54:33.000'),
(9, 103, 1, '4', '1,5', '2021-01-10 11:22:59.000'),
(10, 104, 1, '', '', '2021-01-11 18:34:49.000'),
(10, 104, 1, '2,6', '1,4', '2021-01-11 18:34:49.000');

CREATE TABLE runner_orders_temp (
    order_id INT,
    runner_id INT,
    pickup_time VARCHAR(255),
    distance VARCHAR(50),
    duration VARCHAR(50),
    cancellation VARCHAR(255)
);

INSERT INTO runner_orders_temp (order_id, runner_id, pickup_time, distance, duration, cancellation)
VALUES 
(1, 1, '2021-01-01 18:15:34', '20', '32', ''),
(2, 1, '2021-01-01 19:10:54', '20', '27', ''),
(3, 1, '2021-01-03 00:12:37', '13.4', '20', ''),
(4, 2, '2021-01-04 13:53:03', '23.4', '40', ''),
(5, 3, '2021-01-08 21:10:57', '10', '15', ''),
(6, 3, '', '', '', 'Restaurant Cancellation'),
(7, 2, '2021-01-08 21:30:45', '25', '25', ''),
(8, 2, '2021-01-10 00:15:02', '23.4', '15', ''),
(9, 2, '', '', '', 'Customer Cancellation'),
(10, 1, '2021-01-11 18:50:20', '10', '10', '');
ALTER TABLE runner_orders_temp
ALTER COLUMN pickup_time DATETIME;
ALTER TABLE runner_orders_temp
ALTER COLUMN distance FLOAT;
ALTER TABLE runner_orders_temp
ALTER COLUMN duration INT;


CREATE TABLE pizza_names (
    pizza_id INT,
    pizza_name VARCHAR(255)
);

INSERT INTO pizza_names (pizza_id, pizza_name) 
VALUES 
(1, 'Mealovers'),
(2, 'Vegetarian');

CREATE TABLE runners (
    runner_id INT,
    registration_date DATETIME
);

INSERT INTO runners (runner_id, registration_date)
VALUES 
  (1, '2024-01-15 10:30:00'),
  (3, '2024-03-05 09:15:00'),
  (4, '2024-04-12 16:20:00'),
  (5, '2024-05-28 11:00:00');
  

CREATE TABLE pizza_recipes (
    pizza_id INT,
    toppings VARCHAR(255)
);

CREATE TABLE pizza_toppings (
    topping_id INT,
   topping_name VARCHAR(255)
);

-- A. Pizza Metrics
-- 1. How many pizzas were ordered?
SELECT COUNT(*) AS pizzas_orders
FROM customer_orders_temp;

-- 2. How many unique customer orders were made?
SELECT COUNT(DISTINCT *) AS unique_customer_orders
FROM customer_orders_temp;

-- 3. How many successful orders were delivered by each runner ?
SELECT runner_id, COUNT(order_id) AS successful_orders
FROM runner_orders_temp
WHERE distance != 0
GROUP BY runner_id;

-- 4. How many of each type of pizza was delivered?
SELECT 
  p.pizza_name, 
  COUNT(c.pizza_id) AS delivered_pizza
FROM customer_orders_temp AS c
JOIN runner_orders_temp AS r ON c.order_id = r.order_id
JOIN pizza_names AS p ON c.pizza_id = p.pizza_id
WHERE r.distance != 0
Group BY  p.pizza_name

-- 5. How many Vegetarian and Meatlovers were ordered by each customer? 
SELECT 
  c.customer_id, 
  p.pizza_name, 
  COUNT(p.pizza_name) AS order_count
FROM customer_orders_temp AS c 
JOIN pizza_names AS p ON c.pizza_id = p.pizza_id
Group BY  c.customer_id, p.pizza_name
ORDER BY  c.customer_id

-- 6. What was the maximum number of pizzas delivered in a single order? 
WITH pizza_count AS (
	SELECT 
      c.order_id, 
      COUNT(c.pizza_id) as pizza_per_order
    FROM customer_orders_temp AS c 
	JOIN runner_orders_temp AS r ON c.order_id = r.order_id
    WHERE r.distance != 0
  	GROUP by  c.order_id
)
SELECT top 1
	order_id,
    MAX(pizza_per_order) AS max_per_order
FROM pizza_count
GROUP by order_id
ORDER by max_per_order DESC

-- 7. For each customer, how many delivered pizzas had at least 1 change and how many had no changes?
SELECT 
  c.customer_id, 
  SUM( 
    CASE
      WHEN c.exclusions != '' OR c.extras != '' THEN 1
      ELSE 0 
    END
  ) AS at_least_1_change,
  SUM( 
    CASE
      WHEN c.exclusions = '' OR c.extras = '' THEN 1
      ELSE 0 
    END
  ) AS no_change
FROM customer_orders_temp AS c 
JOIN runner_orders_temp AS r ON c.order_id = r.order_id
WHERE r.distance != 0
GROUP by  c.customer_id


-- 8. How many pizzas were delivered that had both exclusions and extras? 
SELECT 
  c.customer_id, 
  SUM(
    CASE
      WHEN exclusions IS NOT NULL AND extras IS NOT NULL THEN 1
      ELSE 0
    END) AS both_exclusions_and_extras
FROM customer_orders_temp AS c 
JOIN runner_orders_temp AS r ON c.order_id = r.order_id
WHERE r.distance != 0 AND exclusions <> ' ' AND extras <> ' '
GROUP by  c.customer_id

-- 9. What was the total volume of pizzas ordered for each hour of the day?
SELECT 
  DATEPART(HOUR, [order_time]) AS hour_of_day, 
  COUNT(order_id) AS pizza_count
FROM customer_orders_temp
GROUP BY DATEPART(HOUR, [order_time]);

-- 10. What was the volume of orders for each day of the week?
SELECT 
  DATENAME(WEEKDAY, order_time) AS hour_of_day, 
  COUNT(order_id) AS pizza_count
FROM customer_orders_temp
GROUP BY DATENAME(WEEKDAY, order_time)

-- B. Runner and Customer Experience
-- 1. How many runners signed up for each 1 week period? (i.e. week starts 2021-01-01) 
SELECT 
  DATEPART(WEEK, registration_date) AS registration_week, 
  COUNT(runner_id) AS runner_signup
FROM runners
GROUP BY DATEPART(WEEK, registration_date)

-- 2. What was the average time in minutes it took for each runner to arrive at the Pizza Runner HQ to pickup the order ?
WITH time_taken AS
( 
  SELECT 
    c.order_id, 
    c.order_time, 
    r.pickup_time, 
    DATEDIFF(MINUTE, c.order_time, r.pickup_time) AS pickup_minutes
  FROM customer_orders_temp AS c
  JOIN runner_orders_temp AS r ON c.order_id = r.order_id
  WHERE r.distance != 0
  GROUP BY c.order_id, c.order_time, r.pickup_time
)
SELECT
	avg(pickup_minutes) AS avg_pigup_times
from time_taken
WHERE pickup_minutes > 1

-- 3. Is there any relationship between the number of pizzas and how long the order takes to prepare?
