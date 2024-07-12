# 📕 SQL Challenge - Session 3
<p align="center">
<img src="https://github.com/Kuunna/Mentorship_Program/assets/85633982/65db8e25-7497-4232-86fd-e7ba48a7117b" align="center" width="1000" height="750" >

## 🚀 Table of Contents
**1. Create table**
```TSQL
CREATE TABLE customers (
    customer_id INT PRIMARY KEY,
    email VARCHAR(255) NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    address VARCHAR(255),
    city VARCHAR(100),
    region VARCHAR(100),
    postal_code VARCHAR(20),
    country VARCHAR(100),
    phone VARCHAR(20),
    registration_date DATE NOT NULL,
    channel_id INT,
    first_order_id INT,
    first_order_date DATE,
    last_order_id INT,
    last_order_date DATE
);

CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    customer_id INT NOT NULL,
    order_date DATE NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    ship_name VARCHAR(255),
    ship_address VARCHAR(255),
    ship_city VARCHAR(100),
    ship_region VARCHAR(100),
    ship_postalcode VARCHAR(20),
    ship_country VARCHAR(100),
    shipped_date DATE
);

CREATE TABLE products (
    product_id INT PRIMARY KEY,
    product_name VARCHAR(255) NOT NULL,
    category_id INT,
    unit_price DECIMAL(10, 2) NOT NULL,
    discontinued BIT NOT NULL
);

CREATE TABLE categories (
    category_id INT PRIMARY KEY,
    category_name VARCHAR(255) NOT NULL,
    description TEXT
);

CREATE TABLE order_items (
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    quantity INT NOT NULL,
    discount DECIMAL(5, 2)
);

CREATE TABLE channels (
    id INT PRIMARY KEY,
    channel_name VARCHAR(255) NOT NULL
);

INSERT INTO customers (customer_id, email, full_name, address, city, region, postal_code, country, phone, registration_date, channel_id, first_order_id, first_order_date, last_order_id, last_order_date) VALUES
(1, 'alice@example.com', 'Alice Johnson', '123 Main St', 'New York', 'NY', '10001', 'USA', '555-1234', '2022-01-15', 1, 1, '2022-02-10', 3, '2023-03-20'),
(2, 'bob@example.com', 'Bob Smith', '456 Elm St', 'Los Angeles', 'CA', '90001', 'USA', '555-5678', '2022-03-22', 2, 2, '2022-04-15', 4, '2023-05-30'),
(3, 'charlie@example.com', 'Charlie Brown', '789 Oak St', 'Chicago', 'IL', '60601', 'USA', '555-8765', '2022-05-05', 3, 3, '2022-06-10', 5, '2023-06-25');

INSERT INTO orders (order_id, customer_id, order_date, total_amount, ship_name, ship_address, ship_city, ship_region, ship_postalcode, ship_country, shipped_date) VALUES
(1, 1, '2022-02-10', 719.98, 'Alice Johnson', '123 Main St', 'New York', 'NY', '10001', 'USA', '2022-02-12'),
(2, 2, '2022-04-15', 1214.98, 'Bob Smith', '456 Elm St', 'Los Angeles', 'CA', '90001', 'USA', '2022-04-18'),
(3, 1, '2023-03-20', 29.98, 'Alice Johnson', '123 Main St', 'New York', 'NY', '10001', 'USA', '2023-03-22'),
(4, 2, '2023-05-30', 69.98, 'Bob Smith', '456 Elm St', 'Los Angeles', 'CA', '90001', 'USA', '2023-06-01'),
(5, 3, '2023-06-25', 104.98, 'Charlie Brown', '789 Oak St', 'Chicago', 'IL', '60601', 'USA', '2023-06-28');

INSERT INTO products (product_id, product_name, category_id, unit_price, discontinued) VALUES
(1, 'Smartphone', 1, 699.99, 0),
(2, 'Laptop', 1, 1199.99, 0),
(3, 'Fiction Book', 2, 14.99, 0),
(4, 'T-shirt', 3, 19.99, 0),
(5, 'Blender', 4, 49.99, 0),
(6, 'Tennis Racket', 5, 89.99, 0);

INSERT INTO categories (category_id, category_name, description) VALUES
(1, 'Electronics', 'Devices and gadgets'),
(2, 'Books', 'Various kinds of books'),
(3, 'Clothing', 'Apparel and accessories'),
(4, 'Home & Kitchen', 'Household and kitchen items'),
(5, 'Sports', 'Sports equipment and apparel');

INSERT INTO order_items (order_id, product_id, unit_price, quantity, discount) VALUES
(1, 1, 699.99, 1, 0.00),
(1, 3, 14.99, 1, 0.00),
(2, 2, 1199.99, 1, 0.00),
(2, 4, 19.99, 1, 0.00),
(3, 3, 14.99, 2, 0.10),
(4, 4, 19.99, 2, 0.10),
(5, 5, 49.99, 2, 0.05);

INSERT INTO channels (id, channel_name) VALUES
(1, 'Google Ads'),
(2, 'Facebook Ads'),
(3, 'Instagram'),
(4, 'Organic Search'),
(5, 'Referral');
```

## ❓ Case Study Questions
**2. List the Top 3 Most Expensive Orders**
```TSQL
SELECT TOP 3 order_id, customer_id, total_amount
FROM orders
ORDER BY total_amount DESC
```
![Screenshot 2024-07-12 100232](https://github.com/user-attachments/assets/13652dfd-e2e9-45e3-a52d-d3e4214df087)

**3. Compute Deltas Between Consecutive Orders**
```TSQL
WITH OrderRanks AS (
    SELECT
        customer_id,
        order_id,
        total_amount,
        order_date,
        ROW_NUMBER() OVER (PARTITION BY customer_id ORDER BY order_date) AS order_rank
    FROM orders
),
OrderDeltas AS (
    SELECT
        o1.order_id,
        o1.customer_id,
        o1.total_amount,
        o2.total_amount AS previous_value,
        o1.total_amount - o2.total_amount AS delta
    FROM OrderRanks o1
    LEFT JOIN OrderRanks o2
    ON o1.customer_id = o2.customer_id AND o1.order_rank = o2.order_rank + 1
)
SELECT *
FROM OrderDeltas
ORDER BY customer_id, order_id
```
![Screenshot 2024-07-12 100252](https://github.com/user-attachments/assets/b608c61f-5a9b-44b2-8728-4add0dcc7ed0)

**4. Compute the Running Total of Purchases per Customer**
```TSQL
WITH RunningTotals AS (
    SELECT
        o.customer_id,
        c.full_name,
        o.order_id,
        o.order_date,
        o.total_amount,
        SUM(o.total_amount) OVER (PARTITION BY o.customer_id ORDER BY o.order_date ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS running_total
    FROM orders o
    JOIN customers c ON o.customer_id = c.customer_id
)
SELECT *
FROM RunningTotals
ORDER BY customer_id, order_date
```
![Screenshot 2024-07-12 100307](https://github.com/user-attachments/assets/704fb60f-0c90-44aa-a8e6-e33e76b45f5e)

