using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TempMonitorUI
{
    public class ScpiServer
    {
        private readonly TcpListener _listener;
        private readonly Func<float> _getTemperature;
        private bool _isRunning;

        public ScpiServer(Func<float> getTemperature, int port = 5021)
        {
            _getTemperature = getTemperature;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _isRunning = true;
            _listener.Start();
            Logger.Write($"✅ SCPI 服务器已启动 (端口 5021)");
            _ = Task.Run(() => AcceptClientsAsync());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            Logger.Write("⏹ SCPI 服务器已停止");
        }

        private async Task AcceptClientsAsync()
        {
            while (_isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
                catch { }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            using (var stream = client.GetStream())
            {
                var buffer = new byte[1024];
                var commandBuffer = new StringBuilder();
                try
                {
                    while (_isRunning)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        commandBuffer.Append(chunk);

                        // 检查是否收到了完整的命令（以 \n 结尾）
                        if (commandBuffer.ToString().Contains("\n"))
                        {
                            string fullCommand = commandBuffer.ToString().Trim();
                            commandBuffer.Clear();

                            Logger.Write($"📩 SCPI 收到: {fullCommand}");

                            string response = ParseCommand(fullCommand);
                            byte[] responseBytes = Encoding.ASCII.GetBytes(response + "\n");
                            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                            Logger.Write($"📤 SCPI 回复: {response}");
                        }
                    }
                }
                catch { }
            }
        }

        private string ParseCommand(string command)
        {
            // 移除末尾的 \r 或 \n
            command = command.Trim().ToUpper();

            // 解析 SCPI 命令
            if (command.StartsWith("MEASURE:VOLTAGE?"))
            {
                float temp = _getTemperature();
                return $"{temp:F3}";
            }

            if (command.StartsWith("*IDN?"))
            {
                return "TempMonitorUI,SCPI-Simulator,1.0,2026";
            }

            if (command.StartsWith("SYSTEM:STATUS?"))
            {
                return "0";
            }

            return "ERROR: UNKNOWN COMMAND";
        }
    }
}