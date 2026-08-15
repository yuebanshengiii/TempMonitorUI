using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace TempMonitorUI
{
    public static class DbHelper
    {
        private const string ConnectionString = "Data Source=history.db";
        static DbHelper()
        {
            Initialize();
        }
        // 初始化：创建表（如果不存在）
        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS TemperatureRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Value REAL NOT NULL,
                    RecordTime TEXT NOT NULL
                )";
            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        // 插入一条记录
        public static void Insert(float temp)
        {
            // 用 Task.Run 让数据库写入在后台线程执行，不阻塞 UI
            _ = Task.Run(() =>
            {
                try
                {
                    using var connection = new SqliteConnection(ConnectionString);
                    connection.Open();

                    string sql = "INSERT INTO TemperatureRecords (Value, RecordTime) VALUES (@value, @time)";
                    using var command = new SqliteCommand(sql, connection);
                    command.Parameters.AddWithValue("@value", temp);
                    command.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // 记录错误（但不要影响主流程）
                    Logger.WriteError($"数据库写入失败: {ex.Message}");
                }
            });
        }

        // 查询最近 N 条记录
        public static List<(string Time, float Value)> LoadLastRecords(int count = 20)
        {
            var list = new List<(string, float)>();
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                string sql = "SELECT RecordTime, Value FROM TemperatureRecords ORDER BY Id DESC LIMIT @count";
                using var command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@count", count);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string time = reader.GetString(0);
                    float value = reader.GetFloat(1);
                    list.Add((time, value));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"读取历史数据失败: {ex.Message}");
            }
            return list;
        }
    }
}