-- A. High Level Sales Analysis
-- 1. What was the total quantity sold for all products?
SELECT 
  pd.product_name, 
  SUM(s.qty) AS total_quantity
FROM sales s
JOIN product_details pd ON s.prod_id = pd.product_id
GROUP BY pd.product_name

-- 2. What is the total generated revenue for all products before discounts?
SELECT 
  pd.product_name, 
  SUM(s.qty) * SUM(s.price) AS total_revenue
FROM sales s
INNER JOIN product_details pd ON s.prod_id = pd.product_id
GROUP BY pd.product_name

-- 3. What was the total discount amount for all products?
SELECT 
  product.product_name, 
  SUM(s.qty * s.price * s.discount / 100) AS total_discount
FROM sales s
INNER JOIN product_details pd ON s.prod_id = pd.product_id
GROUP BY pd.product_name

-- B. Transaction Analysis
-- 1. How many unique transactions were there?
SELECT COUNT(DISTINCT txn_id) AS transaction_count
FROM sales

-- 2. What is the average unique products purchased in each transaction? 
with total_quantities as (
	SELECT 
      txn_id, 
      SUM(qty) AS total_quantity
    FROM sales
    GROUP BY txn_id
)
SELECT ROUND(AVG(total_quantity)) AS avg_unique_products
FROM total_quantities

-- 3. What are the 25th, 50th and 75th percentile values for the revenue per transaction? 
WITH revenue_cte AS (
    SELECT txn_id, 
      	SUM(price * qty) AS revenue
    FROM sales
    GROUP BY txn_id
)
SELECT
  	PERCENTILE_CONT(0.25) WITHIN GROUP (ORDER BY revenue) AS median_25th,
    PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY revenue) AS median_50th,
	PERCENTILE_CONT(0.75) WITHIN GROUP (ORDER BY revenue) AS median_75th
FROM revenue_cte

-- 4. What is the average discount value per transaction? 
with discounted_value as (
  SELECT 
	  	txn_id,
  		SUM(qty * price * discount / 100.0) AS discount_per_trans
  FROM sales
  GROUP BY txn_id
)
SELECT ROUND(AVG(discount_per_trans)) AS avg_discount
FROM discounted_value

-- 5. What is the percentage split of all transactions for members vs non-members? 
WITH transactions_cte AS (
    SELECT
      member,
      COUNT(DISTINCT txn_id) AS transactions
    FROM sales
    GROUP BY member
)
SELECT
    member,
    transactions,
    ROUND(100.0 * transactions / (SELECT SUM(transactions) fROM transactions_cte), 2) AS percentage
FROM transactions_cte
GROUP BY member, transactions

-- 6. What is the average revenue for member transactions and non-member transactions? 
WITH revenue_cte AS ( 
  SELECT
    member,
  	txn_id,
    SUM(price * qty) AS revenue
  FROM sales
  GROUP BY member, txn_id
)
SELECT member,
	ROUND(AVG(revenue), 2) as avg_revenue
FROM revenue_cte
GROUP by member

-- C. Product Analysis
-- 1. What are the top 3 products by total revenue before discount?
SELECT 
  pd.product_id,
  pd.product_name, 
  SUM(s.qty) * SUM(s.price) AS total_revenue
FROM sales s
JOIN product_details pd ON s.prod_id = pd.product_id
GROUP BY pd.product_id, pd.product_name
ORDER BY total_revenue DESC
LIMIT 3

-- 2. What is the total quantity, revenue and discount for each segment? 
SELECT 
  pd.segment_id,
  pd.segment_name, 
  SUM(s.qty) AS total_quantity,
  SUM(s.qty * s.price) AS total_revenue,
  SUM(s.qty * s.price * s.discount / 100) AS total_discount
FROM sales s
JOIN product_details pd ON s.prod_id = pd.product_id
GROUP BY pd.segment_id, pd.segment_name

-- 3. What is the top selling product for each segment? 
with top_selling as (
	SELECT
  		pd.segment_name,
  		pd.product_name,
  		sum(s.qty) as total_quantity,
  		DENSE_RANK() OVER (
          PARTITION BY pd.segment_name
          ORDER BY sum(s.qty) DESC) AS ranking
  	from sales s 
  	join product_details pd on s.prod_id = pd.product_id
  	GROUP by pd.segment_name, pd.product_name
)
SELECT
	segment_name,
    product_name,
    total_quantity
from top_selling
WHERE ranking = 1
-- 4. What is the total quantity, revenue and discount for each category? 
SELECT 
  p.category_id,
  p.category_name, 
  SUM(s.qty) AS total_quantity,
  SUM(s.qty * s.price) AS total_revenue,
  SUM(s.qty * s.price * s.discount / 100) AS total_discount
FROM sales s
INNER JOIN product_details p oN s.prod_id = p.product_id
GROUP BY p.category_id, p.category_name
ORDER BY p.category_id

-- 5. What is the top selling product for each category? 
WITH top_selling AS ( 
  SELECT 
    p.category_name, 
    p.product_name,
    SUM(s.qty) AS total_quantity,
    DENSE_RANK() OVER (
      PARTITION BY p.category_name
      ORDER BY SUM(s.qty) DESC) AS ranking
  FROM sales s
  JOIN product_details p ON s.prod_id = p.product_id
  GROUP BY p.category_name, p.product_name
)

SELECT 
  category_name, 
  product_name,
  total_quantity
FROM top_selling
WHERE ranking = 1

-- 6. What is the percentage split of revenue by product for each segment? 
WITH revenue_by_product AS ( 
  SELECT
    prod_id,
  	ROUND(100.0 * SUM(price * qty) / (SELECT SUM(price * qty) FROM sales), 2) revenue_percent_by_product
  FROM sales
  GROUP by prod_id
)
SELECT 
  pd.segment_name,
  pd.product_name, 
  r.revenue_percent_by_product
FROM revenue_by_product r
JOIN product_details pd ON r.prod_id = pd.product_id

-- 7. What is the percentage split of revenue by segment for each category? 
SELECT
    pd.category_name,
  	pd.segment_name,
	ROUND(100.0 * SUM(s.price * s.qty) / (SELECT SUM(price * qty) FROM sales), 2) revenue_percent_by_segment
FROM sales s
join product_details pd on s.prod_id = pd.product_id
GROUP by pd.category_name, pd.segment_name

-- 8. What is the percentage split of total revenue by category? 
SELECT
	pd.category_id,
    pd.category_name,
	ROUND(100.0 * SUM(s.price * s.qty) / (SELECT SUM(price * qty) FROM sales), 2) revenue_percent_by_category
FROM sales s
join product_details pd on s.prod_id = pd.product_id
GROUP by pd.category_id,pd.category_name

-- 9. What is the total transaction “penetration” for each product? (hint: penetration = number of transactions where at least 1 quantity of a product was purchased divided by total number of transactions) 
SELECT
    pd.product_name,
	ROUND(100.0 * COUNT(DISTINCT s.txn_id) / (SELECT count(DISTINCT txn_id) FROM sales), 2) revenue_percent_by_segment
FROM sales s
join product_details pd on s.prod_id = pd.product_id
WHERE s.qty > 0
GROUP by pd.product_name
