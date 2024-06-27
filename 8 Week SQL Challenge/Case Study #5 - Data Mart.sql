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
SELECT 
  calendar_year, 
  ROUND(100 * Sum(CASE WHEN demographic = 'Couples' THEN yearly_sales ELSE NULL END) / SUM(yearly_sales), 2) AS couples_percentage,
  ROUND(100 * Sum(CASE WHEN demographic = 'Families'THEN yearly_sales ELSE NULL END) / SUM(yearly_sales), 2) AS families_percentage,
  ROUND(100 * Sum(CASE WHEN demographic = 'unknown' THEN yearly_sales ELSE NULL END) / SUM(yearly_sales), 2) AS unknown_percentage
FROM demographic_sales
GROUP BY calendar_year

-- 8. Which age_band and demographic values contribute the most to Retail sales? 
SELECT 
  age_band, 
  demographic, 
  SUM(sales) AS retail_sales,
  ROUND(100.0 * Sum(sales) / (SELECT SUM(sales) From clean_weekly_sales), 2) AS couples_percentage
FROM clean_weekly_sales
WHERE platform = 'Retail'
GROUP BY age_band, demographic
ORDER BY retail_sales DESC

-- 9. Can we use the avg_transaction column to find the average transaction size for each year for Retail vs Shopify? If not - how would you calculate it instead?
SELECT 
  calendar_year, 
  platform, 
  ROUND(AVG(avg_transaction), 0) AS avg_transaction_row, 
  SUM(sales) / sum(transactions) AS avg_transaction_group
FROM clean_weekly_sales
GROUP BY calendar_year, platform
ORDER BY calendar_year, platform

-- C. Before & After Analysis 
-- 1. What is the total sales for the 4 weeks before and after 2020-06-15? What is the growth or reduction rate in actual values and percentage of sales? 
with before_after_changes AS (
	SELECT 
        SUM(CASE WHEN week_number BETWEEN 21 AND 24 AND calendar_year = 2020 THEN sales ELSE 0 END) AS total_sales_before,
        SUM(CASE WHEN week_number BETWEEN 25 AND 28 AND calendar_year = 2020 THEN sales ELSE 0 END) AS total_sales_after
    FROM clean_weekly_sales
)
SELECT 
  total_sales_before AS total_sales_4_weeks_before,
  total_sales_after as total_sales_4_weeks_after,
  total_sales_after - total_sales_before AS sales_variance, 
  ROUND(100.0 * (total_sales_after - total_sales_before) / total_sales_before, 2) AS variance_percent
FROM before_after_changes

-- 2. What about the entire 12 weeks before and after? 
with before_after_changes AS (
	SELECT 
        SUM(CASE WHEN week_number BETWEEN 13 AND 24 AND calendar_year = 2020 THEN sales ELSE 0 END) AS total_sales_before,
        SUM(CASE WHEN week_number BETWEEN 25 AND 37 AND calendar_year = 2020 THEN sales ELSE 0 END) AS total_sales_after
    FROM clean_weekly_sales
)
SELECT 
  total_sales_before AS total_sales_4_weeks_before,
  total_sales_after as total_sales_4_weeks_after,
  total_sales_after - total_sales_before AS sales_variance, 
  ROUND(100.0 * (total_sales_after - total_sales_before) / total_sales_before, 2) AS variance_percent
FROM before_after_changes

-- 3. How do the sale metrics for these 2 periods before and after compare with the previous years in 2018 and 2019 
