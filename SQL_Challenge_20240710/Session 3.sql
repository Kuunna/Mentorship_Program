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

--2. List the Top 3 Most Expensive Orders

SELECT TOP 3 order_id, customer_id, total_amount
FROM orders
ORDER BY total_amount DESC

--3. Compute Deltas Between Consecutive Orders

--4. Compute the Running Total of Purchases per Customer


