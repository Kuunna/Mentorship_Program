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
