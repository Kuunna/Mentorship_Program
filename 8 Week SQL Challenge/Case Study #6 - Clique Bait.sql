--  A. Digital Analysis
-- 1. How many users are there?
SELECT COUNT(DISTINCT user_id) AS user_count
FROM users

-- 2. How many cookies does each user have on average?
SELECT
	ROUND(AVG(cookie_count)) as avg_cookie
from (
    SELECT user_id, 
        COUNT(cookie_id) AS cookie_count
    FROM users
    GROUP By user_id
)

-- 3. What is the unique number of visits by all users per month?
SELECT 
  EXTRACT(MONTH FROM event_time) as month, 
  COUNT(DISTINCT visit_id) AS visit_count
FROM events
GROUP BY EXTRACT(MONTH FROM event_time)

-- 4. What is the number of events for each event type? 
SELECT 
  event_type, 
  COUNT(*) AS event_count
FROM events
GROUP BY event_type
ORDER BY event_type

-- 5. What is the percentage of visits which have a purchase event?
SELECT 
  Round(100.0 * COUNT(DISTINCT visit_id) / 
        (SELECT COUNT(DISTINCT visit_id) FROM events), 2) AS percentage_purchase
FROM events 
WHERE event_type = 3

-- 6. What is the percentage of visits which view the checkout page but do not have a purchase event?
WITH checkout_purchase AS (
    SELECT 
      visit_id,
      Sum(CASE WHEN event_type = 1 AND page_id = 12 THEN 1 ELSE 0 END) AS checkout,
      SUM(CASE WHEN event_type = 3 THEN 1 ELSE 0 END) AS purchase
    FROM events
    GROUP BY visit_id)
SELECT
	ROUND(100.0 * (1- (SUM(purchase) / SUM(checkout))), 2) as percent_checkout_view_with_no_purchase
FROM checkout_purchase

-- 7. What are the top 3 pages by number of views?
SELECT 
  ph.page_name,
  COUNT(e.visit_id) AS page_views
FROM events e 
join page_hierarchy ph ON e.page_id = ph.page_id
WHERE e.event_type = 1
GROUP by  ph.page_name
ORder by COUNT(e.visit_id) DESC
LIMIT 3

-- 8. What is the number of views and cart adds for each product category?
SELECT 
  ph.product_category,
  sum(CASE when e.event_type = 1 then 1 else 0 end) as page_views,
  sum(CASE when e.event_type = 2 then 1 else 0 end) as cart_adds
FROM events e 
JOIN page_hierarchy AS ph ON e.page_id = ph.page_id
GROUP by ph.product_category

-- 9. What are the top 3 products by purchases?
SELECT 
  ph.product_category,
  sum(CASE when e.event_type = 3 then 1 else 0 end) as purchase
FROM events e 
JOIN page_hierarchy AS ph ON e.page_id = ph.page_id
GROUP by ph.product_category
ORDER by purchase DESC
LIMIT 3