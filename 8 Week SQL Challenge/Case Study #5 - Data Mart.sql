-- B. Data Exploration
-- 1. What day of the week is used for each week_date value? 
SELECT 
	DISTINCT(TO_CHAR(week_date, 'day')) AS week_day 
FROM clean_weekly_sales

-- 2. What range of week numbers are missing from the dataset? 
WITH week_number AS (
  SELECT GENERATE_SERIES(1,52) AS week_number
)
SELECT 
	DISTINCT wn.week_number
FROM week_number wn
LEFT JOIN clean_weekly_sales ws ON wn.week_number = ws.week_number
WHERE ws.week_number IS NULL

-- 3. How many total transactions were there for each year in the dataset? 
SELECT 
  calendar_year, 
  Count(transactions) AS total_transactions
FROM clean_weekly_sales
GROUP BY calendar_year
ORDER BY calendar_year

-- 4. What is the total sales for each region for each month?
SELECT 
  month_number, 
  region,
  SUM(sales) AS total_sales
FROM clean_weekly_sales
GROUP BY month_number, region
ORDER BY month_number, region

-- 5. What is the total count of transactions for each platform? 
SELECT 
  platform, 
  Sum(transactions) AS total_transactions
FROM clean_weekly_sales
GROUP BY platform

-- 6. What is the percentage of sales for Retail vs Shopify for each month?
WITH monthly_transactions AS (
  SELECT 
    calendar_year, 
    month_number, 
    platform, 
    SUM(sales) AS monthly_sales
  FROM clean_weekly_sales
  GROUP BY calendar_year, month_number, platform
)
SELECT 
  calendar_year, 
  month_number, 
  ROUND(100.0 * SUM(CASE WHEN platform = 'Retail' THEN monthly_sales ELSE NULL END) / SUM(monthly_sales), 2) AS retail_percent,
  ROUND(100.0 * SUM(CASE WHEN platform = 'Shopify'THEN monthly_sales ELSE NULL END) / SUM(monthly_sales), 2) AS shopify_percent
FROM monthly_transactions
GROUP BY calendar_year, month_number
ORDER BY calendar_year, month_number

-- 7. What is the percentage of sales by demographic for each year in the dataset? 
WITH demographic_sales AS (
  SELECT 
    calendar_year, 
    demographic, 
    SUM(sales) AS yearly_sales
  FROM clean_weekly_sales
  GROUP BY calendar_year, demographic
)

