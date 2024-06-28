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
