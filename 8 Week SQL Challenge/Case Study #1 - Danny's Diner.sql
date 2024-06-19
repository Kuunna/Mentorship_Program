-- 1. What is the total amount each customer spent at the restaurant
SELECT s.customer_id,
	SUM(m.price) AS total_amount
FROM sales s
JOIN menu m ON m.product_id = s.product_id
GROUP BY s.customer_id
ORDER BY s.customer_id

-- 2. How many days has each customer visited the restaurant
SELECT customer_id, COUNT(order_date) AS visited_date
FROM (
	SELECT customer_id, order_date
    FROM sales
    GROUP BY customer_id, order_date
) unique_dates
GROUP BY customer_id;

-- 3. What was the first item from the menu purchased by each customer
WITH first_purchases AS (
    SELECT 
      s.customer_id,
      m.product_name,
      s.order_date,
      ROW_NUMBER() OVER (
        PARTITION BY s.customer_id 
        ORDER BY s.order_date, s.product_id
      ) AS rank
    FROM sales s
    JOIN menu m ON s.product_id = m.product_id
)
SELECT customer_id, product_name, order_date
FROM first_purchases
WHERE rank = 1;

-- 4. What is the most purchased item on the menu and how many times was it purchased by all customers
SELECT m.product_name,
	Count(s.product_id) AS purchase_count
FROM sales s
JOIN menu m ON m.product_id = s.product_id
GROUP BY m.product_name
ORDER BY most_purchased_item DESC
LIMIT 1

-- 5. Which item was the most popular for each customer
-- WITH customer_purchases AS (
    SELECT 
        s.customer_id, 
  		m.product_name,
  		COUNT(s.product_id) AS purchase_count,
  		ROW_NUMBER() OVER (
          PARTITION BY s.customer_id
          ORDER BY COUNT(s.product_id) DESC) AS rk
    FROM sales s
    JOIN menu m ON s.product_id = m.product_id
    GROUP BY s.customer_id, m.product_name
)
SELECT customer_id, product_name, purchase_count
FROM customer_purchases
WHERE rk = 1

-- 6. Which item was purchased first by the customer after they became a member
WITH first_purchases AS (
    SELECT 
      s.customer_id,
      m.product_name,
      s.order_date,
      ROW_NUMBER() OVER (
        PARTITION BY s.customer_id 
        ORDER BY s.order_date, s.product_id
      ) AS rank
    FROM sales s
    JOIN menu m ON s.product_id = m.product_id
  	JOIN members mb ON s.customer_id = mb.customer_id
					AND s.order_date  mb.join_date
)
SELECT customer_id, product_name
FROM first_purchases
WHERE rank = 1;

-- 7. Which item was purchased just before the customer became a member
WITH first_purchases AS (
    SELECT 
      s.customer_id,
      m.product_name,
      s.order_date,
      DENSE_RANK() OVER (
        PARTITION BY s.customer_id 
        ORDER BY s.order_date DESC
      ) AS rank
    FROM sales s
    JOIN menu m ON s.product_id = m.product_id
  	JOIN members mb ON s.customer_id = mb.customer_id
					AND s.order_date  mb.join_date
)
SELECT customer_id, product_name
FROM first_purchases
WHERE rank = 1;


-- 8. What is the total items and amount spent for each member before they became a member
SELECT 
	s.customer_id,
    COUNT(s.product_id) AS total_items,
    SUM(m.price) AS amount_spent
FROM sales s
JOIN menu m ON s.product_id = m.product_id
JOIN members mb ON s.customer_id = mb.customer_id
  				AND s.order_date  mb.join_date
GROUP BY s.customer_id
ORDER BY s.customer_id

-- 9.  If each $1 spent equates to 10 points and sushi has a 2x points multiplier - how many points would each customer have
WITH customer_spending AS (
  SELECT 
    s.customer_id,
    s.product_id,
    CASE
      WHEN s.product_id = 1 THEN (m.price  20)
      ELSE (m.price  10)
    END AS points
  FROM sales s
  JOIN menu m ON s.product_id = m.product_id
  where s.order_date BETWEEN '2021-01-01' AND '2021-01-31'
)
SELECT customer_id,
	   SUM(points) AS total_points
FROM customer_spending
GROUP BY customer_id
ORDER BY customer_id


-- 10. In the first week after a customer joins the program (including their join date) they earn 2x points on all items, not just sushi - 
   --how many points do customer A and B have at the end of January
WITH customer_spending AS (
  SELECT 
    s.customer_id,
    s.product_id,
    CASE
		-- 2x points if within the first week of joining for all items
    	WHEN s.order_date BETWEEN mb.join_date 
  						  AND DATEADD(day, 6, mb.join_date) 
                          THEN (m.price  20)
        -- 2x points if sushi, outside of the first week of joining
  		WHEN m.product_id = 1 THEN (m.price  20)
  		ELSE (m.price  10)
    END AS points
  FROM sales s
  JOIN menu m ON s.product_id = m.product_id
  JOIN members mb ON s.customer_id = mb.customer_id
  WHERE s.customer_id IN ('A', 'B') 
  	AND s.order_date BETWEEN '2021-01-01' AND '2021-01-31'
)
SELECT customer_id,
	   SUM(points) AS total_points
FROM customer_spending
GROUP BY customer_id
ORDER BY customer_id