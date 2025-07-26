# SmartShop Inventory System

---

## Activity 1: Basic SQL Queries

### 🎯 Step 1: Review the Scenario

To begin, review the following scenario for the "SmartShop Inventory System."

> Imagine you are a database engineer tasked with developing the SmartShop Inventory System for a fictional retail company, SmartShop. This system must manage inventory data across multiple stores, providing real-time insights into stock levels, sales trends, and supplier information.
>
> The company requires:
>
> - A database to store and retrieve inventory data efficiently.
> - Complex queries to analyze trends and relationships between products, sales, and suppliers.
> - Optimized database operations to ensure high performance and scalability.
>
> Your goal is to leverage Microsoft Copilot to create, debug, and optimize SQL queries, ensuring the system meets performance and accuracy requirements. This project will span three activities and culminate in a comprehensive inventory management database.

#### Initial Requirements

For this first activity, SmartShop’s initial requirements include:

- Retrieving product details such as name, price, and stock levels.
- Filtering products based on categories and availability.
- Sorting data for better readability.

---

### ✍️ Step 2: Generate Basic SELECT Queries

To get started, use Copilot to generate basic queries to meet these needs.

Write a query to retrieve the following product details:

- `ProductName`
- `Category`
- `Price`
- `StockLevel`

---

### 🔍 Step 3: Implement Filtering and Sorting

Next, work on filtering and sorting capabilities. Write queries with Copilot to perform the following actions:

- **Filter** products in a specific category.
- **Filter** products with low stock levels.
- **Sort** the results to display products by `Price` in ascending order.

---

### 💾 Step 4: Save Your Work for Activity 1

By the end of this activity, you will have:

- Written basic SQL queries to retrieve and organize inventory data.
- Created queries that are prepared for extension and refinement in Activity 2.

Save all queries in your sandbox environment.

---

## Activity 2: Complex Queries with JOINs and Aggregation

### 🎯 Step 1: Review Recap and New Requirements

In Activity 1, you wrote basic SQL queries to retrieve and filter inventory data. Now, SmartShop has additional requirements for more complex analysis.

> #### New Requirements
>
> - Analyzing sales trends by joining product and sales data.
> - Generating reports on supplier performance using aggregate functions.
> - Combining data from multiple tables to track inventory levels across stores.
>
> SmartShop’s new needs include:
>
> - Tracking product sales by date and store.
> - Identifying top-performing suppliers based on delivered stock.
> - Combining inventory data across stores for consolidated reporting.
>
> *For this activity, you will not need the actual data in the table. However, you should apply your understanding of queries and the structure of the tables in our scenario to execute the queries.*

---

### ✍️ Step 2: Write Multi-Table JOIN Queries

To get started, use Copilot to generate multi-table `JOIN` queries.

- Join the `Products`, `Sales`, and `Suppliers` tables.
- Write a query to display `ProductName`, `SaleDate`, `StoreLocation`, and `UnitsSold`.

---

### 🔍 Step 3: Implement Nested Queries and Aggregation

Next, implement nested queries and aggregation.

Write subqueries with Copilot to:

- Calculate total sales for each product.
- Identify suppliers with the most delayed deliveries.
- Use aggregate functions (e.g., `SUM`, `AVG`, `MAX`) to analyze trends and summarize data.

---

### 💾 Step 4: Save Your Work for Activity 2

By the end of this activity, you will have:

- Generated complex SQL queries to meet SmartShop’s advanced requirements.
- A set of queries ready for debugging and optimization in Activity 3.

Save all complex queries in your sandbox environment.