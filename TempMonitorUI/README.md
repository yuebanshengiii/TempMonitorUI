# 工控上位机监控系统

基于 C# WinForms 开发的工业上位机监控系统，集成 Modbus TCP 通信、SCPI 命令模拟、OpenCV 视觉定位、YOLO 目标检测和本地大模型调用，形成完整的“采集-控制-通信-展示-智能辅助”闭环。

## ✨ 功能特性

### 设备控制
- 实时温度监控与大字显示
- 目标温度设定与自动逼近控制
- 可配置超温报警阈值（界面动态调整）

### 通信协议
- **Modbus TCP 服务器**（端口 502）— 温度数据（寄存器 0-1）和视觉坐标（寄存器 2-5）对外暴露
- **SCPI 命令模拟器**（端口 5021）— 支持 `MEASure:VOLTage?`、`*IDN?` 等标准 SCPI 指令
- **TCP/IP 自定义协议** — 长度头 + 数据体粘包/拆包处理

### 计算机视觉
- **OpenCV 模板匹配**：匹配度 > 0.8 返回像素坐标，自动写入 Modbus 寄存器
- **YOLO 目标检测**：
  - 支持自定义数据集训练（杯子检测模型，mAP50 达 0.99）
  - 训练好的模型导出为 ONNX 格式，集成到 C# 上位机
  - 检测坐标（类别 + 置信度）实时显示，坐标自动写入 Modbus 寄存器
  - 推理速度约 6.9ms，可满足实时检测需求

### 本地大模型调用（新增）
- 集成 Ollama 服务，在上位机中通过 HTTP API 调用本地大模型（Qwen-Coder）
- 支持智能问答、代码生成等辅助功能
- API 地址通过 `appsettings.json` 配置管理，支持不同环境灵活切换
- 异步请求处理，界面不卡顿

### 对外接口
- WebAPI：`GET /temperature` 返回 JSON 格式实时温度（含时间戳）
- WebAPI：`GET /yolo` 返回 YOLO 检测结果 JSON

### 数据持久化
- SQLite 历史数据存储与最近 20 条记录查询
- 线程安全日志系统（界面显示 + 文件同步写入），支持日志自动轮转

### 部署特性
- 支持通过 `appsettings.json` 配置文件修改端口、阈值和 Ollama 地址，无需重新编译
- 全局异常捕获，程序不会因未处理异常而直接崩溃
- 项目已发布可运行版本，支持直接下载使用

## 🛠️ 技术栈

| 类别 | 技术 |
| :--- | :--- |
| 语言与框架 | C#、.NET 8、WinForms |
| 工业通信 | TCP/IP、Modbus TCP、SCPI |
| 计算机视觉 | OpenCVSharp、YOLO、ONNX |
| AI 模型训练 | PyTorch、Ultralytics |
| 本地大模型 | Ollama、Qwen-Coder |
| 数据库 | SQLite |
| WebAPI | ASP.NET Core Minimal API |
| 混合编程 | C++ DLL（P/Invoke） |
| 版本控制 | Git、GitHub |

## 📁 项目结构

```
IndustrialControlSystem/
├── Form1.cs                 # 主窗体
├── Heater.cs               # 温度模拟与控制
├── ModbusServer.cs         # Modbus TCP 服务器
├── ScpiServer.cs           # SCPI 命令模拟器
├── DbHelper.cs             # SQLite 数据操作
├── Logger.cs               # 文件日志
├── MathLib/                # C++ DLL 源码
├── appsettings.json        # 配置文件（端口、阈值、Ollama 地址）
├── cup_dataset.yaml        # YOLO 数据集配置文件
├── best.onnx               # YOLO 训练模型（ONNX 格式）
└── bin/Release/            # 可执行文件
```

## 🚀 快速开始

### 运行环境
- Windows 10/11
- .NET 8 桌面运行时
- （可选）Ollama 服务（用于大模型调用功能）

### 下载运行
1. 访问 [Releases](https://github.com/yuebanshengii/TempMonitorUI/releases) 页面
2. 下载最新版本的 `TempMonitorUI_v1.0.zip`
3. 解压后双击 `TempMonitorUI.exe` 即可运行

### YOLO 模型配置
1. 将训练好的 `best.onnx` 文件放入程序运行目录
2. 点击"YOLO 检测"按钮，选择图片即可进行检测
3. 检测结果（坐标 + 置信度）会显示在日志中，坐标自动写入 Modbus 寄存器

> **训练自己的 YOLO 模型**：使用 Ultralytics 框架训练自定义数据集，导出为 ONNX 格式后替换 `best.onnx` 文件即可。

### 本地大模型配置（可选）
1. 安装并启动 [Ollama](https://ollama.com)
2. 下载模型：`ollama run qwen-coder`（或你选择的其它模型）
3. 在 `appsettings.json` 中配置 Ollama 地址：
   ```json
   {
     "Ollama": {
       "Url": "http://localhost:11434/api/generate"
     }
   }
   ```
4. 运行程序，点击"问 AI"按钮即可使用

### 配置文件
运行前可通过修改 `appsettings.json` 调整参数：

```json
{
  "Modbus": { "Port": 502 },
  "WebApi": { "Port": 5000 },
  "Scpi": { "Port": 5021 },
  "Threshold": 80,
  "Ollama": {
    "Url": "http://localhost:11434/api/generate"
  }
}
```

## 📸 功能演示

![主界面](TempMonitorUI/docs/screenshot.png)

