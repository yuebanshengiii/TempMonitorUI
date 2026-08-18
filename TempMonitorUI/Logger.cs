using System;
using System.IO;

namespace TempMonitorUI
{
    public static class Logger
        //static声明此类是静态类,可直接调用其中的方法不用创建实例
    {
        private static readonly object _lock = new object();
        //创建一个锁对象,确保多个线程不会同时进行某一段代码
        //也就是lock(_lock){这里面的代码}确保多个线程按顺序依次执行
        private static readonly string _logFile = "app_log.txt";
        //定义了一个私有的,属于类本身的,只读字符串片段,
        //readonly使其在初始化后,无法在运行时被重新赋值
        private static readonly long MaxLogFileSize = 5 * 1024 * 1024;
        public static void Write(string message)
        {
            string line = $"{DateTime.Now:HH:mm:ss} {message}";
            lock (_lock)
            {
                // 检查日志文件大小，如果超过阈值则轮转
                RotateLogFileIfNeeded();

                File.AppendAllText(_logFile, line + Environment.NewLine);
            }
        }
        private static void RotateLogFileIfNeeded()
        {
            if (!File.Exists(_logFile))
                return;

            FileInfo fi = new FileInfo(_logFile);
            if (fi.Length < MaxLogFileSize)
                return;

            // 轮转：重命名为带时间戳的文件
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string archiveFileName = $"app_log_{timestamp}.txt";
            File.Move(_logFile, archiveFileName);
        }
        public static void WriteError(string message, Exception ex = null)
        {
            string errorMsg = $"❌ {message}";
            if (ex != null)
            {
                errorMsg += $" | 异常: {ex.Message}";
            }
            Write(errorMsg);  // 复用 Write 方法
        }
    }
}