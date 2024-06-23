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
