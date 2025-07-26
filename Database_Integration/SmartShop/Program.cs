using System;
using Microsoft.Data.Sqlite;

namespace SmartShop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data Source=smartshop.db"))
            {
                connection.Open();

                // 创建表
                var createTableCommand = connection.CreateCommand();
                createTableCommand.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS Products (
                        ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductName TEXT NOT NULL,
                        Category TEXT NOT NULL,
                        Price REAL NOT NULL,
                        StockLevel INTEGER NOT NULL
                    );
                ";
                createTableCommand.ExecuteNonQuery();

                // 插入示例数据
                InsertSampleData(connection);

                Console.WriteLine("数据库和表示例数据已创建。");
                Console.WriteLine("---------------------------------");

                // 1. 检索所有产品详细信息
                Console.WriteLine("所有产品:");
                ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products;");

                // 2. 按类别筛选产品
                Console.WriteLine("\n'Electronics' 类别下的产品:");
                ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products WHERE Category = 'Electronics';");

                // 3. 筛选库存不足的产品 (例如，少于 10 件)
                Console.WriteLine("\n库存不足的产品 (少于 10 件):");
                ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products WHERE StockLevel < 10;");

                // 4. 按价格升序排序
                Console.WriteLine("\n按价格升序排序的所有产品:");
                ExecuteQuery(connection, "SELECT ProductName, Category, Price, StockLevel FROM Products ORDER BY Price ASC;");
            }
        }

        static void InsertSampleData(SqliteConnection connection)
        {
            // 检查是否已有数据，避免重复插入
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(*) FROM Products;";
            long count = (long)checkCommand.ExecuteScalar();

            if (count == 0)
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText =
                @"
                    INSERT INTO Products (ProductName, Category, Price, StockLevel) VALUES
                    ('Laptop', 'Electronics', 1200.50, 15),
                    ('Smartphone', 'Electronics', 800.00, 25),
                    ('Coffee Maker', 'Home Appliances', 50.75, 8),
                    ('Desk Chair', 'Furniture', 150.00, 5),
                    ('T-shirt', 'Apparel', 25.99, 50);
                ";
                insertCommand.ExecuteNonQuery();
            }
        }

        static void ExecuteQuery(SqliteConnection connection, string query)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            using (var reader = command.ExecuteReader())
            {
                // 打印表头
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader.GetName(i),-20}");
                }
                Console.WriteLine();
                Console.WriteLine(new string('-', 80));

                // 打印数据行
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetValue(i),-20}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
