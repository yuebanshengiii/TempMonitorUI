# 工业上位机监控系统 (Industrial Control System)

基于 C# WinForms 开发的工业设备上位机监控系统，集成 Modbus TCP 通信、SCPI 命令模拟、OpenCV 视觉定位、SQLite 数据存储和 WebAPI 接口，形成完整的"采集-控制-通信-展示"闭环。

---

## ✨ 功能特性

### 设备控制
- 温度实时监控与大字显示
- 目标温度设定与自动逼近控制
- 可配置超温报警阈值（界面动态调整）

### 通信协议
- **Modbus TCP 服务器**（端口 502）— 对外提供温度数据（寄存器 0-1）和视觉坐标（寄存器 2-5）
- **SCPI 命令模拟器**（端口 5021）— 支持 `MEASure:VOLTage?`、`*IDN?` 等标准 SCPI 指令
- **TCP/IP 自定义协议** — 长度头 + 数据体粘包/拆包处理

### 视觉定位
- 基于 OpenCVSharp 的模板匹配
- 匹配度高于 0.8 返回像素坐标
- 坐标通过 Modbus 寄存器对外下发

### 数据存储
- SQLite 历史数据持久化
- 最近 20 条记录快速查询

### 对外接口
- WebAPI：`GET /temperature` 返回 JSON 格式实时温度（含时间戳）
- SCPI 命令支持仪器模拟

### 日志系统
- 界面实时日志 + 文件日志同步写入
- 线程安全，支持自动截断

---

## 🛠️ 技术栈

| 类别 | 技术 |
| :--- | :--- |
| 语言与框架 | C#、.NET 8、WinForms |
| 通信协议 | TCP/IP、Modbus TCP、SCPI |
| 数据库 | SQLite |
| 计算机视觉 | OpenCVSharp |
| WebAPI | ASP.NET Core Minimal API |
| 混合编程 | C++ DLL（P/Invoke） |
| 版本控制 | Git、GitHub |

---

## 📁 项目结构

```
IndustrialControlSystem/
├── Form1.cs                 # 主窗体：UI 交互与事件绑定
├── Heater.cs               # 业务逻辑：温度模拟与控制
├── ModbusServer.cs         # 通信层：Modbus TCP 服务器
├── ScpiServer.cs           # 通信层：SCPI 命令模拟器
├── DbHelper.cs             # 数据层：SQLite 读写
├── Logger.cs               # 工具类：文件日志
├── MathLib/                # C++ DLL 源码（混合编程示例）
└── bin/Release/            # 可执行文件与依赖
```

---

## 🚀 快速开始

### 环境要求
- Windows 10/11
- .NET 8 SDK
- Visual Studio 2022（或 VSCode + C# 扩展）

### 运行步骤

1. **克隆仓库**
   ```bash
   git clone https://github.com/yuebanshengiii/IndustrialControlSystem.git
   cd IndustrialControlSystem
   ```

2. **打开解决方案**
   ```bash
   start IndustrialControlSystem.sln
   ```

3. **恢复 NuGet 包**
   ```bash
   dotnet restore
   ```

4. **运行程序**
   ```bash
   dotnet run --project IndustrialControlSystem.csproj
   ```

---

## 🧪 功能演示

### 温度监控与控制
- 点击"启动加热器"开始温度模拟
- 调整目标温度，系统自动逼近
- 修改报警阈值，超温时界面变红并记录日志

### Modbus 通信验证
- 启动程序后，Modbus TCP 服务器自动启动（端口 502）
- 使用 Modbus Poll 连接 `127.0.0.1:502`，读取保持寄存器：
  - 寄存器 0-1：温度值（Float，CD AB）
  - 寄存器 2-5：视觉坐标（Float，CD AB）

### SCPI 命令模拟
- 点击"启动 SCPI 服务器"（端口 5021）
- 使用 Telnet 连接：
  ```bash
  telnet 127.0.0.1 5021
  ```
- 支持命令：
  - `MEASure:VOLTage?` — 返回当前温度
  - `*IDN?` — 返回设备标识

### WebAPI 接口
- 启动程序后，WebAPI 服务自动启动（端口 5000）
- 浏览器访问 `http://localhost:5000/temperature` 返回 JSON：
  ```json
  {
    "temperature": 25.5,
    "unit": "℃",
    "timestamp": "2026-08-15 14:23:05"
  }
  ```

### 视觉定位
- 点击"视觉定位"按钮，选择包含模板的目标图片
- 程序返回匹配坐标和匹配度（>0.8 视为成功）
- 坐标自动写入 Modbus 寄存器 2-5

### C++ 混合编程演示
- 点击"测试 C++ DLL"按钮
- 调用 `MathLib.dll` 中的函数：
  - `ProcessTemperature(temp)` — 返回 `temp * 2 + 5`
  - `AddTwoIntegers(10, 20)` — 返回 30
  - `GetPi()` — 返回 3.141593

---

## 📊 系统架构

```
┌─────────────────────────────────────────────────────────────┐
│                      UI 层 (Form1)                        │
│               WinForms 界面 + 事件驱动交互                  │
└──────────────────────────┬──────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────────┐
│                    业务逻辑层 (Heater)                      │
│               温度模拟、目标控制、状态管理                    │
└──────────┬────────────────────────────┬────────────────────┘
           │                            │
┌──────────▼──────────────┐  ┌─────────▼────────────────────┐
│     通信层              │  │       数据层                  │
│  ModbusServer          │  │   DbHelper (SQLite)          │
│  ScpiServer            │  │   Logger (文件日志)           │
│  WebAPI (Program.cs)   │  │                              │
└─────────────────────────┘  └──────────────────────────────┘
```

---

## 📝 配置说明

所有硬编码参数集中管理，可通过修改代码中的常量字段进行调整：

| 参数 | 位置 | 默认值 |
| :--- | :--- | :--- |
| Modbus 端口 | `ModbusServer.cs` | 502 |
| SCPI 端口 | `ScpiServer.cs` | 5021 |
| WebAPI 端口 | `Program.cs` | 5000 |
| 超温阈值 | Form1 界面控件 | 80℃ |
| 日志最大行数 | `AppendLog()` | 100 行 |

---

## 📌 依赖项

- **NModbus** — Modbus TCP 协议实现
- **Microsoft.Data.Sqlite** — SQLite 数据库访问
- **OpenCvSharp4** — 计算机视觉处理
- **Microsoft.AspNetCore.App** — WebAPI 支持

---

## 📄 许可证

本项目仅供学习与面试作品展示使用。

---

## 📬 联系方式

如有问题或建议，欢迎通过 GitHub Issues 联系。

---

