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
