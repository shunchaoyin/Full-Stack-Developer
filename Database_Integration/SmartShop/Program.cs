using System;
using Microsoft.Data.Sqlite;

namespace SmartShop
{
    class Program
    {
        static void Main(string[] args)
        {
            // 使用文件数据库，以便数据持久化
            var connectionString = "Data Source=smartshop.db";
            // 每次运行时删除旧数据库，以确保数据是干净的
            if (System.IO.File.Exists("smartshop.db"))
            {
                System.IO.File.Delete("smartshop.db");
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                InitializeDatabase(connection);

                Console.WriteLine("✅ 数据库和表示例数据已成功创建。");
                Console.WriteLine(new string('=', 80));

                // --- Activity 1: Basic Queries ---
                Console.WriteLine("\n### Activity 1: Basic SQL Queries ###");
                RunActivity1Queries(connection);

                // --- Activity 2: Complex Queries ---
                Console.WriteLine("\n### Activity 2: Complex Queries with JOINs and Aggregation ###");
                RunActivity2Queries(connection);

                // --- Activity 3: Debugging and Optimization ---
                Console.WriteLine("\n### Activity 3: Debugging and Optimization ###");
                RunActivity3Tasks(connection);
            }
        }

        static void InitializeDatabase(SqliteConnection connection)
        {
            var createTablesCommand = connection.CreateCommand();
            createTablesCommand.CommandText =
            @"
                -- Products Table
                CREATE TABLE IF NOT EXISTS Products (
                    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductName TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Price REAL NOT NULL,
                    StockLevel INTEGER NOT NULL
                );

                -- Suppliers Table
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierId INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierName TEXT NOT NULL,
                    ContactInfo TEXT
                );

                -- Sales Table
                CREATE TABLE IF NOT EXISTS Sales (
                    SaleId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductId INTEGER,
                    StoreLocation TEXT NOT NULL,
                    SaleDate TEXT NOT NULL,
                    UnitsSold INTEGER NOT NULL,
                    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
                );

                -- Deliveries Table (for tracking supplier performance)
                CREATE TABLE IF NOT EXISTS Deliveries (
                    DeliveryId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductId INTEGER,
                    SupplierId INTEGER,
                    QuantityDelivered INTEGER NOT NULL,
                    ExpectedDate TEXT NOT NULL,
                    ActualDate TEXT NOT NULL,
                    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
                    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
                );
            ";
            createTablesCommand.ExecuteNonQuery();

            // Insert Sample Data
            var insertDataCommand = connection.CreateCommand();
            insertDataCommand.CommandText =
            @"
                -- Insert Products
                INSERT INTO Products (ProductName, Category, Price, StockLevel) VALUES
                ('Laptop Pro', 'Electronics', 1499.99, 15),
                ('Smartphone X', 'Electronics', 899.50, 25),
                ('Espresso Machine', 'Home Appliances', 199.00, 8),
                ('Ergonomic Chair', 'Furniture', 250.00, 5),
                ('Organic Cotton T-shirt', 'Apparel', 29.99, 50);

                -- Insert Suppliers
                INSERT INTO Suppliers (SupplierName, ContactInfo) VALUES
                ('Tech Imports Inc.', 'contact@techimports.com'),
                ('Appliance Masters', 'sales@appliancemasters.net'),
                ('Fashion Forward', 'support@fashionforward.co');

                -- Insert Sales
                INSERT INTO Sales (ProductId, StoreLocation, SaleDate, UnitsSold) VALUES
                (1, 'Downtown', '2025-07-20', 1),
                (2, 'Uptown', '2025-07-21', 2),
                (4, 'Downtown', '2025-07-22', 1),
                (1, 'Online', '2025-07-23', 3);

                -- Insert Deliveries
                INSERT INTO Deliveries (ProductId, SupplierId, QuantityDelivered, ExpectedDate, ActualDate) VALUES
                (1, 1, 20, '2025-07-10', '2025-07-12'), -- 2 days delay
                (2, 1, 30, '2025-07-11', '2025-07-11'), -- On time
                (3, 2, 15, '2025-07-15', '2025-07-18'), -- 3 days delay
                (5, 3, 100, '2025-07-18', '2025-07-18'); -- On time
            ";
            insertDataCommand.ExecuteNonQuery();
        }

        static void RunActivity1Queries(SqliteConnection connection)
        {
            // 1. 检索所有产品详细信息
            Console.WriteLine("\n1. 所有产品详细信息:");
            ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products;");

            // 2. 按类别筛选产品
            Console.WriteLine("\n2. 'Electronics' 类别下的产品:");
            ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products WHERE Category = 'Electronics';");

            // 3. 筛选库存不足的产品 (例如，少于 10 件)
            Console.WriteLine("\n3. 库存不足的产品 (少于 10 件):");
            ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products WHERE StockLevel < 10;");

            // 4. 按价格升序排序
            Console.WriteLine("\n4. 按价格升序排序的所有产品:");
            ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products ORDER BY Price ASC;");
        }

        static void RunActivity2Queries(SqliteConnection connection)
        {
            // 1. 连接 Products 和 Sales 表
            Console.WriteLine("\n1. 产品销售报告 (连接查询):");
            ExecuteQuery(connection, @"
                SELECT
                    p.ProductName,
                    s.SaleDate,
                    s.StoreLocation,
                    s.UnitsSold
                FROM Sales s
                JOIN Products p ON s.ProductId = p.ProductId;
            ");

            // 2. 计算每个产品的总销售量 (聚合查询)
            Console.WriteLine("\n2. 每个产品的总销售量 (聚合查询):");
            ExecuteQuery(connection, @"
                SELECT
                    p.ProductName,
                    SUM(s.UnitsSold) AS TotalUnitsSold
                FROM Sales s
                JOIN Products p ON s.ProductId = p.ProductId
                GROUP BY p.ProductName
                ORDER BY TotalUnitsSold DESC;
            ");

            // 3. 识别交货延迟最多的供应商 (子查询)
            Console.WriteLine("\n3. 供应商交货表现 (子查询):");
            ExecuteQuery(connection, @"
                SELECT
                    s.SupplierName,
                    AVG(JULIANDAY(d.ActualDate) - JULIANDAY(d.ExpectedDate)) AS AverageDelayInDays
                FROM Deliveries d
                JOIN Suppliers s ON d.SupplierId = s.SupplierId
                GROUP BY s.SupplierName
                ORDER BY AverageDelayInDays DESC;
            ");
        }

        static void RunActivity3Tasks(SqliteConnection connection)
        {
            Console.WriteLine("\n--- 演示查询优化 ---");
            string complexQuery = @"
                SELECT
                    s.SupplierName,
                    AVG(JULIANDAY(d.ActualDate) - JULIANDAY(d.ExpectedDate)) AS AverageDelayInDays
                FROM Deliveries d
                JOIN Suppliers s ON d.SupplierId = s.SupplierId
                WHERE p.Category = 'Electronics'
                GROUP BY s.SupplierName
                ORDER BY AverageDelayInDays DESC;
            ";

            // 为了让查询更复杂以显示优化效果，我们稍微修改一下，加入 Products 表
            string queryToOptimize = @"
                SELECT
                    s.SupplierName,
                    AVG(JULIANDAY(d.ActualDate) - JULIANDAY(d.ExpectedDate)) AS AverageDelayInDays
                FROM Deliveries d
                JOIN Suppliers s ON d.SupplierId = s.SupplierId
                JOIN Products p ON d.ProductId = p.ProductId
                WHERE p.Category = 'Electronics'
                GROUP BY s.SupplierName
                ORDER BY AverageDelayInDays DESC;
            ";

            Console.WriteLine("\n1. 运行未经优化的查询...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            ExecuteQuery(connection, queryToOptimize, "未经优化的查询");
            stopwatch.Stop();
            Console.WriteLine($"⏱️  执行时间: {stopwatch.Elapsed.TotalMilliseconds} ms");

            Console.WriteLine("\n2. 正在创建索引以进行优化...");
            CreateIndexes(connection);
            Console.WriteLine("✅ 索引已成功创建。");

            Console.WriteLine("\n3. 运行优化后的查询...");
            stopwatch.Restart();
            ExecuteQuery(connection, queryToOptimize, "优化后的查询");
            stopwatch.Stop();
            Console.WriteLine($"⏱️  执行时间: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine("\n💡 注意: 在大型数据库中，索引对性能的提升会更加显著。");
        }

        static void CreateIndexes(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE INDEX IF NOT EXISTS idx_sales_product_id ON Sales(ProductId);
                CREATE INDEX IF NOT EXISTS idx_deliveries_product_id ON Deliveries(ProductId);
                CREATE INDEX IF NOT EXISTS idx_deliveries_supplier_id ON Deliveries(SupplierId);
                CREATE INDEX IF NOT EXISTS idx_products_category ON Products(Category);
            ";
            command.ExecuteNonQuery();
        }

        static void ExecuteQuery(SqliteConnection connection, string query, string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                Console.WriteLine($"--- {title} ---");
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("未找到结果。");
                        return;
                    }

                    // 打印表头
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i),-25}");
                    }
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 25 * reader.FieldCount));

                    // 打印数据行
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetValue(i),-25}");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
