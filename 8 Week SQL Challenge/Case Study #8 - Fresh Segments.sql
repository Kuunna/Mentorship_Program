-- A. Data Exploration and Cleansing
-- 1. Update the fresh_segments.interest_metrics table by modifying the month_year column to be a date data type with the start of the month


-- 2. What is count of records in the fresh_segments.interest_metrics for each month_year value sorted in chronological order (earliest to latest) with the null values appearing first?


-- 3. What do you think we should do with these null values in the fresh_segments.interest_metrics? 


-- 4. How many interest_id values exist in the fresh_segments.interest_metrics table but not in the fresh_segments.interest_map table? What about the other way around? 


-- 5. Summarise the id values in the fresh_segments.interest_map by its total record count in this table.


-- 6. What sort of table join should we perform for our analysis and why? Check your logic by checking the rows where 'interest_id = 21246' in your joined output and include all columns from fresh_segments.interest_metrics and all columns from fresh_segments.interest_map except from the id column. 


-- 7. Are there any records in your joined table where the month_year value is before the created_at value from the fresh_segments.interest_map table? Do you think these values are valid and why? 


-- B. Interest Analysis
-- 1. Which interests have been present in all month_year dates in our dataset?


-- 2. Using this same total_months measure - calculate the cumulative percentage of all records starting at 14 months - which total_months value passes the 90% cumulative percentage value? 


-- 3. If we were to remove all interest_id values which are lower than the total_months value we found in the previous question - how many total data points would we be removing? 


-- 4. Does this decision make sense to remove these data points from a business perspective? 


-- 5. If we include all of our interests regardless of their counts - how many unique interests are there for each month? 


-- C. Segment Analysis 
-- 1. Using the complete dataset - which are the top 10 and bottom 10 interests which have the largest composition values in any month_year? Only use the maximum composition value for each interest but you must keep the corresponding month_year


-- 2. Which 5 interests had the lowest average ranking value?


-- 3. Which 5 interests had the largest standard deviation in their percentile_ranking value?


-- 4. For the 5 interests found in the previous question - what was minimum and maximum percentile_ranking values for each interest and its corresponding year_month value? Can you describe what is happening for these 5 interests?


-- 5. How would you describe our customers in this segment based off their composition and ranking values? What sort of products or services should we show to these customers and what should we avoid?

-- D. Index Analysis 
-- 1. What is the top 10 interests by the average composition for each month?


-- 2. For all of these top 10 interests - which interest appears the most often?


-- 3. What is the average of the average composition for the top 10 interests for each month?


-- 4. What is the 3 month rolling average of the max average composition value from September 2018 to August 2019 and include the previous top ranking interests in the same output shown below.


-- 5. Provide a possible reason why the max average composition might change from month to month? Could it signal something is not quite right with the overall business model for Fresh Segments?
