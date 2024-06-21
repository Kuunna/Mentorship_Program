CREATE TABLE customer_orders_temp (
    order_id INT,
    customer_id INT,
    pizza_id INT,
    exclusions VARCHAR(50),
    extras VARCHAR(50),
    order_time DATETIME
);

INSERT INTO customer_orders_temp (order_id, customer_id, pizza_id, exclusions, extras, order_time)
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
  (2, '2024-01-15 16:20:00'),
  (3, '2024-03-05 09:15:00'),
  (4, '2024-04-12 16:20:00'),
  (5, '2024-05-28 11:00:00');
  

CREATE TABLE pizza_recipes (
    pizza_id INT,
    toppings VARCHAR(255)
);

INSERT INTO pizza_recipes (pizza_id, toppings)
VALUES 
   (1, '1,2,4'),   -- Margherita: Tomato sauce, Mozzarella, Basil
   (2, '1,3,8'),   -- Pepperoni: Tomato sauce, Mozzarella, Pepperoni
   (3, '4,5,6,9'), -- Veggie: Tomato sauce, Mozzarella, Mushrooms, Onions, Peppers
   (4, '1,2,3,7'); -- Meat Lovers: Tomato sauce, Mozzarella, Beef, Sausage


CREATE TABLE pizza_toppings (
    topping_id INT,
   topping_name VARCHAR(255)
);

INSERT INTO pizza_toppings (topping_id, topping_name)
VALUES
   (1, 'Tomato sauce'),
   (2, 'Mozzarella'),
   (3, 'Pepperoni'),
   (4, 'Mushrooms'),
   (5, 'Onions'),
   (6, 'Peppers'),
   (7, 'Sausage'),
   (8, 'Basil'),
   (9, 'Beef');


-- A. Pizza Metrics
-- 1. How many pizzas were ordered?
SELECT COUNT(*) AS pizzas_orders
FROM customer_orders_temp;

-- 2. How many unique customer orders were made?
SELECT COUNT(DISTINCT *) AS unique_customer_orders
FROM customer_orders_temp;

-- 3. How many successful orders were delivered by each runner?
SELECT runner_id, COUNT(order_id) AS successful_orders
FROM runner_orders_temp
WHERE distance > 0
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

--6. What was the maximum number of pizzas delivered in a single order? 
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
WITH time_prepare AS
( 
  SELECT 
    c.order_id, 
    COUNT(c.order_id) AS Number_of_pizza,
    c.order_time, 
    r.pickup_time, 
    DATEDIFF(MINUTE, c.order_time, r.pickup_time) AS prepare_minutes
  FROM customer_orders_temp AS c
  JOIN runner_orders_temp AS r ON c.order_id = r.order_id
  WHERE r.distance != 0
  GROUP BY c.order_id, c.order_time, r.pickup_time
)
SELECT
	Number_of_pizza,
	avg(prepare_minutes) AS avg_pigup_times
from time_prepare
WHERE prepare_minutes > 1
GROUP by Number_of_pizza

-- 4. Average delivery distance corresponding to each customer ?
select 
	c.customer_id,
	avg(r.distance) AS avg_distance
FROM customer_orders_temp c
JOIN runner_orders_temp AS r ON r.order_id = c.order_id
WHERE r.duration > 0
GROUP by c.customer_id

-- 5. What was the difference between the longest and shortest delivery times for all orders? 
SELECT MAX(duration) - MIN(duration) AS delivery_time_difference
FROM runner_orders_temp
where duration != 0

-- 6. What was the average speed for each runner for each delivery and do you notice any trend for these values? 
SELECT 
  c.order_id, 
  c.customer_id, 
  r.runner_id, 
  COUNT(c.order_id) AS pizza_count, 
  ROUND((r.distance/r.duration * 60), 2) AS avg_speed
FROM runner_orders_temp AS r
JOIN customer_orders_temp AS c ON r.order_id = c.order_id
where r.distance > 0
GROUP BY r.runner_id, c.customer_id, c.order_id, r.distance, r.duration
ORDER BY c.order_id;

-- 7. What is the successful delivery percentage for each runner? 
SELECT 
   runner_id,
   SUM(CASE 
          WHEN distance = 0 THEN 0
          ELSE 1 
       END) * 100 / COUNT(*)
   AS success_percentage
FROM runner_orders_temp
GROUP by runner_id

-- C. Ingredient Optimisation
-- 1. What are the standard ingredients for each pizza?
WITH Toppings AS (
    SELECT 
      pr.pizza_id, 
      pt.topping_name
    FROM pizza_recipes pr
    CROSS APPLY STRING_SPLIT(pr.toppings, ',') AS toppings_split
    JOIN pizza_toppings pt ON pt.topping_id = toppings_split.value
)
SELECT 
  pizza_id, 
  STRING_AGG(topping_name, ', ') WITHIN GROUP(ORDER BY topping_name) AS toppings
FROM Toppings
GROUP BY pizza_id

-- 2. What was the most commonly added extra? 
WITH Toppings AS (
    SELECT
      pizza_id,
      value AS topping_id
    FROM pizza_recipes
    CROSS APPLY STRING_SPLIT(toppings, ',')
)
SELECT 
  t.topping_id, 
  pt.topping_name, 
  COUNT(t.topping_id) AS topping_count
from Toppings t 
JOIN pizza_toppings pt ON t.topping_id = pt.topping_id
GROUP BY t.topping_id, pt.topping_name
ORDER BY topping_count DESC;

-- 3. What was the most common exclusion?
WITH exclusions_topping AS (
    SELECT
  		value AS topping_id
    FROM customer_orders_temp
    CROSS APPLY STRING_SPLIT(exclusions, ',')
    WHERE exclusions IS NOT NULL AND exclusions != ''
)
SELECT 
	et.topping_id,
    pt.topping_name,
    COUNT(et.topping_id) AS the_most_common_exclusion
FROM exclusions_topping AS et
JOIN pizza_toppings pt ON et.topping_id = pt.topping_id
GROUP BY et.topping_id, pt.topping_name
ORDER BY the_most_common_exclusion DESC;

-- D. Pricing and Ratings
-- 1. If a Meat Lovers pizza costs $12 and Vegetarian costs $10 and there were no charges for changes 
--    how much money has Pizza Runner made so far if there are no delivery fees?
WITH total AS (
  SELECT 
      order_id, 
      SUM(CASE
              WHEN pizza_id = 1 THEN 12
              WHEN pizza_id = 2 THEN 10
              ELSE 0
          END) as money_total
  from customer_orders_temp
  GROUP by order_id
)
SELECT
	SUM(money_total) AS money_total
from total

-- 2. What if there was an additional $1 charge for any pizza extras? 
WITH total AS (
    SELECT
        order_id,
        SUM(
            CASE
                WHEN pizza_id = 1 THEN 12  
                WHEN pizza_id = 2 THEN 10  
                ELSE 0
            END
        ) + 
        SUM(
            CASE 
          		WHEN (extras IS NOT NULL AND extras != '') THEN (LEN(extras) - LEN(REPLACE(extras, ',', '')) + 1)
          		ELSE 0 
            END) 
        AS money_total
    FROM customer_orders_temp
    GROUP BY order_id
)
SELECT 
	SUM(money_total) AS total_revenue  
FROM total;


-- 3 Design new table for ratings for each successful customer order between 1 to 5.
CREATE TABLE customer_runner_ratings (
    rating_id INT,
    order_id INT,
    customer_id INT,
    runner_id INT,
    rating INT,
    comment VARCHAR(255),
    rating_date DATETIME
 )
INSERT INTO customer_runner_ratings (rating_id, order_id, customer_id, runner_id, rating, comment, rating_date)
VALUES 
	(1, 1, 101, 1, 5, 'Great service, fast delivery!', '2021-01-01 18:30:00'),
	(2, 2, 101, 1, 4, 'Friendly runner, but a bit late.', '2021-01-01 19:30:00'),
	(3, 3, 102, 1, 5, 'Excellent service, highly recommend!', '2021-01-03 00:30:00'),
	(4, 4, 103, 2, 3, 'Food was cold, but the runner was nice.', '2021-01-04 14:30:00'),
	(5, 5, 104, 3, 5, 'Fast and efficient delivery!', '2021-01-08 21:30:00'),
	(6, 7, 105, 2, 4, 'Good communication, arrived on time.', '2021-01-08 22:00:00'),
	(7, 8, 102, 2, 5, 'Amazing service, will order again!', '2021-01-10 00:45:00'),
	(8, 9, 103, 2, 2, 'Late delivery, food was soggy.', '2021-01-10 12:00:00'),
	(9,10, 104, 1, 4, 'Good service, but forgot the extra sauce.', '2021-01-11 19:30:00');

-- 4 Create a table which has the following information for successful deliveries?
SELECT
    c.customer_id,
    c.order_id,
    r.runner_id,
    cr.rating,
    c.order_time,
    r.pickup_time,
    DATEDIFF(MINUTE, c.order_time, r.pickup_time) AS time_between_order_and_pickup,
    r.duration AS delivery_duration, 
    ROUND((r.distance / TRY_CAST(r.duration AS FLOAT))* 60, 2) AS average_speed,
    COUNT(c.order_id) AS total_number_of_pizzas 
FROM customer_orders_temp c
JOIN runner_orders_temp r ON c.order_id = r.order_id
JOIN customer_runner_ratings cr ON c.order_id = cr.order_id
WHERE r.cancellation IS NULL OR r.cancellation = ''  
GROUP BY
    c.customer_id, c.order_id, r.runner_id, cr.rating, c.order_time, r.pickup_time, r.distance, r.duration
ORDER BY c.order_id;
    
-- 5. If a Meat Lovers pizza was $12 and Vegetarian $10 fixed prices with no cost for extras and each runner is paid $0.30 per kilometre traveled 
--    how much money does Pizza Runner have left over after these deliveries?
WITH total AS (
    SELECT
        c.order_id,
        SUM(CASE
                WHEN c.pizza_id = 1 THEN 12  
                WHEN c.pizza_id = 2 THEN 10  
                ELSE 0 END) 
        + SUM(CASE 
          		WHEN (c.extras IS NOT NULL AND c.extras != '') THEN (LEN(c.extras) - LEN(REPLACE(c.extras, ',', '')) + 1)
          		ELSE 0 END ) 
        - SUM(CASE 
          		WHEN r.distance <> 0 THEN r.distance * 3 / 10 
          		ELSE 0 END
        ) AS money_total
    FROM customer_orders_temp c
	JOIN runner_orders_temp r ON c.order_id = r.order_id
    GROUP BY c.order_id
)
SELECT 
	ROUND(SUM(money_total), 2) AS total_revenue  
FROM total
