using System.Drawing;
using System.Threading.Tasks;
using TempSimulator;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Runtime.InteropServices;

namespace TempMonitorUI
{
    public partial class Form1 : Form
    {
        private Heater _heater;
        private ModbusServer _modbusServer; // 新增：持有 ModbusServer 实例
        private ScpiServer _scpiServer;

        [DllImport("MathLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double ProcessTemperature(double input);

        [DllImport("MathLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddTwoIntegers(int a, int b);

        [DllImport("MathLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetPi();
        public Form1()
        {
            InitializeComponent();

        }
        private void LoadYoloCoordsAndSend()
        {
            string jsonPath = @"C:\aixuexi\yolo_coords.json";
            if (!File.Exists(jsonPath))
            {
                AppendLog("❌ 未找到 yolo_coords.json，请先运行 Python 脚本生成坐标");
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                var data = System.Text.Json.JsonSerializer.Deserialize<YoloResult>(jsonContent);

                if (data != null)
                {
                    float x = (float)data.x;
                    float y = (float)data.y;

                    _modbusServer?.UpdateVisionCoords(x, y);
                    AppendLog($"📤 YOLO 坐标已下发: X={x:F1}, Y={y:F1} ({data.name}, 置信度 {data.confidence:F2})");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"❌ 读取 JSON 失败: {ex.Message}");
            }
        }

        // 定义一个简单的类用于 JSON 反序列化
        private class YoloResult
        {
            public double x { get; set; }
            public double y { get; set; }
            public string name { get; set; }
            public double confidence { get; set; }
        }
        private void lblTemp_Click(object sender, EventArgs e)
        {
        }
        public Heater? GetHeater()
        {
            return _heater;
        }
        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = false;

            if (_heater == null)
            {
                _heater = new Heater();
                _heater.OnTemperatureChanged += OnTemperatureChangedHandler;

                _heater.SetTarget((float)numTargetTemp.Value);
            }

            _heater.Start();

            // 启动 Modbus 服务器（通过独立的 ModbusServer 类）
            if (_modbusServer == null)
            {
                _modbusServer = new ModbusServer(_heater);
                _modbusServer.Start();
            }

            numTargetTemp.Enabled = false;
            AppendLog("加热器已启动");
            RefreshLogBox();

            await Task.Delay(1000);
            btnStop.Enabled = true;
        }

        private void OnTemperatureChangedHandler(float temp)
        {
            this.Invoke(() =>
            {
                lblTemp.Text = $"当前：{temp:F1} ℃";
                float threshold = (float)numThreshold.Value;
                if (temp > threshold)
                {
                    lblTemp.ForeColor = Color.Red;
                    AppendLog($"🔥 高温警报！当前：{temp:F1}℃");
                    // 每次温度变化时，保存到数据库（后台线程执行）
                    DbHelper.Insert(temp);
                }
                else
                {
                    lblTemp.ForeColor = Color.Black;
                }

                float target = _heater?.GetTarget() ?? 25f;
                lblDeviationDisplay.Text = $"偏差：{temp - target:F1} ℃";
            });
        }

        private void AppendLog(string msg)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(() => AppendLog(msg));
                return;
            }
            rtbLog.AppendText($"{DateTime.Now:HH:mm:ss} {msg}{Environment.NewLine}");

            if (rtbLog.Lines.Length > 120)
            {
                var lines = rtbLog.Lines.Skip(rtbLog.Lines.Length - 100).ToArray();
                rtbLog.Text = string.Join(Environment.NewLine, lines);
            }
            rtbLog.ScrollToCaret();
            Logger.Write(msg);
        }

        private void rtbLog_TextChanged(object sender, EventArgs e) { }

        private void RefreshLogBox()
        {
            rtbLog.Hide();
            rtbLog.Show();
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = false;

            if (_heater != null)
            {
                _heater.Stop();
                AppendLog("加热器已停止");
                RefreshLogBox();
                numTargetTemp.Enabled = true;
            }
            else
            {
                AppendLog("加热器未启动");
            }

            // 停止 Modbus 服务器
            _modbusServer?.Stop();
            _modbusServer = null;

            await Task.Delay(1000);
            btnStart.Enabled = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void btnLoadHistory_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            AppendLog("📚 === 最近20条历史记录 ===");
            var records = DbHelper.LoadLastRecords(20);
            if (records.Count == 0)
            {
                AppendLog("暂无历史数据。");
                return;
            }
            foreach (var (time, value) in records)
            {
                rtbLog.AppendText($"{time}  →  {value:F1} ℃{Environment.NewLine}");
            }
            rtbLog.ScrollToCaret();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            numTargetTemp.ValueChanged += (s, ev) =>
            {
                if (_heater != null)
                    _heater.SetTarget((float)numTargetTemp.Value);
            };
        }

        private void btnVision_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 打开文件选择对话框，选择目标图片
                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "请选择要定位的图片";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string sourcePath = openFileDialog.FileName;
                string templatePath = "template.jpg"; // 模板图片放在程序目录下

                // 2. 检查模板文件是否存在
                if (!File.Exists(templatePath))
                {
                    AppendLog("❌ 模板文件 template.jpg 不存在，请放在程序目录下");
                    lblVisionResult.Text = "模板文件缺失";
                    return;
                }

                // 3. 加载图片并执行模板匹配
                using var source = Cv2.ImRead(sourcePath);
                using var template = Cv2.ImRead(templatePath);

                if (source.Empty() || template.Empty())
                {
                    AppendLog("❌ 图片加载失败");
                    lblVisionResult.Text = "图片加载失败";
                    return;
                }

                // 4. 模板匹配
                using var result = new Mat();
                Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);

                // 5. 找到最佳匹配位置
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                // 6. 判断匹配度阈值
                if (maxVal > 0.8)
                {
                    // 在源图上画出匹配位置
                    var rect = new OpenCvSharp.Rect(maxLoc.X, maxLoc.Y, template.Width, template.Height);
                    Cv2.Rectangle(source, rect, new Scalar(0, 0, 255), 2);

                    // 保存结果图片
                    string outputPath = "vision_result.jpg";
                    source.SaveImage(outputPath);

                    AppendLog($"✅ 匹配成功！位置: ({maxLoc.X}, {maxLoc.Y})，匹配度: {maxVal:F2}");
                    lblVisionResult.Text = $"坐标: ({maxLoc.X}, {maxLoc.Y})  匹配度: {maxVal:F2}";
                    // 更新 Modbus 寄存器中的视觉坐标
                    _modbusServer?.UpdateVisionCoords(maxLoc.X, maxLoc.Y);
                    AppendLog($"📤 视觉坐标已发送到 Modbus");
                }
                else
                {
                    AppendLog($"❌ 匹配失败，匹配度仅 {maxVal:F2}，请检查模板图片是否匹配");
                    lblVisionResult.Text = $"匹配失败 (匹配度 {maxVal:F2})";
                }
            }
            catch (Exception ex)
            {
                AppendLog($"❌ 视觉处理异常: {ex.Message}");
                lblVisionResult.Text = "处理异常";
            }
        }

        private void btnScpiServer_Click(object sender, EventArgs e)
        {
            if (_scpiServer == null)
            {
                _scpiServer = new ScpiServer(() => _heater?.GetCurrentTemp() ?? 0f);
                _scpiServer.Start();
                btnScpiServer.Text = "停止 SCPI 服务器";
            }
            else
            {
                _scpiServer.Stop();
                _scpiServer = null;
                btnScpiServer.Text = "启动 SCPI 服务器";
            }
        }

        private void btnTestCppDll_Click(object sender, EventArgs e)
        {
            try
            {
                double pi = GetPi();
                int sum = AddTwoIntegers(10, 20);
                double processedTemp = ProcessTemperature(_heater?.GetCurrentTemp() ?? 25.0);

                string msg = $"π = {pi:F6}\n" +
                             $"10 + 20 = {sum}\n" +
                             $"温度处理后 = {processedTemp:F2}";

                AppendLog($"✅ C++ DLL 调用成功: {msg.Replace("\n", " | ")}");
                MessageBox.Show(msg, "C++ DLL 测试结果");
            }
            catch (Exception ex)
            {
                AppendLog($"❌ DLL 调用失败: {ex.Message}");
                MessageBox.Show($"DLL 调用失败：{ex.Message}", "错误");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadYolo_Click(object sender, EventArgs e)
        {
            LoadYoloCoordsAndSend();
        }
    }
}