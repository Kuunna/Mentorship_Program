-- A. Data Exploration and Cleansing
-- 1. Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month
ALTER TABLE interest_metrics
ALTER COLUMN month_year VARCHAR(10)

UPDATE interest_metrics
SET month_year =  CONVERT(DATE, '01-' + month_year, 105)

ALTER TABLE interest_metrics
ALTER COLUMN month_year DATE;

SELECT * FROM fresh_segments.dbo.interest_metrics;

-- 2. What is count of records in the fresh_segments.interest_metrics for each month_year value sorted in chronological order (earliest to latest) with the null values appearing first?
SELECT month_year, 
	COUNT(*)
FROM interest_metrics
GROUP BY month_year
ORDER BY month_year NULLS FIRST

-- 3. What do you think we should do with these null values in the fresh_segments.interest_metrics?
SELECT *
FROM interest_metrics
WHERE month_year IS NULL
ORDER BY interest_id DESC; 

DELETE FROM interest_metrics
WHERE interest_id IS NULL

-- 4. How many interest_id values exist in the fresh_segments.interest_metrics table but not in the fresh_segments.interest_map table? What about the other way around? 
ALTER TABLE interest_metrics ADD COLUMN interest_id_temp INTEGER;
UPDATE interest_metrics SET interest_id_temp = CAST(interest_id AS INTEGER);
ALTER TABLE interest_metrics DROP COLUMN interest_id;
ALTER TABLE interest_metrics RENAME COLUMN interest_id_temp TO interest_id;

SELECT 
  COUNT(DISTINCT map.id) AS map_id_count,
  COUNT(DISTINCT metrics.interest_id) AS metrics_id_count,
  SUM(CASE WHEN map.id is NULL THEN 1 END) AS not_in_metric,
  SUM(CASE WHEN metrics.interest_id is NULL THEN 1 END) AS not_in_map
FROM interest_map map
FULL JOIN interest_metrics metrics ON metrics.interest_id = map.id

-- 5. Summarise the id values in the fresh_segments.interest_map by its total record count in this table.
SELECT COUNT(*) AS map_id_count
FROM interest_map;

-- 6. What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where 'interest_id = 21246' in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column. 
SELECT 
  metrics.*, 
  map.interest_name, 
  map.interest_summary, 
  map.created_at, 
  map.last_modified
FROM interest_map map
FULL OUTER JOIN interest_metrics metrics ON metrics.interest_id = map.id
WHERE metrics.interest_id = 21246 OR map.id = 21246

-- B. Interest Analysis
-- 1. Which interests have been present in all month_year dates in our dataset?
SELECT interest_id,
	COUNT(month_year) AS cnt
FROM interest_metrics
GROUP BY interest_id
HAVING COUNT(month_year) = (SELECT COUNT(DISTINCT month_year) FROM interest_metrics)

-- 2. Using this same total_months measure - calculate the cumulative percentage of all records starting at 14 months - which total_months value passes the 90% cumulative percentage value? 
WITH interest_months AS (
  SELECT
    interest_id,
    COUNT(DISTINCT month_year) AS total_months
  FROM interest_metrics
  GROUP BY interest_id
),
interest_count AS (
  SELECT
    total_months,
    COUNT(interest_id) AS interests
  FROM interest_months
  GROUP BY total_months
)
SELECT *,
  ROUND(100 * SUM(interests) OVER (ORDER BY total_months DESC) 
        / (SUM(interests) OVER ()),2) AS cumulative_percentage
FROM interest_count

-- 3. If we were to remove all interest_id values which are lower than the total_months value we found in the previous question - how many total data points would we be removing? 
WITH interest_months AS (
  SELECT
    interest_id,
    COUNT(DISTINCT month_year) AS total_months
  FROM interest_metrics
  GROUP BY interest_id
)

SELECT 
  COUNT(interest_id) AS interests,
  COUNT(DISTINCT interest_id) AS unique_interests
FROM interest_metrics
WHERE interest_id IN (
  SELECT interest_id 
  FROM interest_months
  WHERE total_months < 6);

-- 4. Does this decision make sense to remove these data points from a business perspective? 


-- 5. If we include all of our interests regardless of their counts - how many unique interests are there for each month? 
WITH interest_metrics_edited AS (
SELECT *
FROM interest_metrics
WHERE interest_id NOT IN (
  SELECT interest_id
  FROM interest_metrics
  GROUP BY interest_id
  HAVING COUNT(DISTINCT month_year) < 6)
)
SELECT 
  month_year,
  COUNT(DISTINCT interest_id) AS unique_interests
FROM interest_metrics_edited
GROUP BY month_year
ORDER BY month_year
-- C. Segment Analysis 
-- 1. Using the complete dataset - which are the top 10 and bottom 10 interests which have the largest composition values in any month_year? Only use the maximum composition value for each interest but you must keep the corresponding month_year


-- 2. Which 5 interests had the lowest average ranking value?
WITH interest_metrics_edited AS (
SELECT *
FROM interest_metrics
WHERE interest_id NOT IN (
  SELECT interest_id
  FROM interest_metrics
  GROUP BY interest_id
  HAVING COUNT(DISTINCT month_year) < 6)
)
SELECT 
  metrics.interest_id,
  map.interest_name,
  Round(AVG(1.0 * metrics.ranking), 2) AS avg_ranking
FROM interest_metrics_edited metrics
JOIN interest_map map ON metrics.interest_id = map.id
GROUP BY metrics.interest_id, map.interest_name
ORDER BY avg_ranking
LIMIT 5

-- 3. Which 5 interests had the largest standard deviation in their percentile_ranking value?
WITH interest_metrics_edited AS (
  SELECT *
  FROM interest_metrics
  WHERE interest_id NOT IN (
    SELECT interest_id
    FROM interest_metrics
    GROUP BY interest_id
    HAVING COUNT(DISTINCT month_year) < 6)
)
SELECT 
  DISTINCT metrics.interest_id,
  map.interest_name,
  ROUND(CAST(STDDEV_SAMP(metrics.percentile_ranking) OVER (PARTITION BY metrics.interest_id) AS NUMERIC), 2) AS std_percentile_ranking
FROM interest_metrics_edited metrics
JOIN interest_map map ON metrics.interest_id = map.id
ORDER BY std_percentile_ranking DESC
LIMIT 5

-- 4. For the 5 interests found in the previous question - what was minimum and maximum percentile_ranking values for each interest and its corresponding year_month value? Can you describe what is happening for these 5 interests?


-- 5. How would you describe our customers in this segment based off their composition and ranking values? What sort of products or services should we show to these customers and what should we avoid?

-- D. Index Analysis 
-- 1. What is the top 10 interests by the average composition for each month?


-- 2. For all of these top 10 interests - which interest appears the most often?


-- 3. What is the average of the average composition for the top 10 interests for each month?


-- 4. What is the 3 month rolling average of the max average composition value from September 2018 to August 2019 and include the previous top ranking interests in the same output shown below.


-- 5. Provide a possible reason why the max average composition might change from month to month? Could it signal something is not quite right with the overall business model for Fresh Segments?
