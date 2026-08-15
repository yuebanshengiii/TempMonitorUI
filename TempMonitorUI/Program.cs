using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Http;
namespace TempMonitorUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
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

            app.Run("http://localhost:5000");
        }
    }
}