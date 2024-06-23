-- A. Customer Nodes Exploration 
-- 1. How many unique nodes are there on the Data Bank system? 
-- Variant A:
SELECT 
	COUNT(DISTINCT node_id) AS unique_nodes
FROM customer_nodes
-- Variant B:
SELECT 
	COUNT(*) AS unique_nodes
FROM (
	SELECT node_id
    FROM customer_nodes
    GROUP BY node_id
) Customer

-- 2. What is the number of nodes per region? 
SELECT
  r.region_name, 
  COUNT(DISTINCT cn.node_id) AS node_count
FROM regions r
JOIN customer_nodes cn ON r.region_id = cn.region_id
GROUP BY r.region_name

-- 3. How many customers are allocated to each region? 
SELECT
  cn.region_id,
  r.region_name, 
  COUNT(cn.customer_id) AS customer_count
FROM regions r
JOIN customer_nodes cn ON r.region_id = cn.region_id
GROUP BY cn.region_id, r.region_name
ORDER by cn.region_id

-- 4. How many days on average are customers reallocated to a different node? 
WITH node_days AS (
  SELECT 
    customer_id, 
    node_id,
    end_date - start_date AS days_in_node
  FROM customer_nodes
  WHERE end_date != '9999-12-31'
) , total_node_days AS (
  SELECT 
    customer_id,
    node_id,
    SUM(days_in_node) AS total_days_in_node
  FROM node_days
  GROUP BY customer_id, node_id
)
SELECT ROUND(AVG(total_days_in_node), 1) AS avg_node_reallocation_days
FROM total_node_days

-- 5. What is the median, 80th and 95th percentile for this same reallocation days metric for each region?
 WITH node_days AS (
  SELECT 
    customer_id, 
    region_id,
    end_date - start_date AS days_in_node
  FROM customer_nodes
  WHERE end_date != '9999-12-31'
), total_node_days AS (
  SELECT 
    customer_id,
    region_id,
    SUM(days_in_node) AS total_days_in_node
  FROM node_days
  GROUP BY customer_id, region_id
)
SELECT 
  region_id,
  PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY total_days_in_node) AS median_days,
  PERCENTILE_CONT(0.8) WITHIN GROUP (ORDER BY total_days_in_node) AS percentile_80,
  PERCENTILE_CONT(0.95) WITHIN GROUP (ORDER BY total_days_in_node) AS percentile_95
FROM total_node_days
GROUP BY region_id

-- B. Customer Transactions
-- 1. What is the unique count and total amount for each transaction type?
SELECT
  txn_type, 
  COUNT(customer_id) AS transaction_count, 
  SUM(txn_amount) AS total_amount
FROM customer_transactions
GROUP BY txn_type

-- 2. What is the average total historical deposit counts and amounts for all customers? 
WITH deposits AS (
  SELECT 
    customer_id, 
    COUNT(customer_id) AS txn_count, 
    AVG(txn_amount) AS avg_amount
  FROM customer_transactions
  WHERE txn_type = 'deposit'
  GROUP BY customer_id
)
SELECT 
  ROUND(AVG(txn_count), 1) AS avg_deposit_count, 
  ROUND(AVG(avg_amount), 1) AS avg_deposit_amt
FROM deposits

-- 3. For each month - how many Data Bank customers make more than 1 deposit and either 1 purchase or 1 withdrawal in a single month? 
WITH monthly_transactions AS (
  SELECT 
    customer_id, 
    DATE_PART('month', txn_date) AS mth,
    SUM(CASE WHEN txn_type = 'deposit' THEN 0 ELSE 1 END) AS deposit_count,
    SUM(CASE WHEN txn_type = 'purchase' THEN 0 ELSE 1 END) AS purchase_count,
    SUM(CASE WHEN txn_type = 'withdrawal' THEN 1 ELSE 0 END) AS withdrawal_count
  FROM customer_transactions
  GROUP BY customer_id, DATE_PART('month', txn_date)
)
SELECT
  mth,
  COUNT(DISTINCT customer_id) AS customer_count
FROM monthly_transactions
WHERE deposit_count > 1 AND (purchase_count >= 1 OR withdrawal_count >= 1)
GROUP BY mth

-- 4. What is the closing balance for each customer at the end of the month? Also show the change in balance each month in the same table output.
-- Tính số dư giao dịch hàng tháng cho mỗi khách hàng
WITH monthly_balances AS (
  SELECT 
    customer_id, 
    EXTRACT(MONTH FROM txn_date) AS month, 
    SUM(CASE 
      		WHEN txn_type = 'withdrawal' OR txn_type = 'purchase' THEN -txn_amount
      		ELSE txn_amount 
        END) AS monthly_balance
  FROM customer_transactions
  GROUP BY customer_id, EXTRACT(MONTH FROM txn_date)
),
-- Tính số dư tích lũy hàng tháng cho mỗi khách hàng
cumulative_balances AS (
  SELECT 
    customer_id, 
    month, 
    SUM(monthly_balance) OVER (PARTITION BY customer_id ORDER BY month) AS closing_balance
  FROM monthly_balances
)
SELECT 
  customer_id, 
  month, 
  COALESCE(closing_balance, 0) AS closing_balance,
  COALESCE(closing_balance - LAG(closing_balance, 1) OVER (PARTITION BY customer_id ORDER BY month), 0) AS change_in_balance
FROM cumulative_balances
ORDER BY customer_id, month
