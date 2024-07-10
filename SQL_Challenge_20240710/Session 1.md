#1. Create table
CREATE TABLE color (
    id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    extra_fee DECIMAL(10, 2) DEFAULT 0
)
CREATE TABLE customer (
    id INT PRIMARY KEY,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    favorite_color_id INT
)
CREATE TABLE category (
    id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    parent_id INT
)
CREATE TABLE clothing (
    id INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    size NVARCHAR(10) CHECK (size IN ('S', 'M', 'L', 'XL', '2XL', '3XL')),
    price DECIMAL(10, 2) NOT NULL,
    color_id INT,
    category_id INT
)
CREATE TABLE clothing_order (
    id INT PRIMARY KEY,
    customer_id INT,
    clothing_id INT,
    items INT NOT NULL,
    order_date DATE NOT NULL
)

INSERT INTO color (id, name, extra_fee) VALUES 
(1, 'Red', 0),
(2, 'Blue', 0),
(3, 'Green', 0.50),
(4, 'Yellow', 0.25);

INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES 
(1, 'John', 'Doe', 1),
(2, 'Jane', 'Smith', 2),
(3, 'Alice', 'Johnson', 3),
(4, 'Bob', 'Lee', 4),
(5, 'Eve', 'Brown', 1);

INSERT INTO category (id, name, parent_id) VALUES 
(1, 'Clothing', NULL),
(2, 'Men', 1),
(3, 'Women', 1),
(4, 'Kids', 1);

INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES 
(1, 'T-Shirt', 'M', 15.00, 1, 2),
(2, 'Jeans', 'L', 40.00, 2, 2),
(3, 'Dress', 'S', 50.00, 3, 3),
(4, 'Skirt', 'XL', 30.00, 4, 3),
(5, 'Jacket', 'M', 60.00, 1, 2);

INSERT INTO clothing_order (id, customer_id, clothing_id, items, order_date) VALUES 
(1, 1, 1, 2, '2023-06-01'),
(2, 2, 2, 1, '2023-06-02'),
(3, 3, 3, 1, '2023-06-03'),
(4, 4, 4, 1, '2023-06-04');


#2. List All Clothing Items
WITH cu_favorite_color AS (
	SELECT cu.id, cu.first_name, cu.last_name, cl.name AS favorite_color
	FROM customer cu
	JOIN color cl ON cu.favorite_color_id = cl.id
)
SELECT c.name AS clothes, cl.name AS color, cu.last_name, cu.first_name, cfc.favorite_color
FROM clothing c
JOIN color cl ON c.color_id = cl.id
JOIN customer cu ON cu.favorite_color_id = cl.id
JOIN clothing_order co ON co.customer_id = cu.id AND co.clothing_id = c.id
JOIN cu_favorite_color cfc ON cfc.id = cu.id
ORDER BY cl.name


#3. Get All Non-Buying Customers
SELECT cu.last_name, cu.first_name, cl.name AS favorite_color
FROM customer cu
LEFT JOIN clothing_order co ON cu.id = co.customer_id
JOIN color cl ON cu.favorite_color_id = cl.id
WHERE co.id IS NULL;

#4. Select All Main Categories and Their Subcategories
SELECT c1.name AS category, c2.name AS subcategory
FROM category c1
LEFT JOIN category c2 ON c1.id = c2.parent_id
WHERE c1.parent_id IS NULL
