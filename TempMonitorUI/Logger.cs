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

        public static void Write(string message)
        {
            string line = $"{DateTime.Now:HH:mm:ss} {message}";
            //定义写入内容为 日期 传入参数 的格式
            //局部变量,多个线程访问时,并不会同时赋值line,每个线程都有自己独立的line
            //一般在方法体内部,不是类的成员,没有访问修饰符
            lock (_lock)
            {
                File.AppendAllText(_logFile, line + Environment.NewLine);
                //File.AppendAllText接收两个参数,文本路径和字符串
                //作用是打开文本路径对应的文本,如果文本不存在会自动创建,并且将字符串加在最后面
                //Environment.NewLine表示当前系统的换行符
            }
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