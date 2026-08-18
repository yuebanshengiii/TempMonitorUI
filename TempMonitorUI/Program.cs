using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace TempMonitorUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                string errorMsg = $"程序发生未处理异常：{e.ExceptionObject.ToString()}";
                Logger.WriteError(errorMsg);
                MessageBox.Show($"程序发生错误，请查看日志文件 app_log.txt 了解详情。\n\n{errorMsg}", "程序错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            ApplicationConfiguration.Initialize();

            // 1. 先创建主窗体实例，以便获取 Heater 引用
            var mainForm = new Form1();

            // 2. 在后台启动 WebAPI 服务器（不阻塞 UI 线程）
            _ = Task.Run(() => StartWebApi(mainForm));

            // 3. 运行主窗体（阻塞 UI 线程，直到窗体关闭）
            Application.Run(mainForm);
        }

        static void StartWebApi(Form1 form)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapGet("/temperature", async (HttpContext ctx) =>
            {
                var heater = form.GetHeater();
                if (heater == null)
                    return Results.Json(new { error = "Heater not initialized" }, statusCode: 503);

                return Results.Json(new
                {
                    temperature = heater.GetCurrentTemp(),
                    unit = "℃",
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            });
            app.MapGet("/yolo", () =>
            {
                string jsonPath = @"C:\aixuexi\yolo_coords.json";
                if (!File.Exists(jsonPath))
                    return Results.NotFound(new { error = "No YOLO result available. Please run yolo_to_json.py first." });

                string json = File.ReadAllText(jsonPath);
                // 直接返回 JSON 内容（已经是 JSON 格式）
                return Results.Json(json);
            });
            var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
            int webApiPort = config.GetValue<int>("WebApi:Port", 5000);
            app.Run($"http://localhost:{webApiPort}");
        }
    }
}