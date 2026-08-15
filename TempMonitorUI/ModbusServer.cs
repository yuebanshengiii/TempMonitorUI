using NModbus;
using NModbus.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TempSimulator;

namespace TempMonitorUI
{
    public class ModbusServer : IDisposable
    {
        private IModbusSlave _modbusSlave;
        private TcpListener _tcpListener;
        private IModbusSlaveNetwork _modbusNetwork;
        private readonly Heater _heater;
        // 新增字段：存储视觉坐标
        private float _visionX = 0;
        private float _visionY = 0; 
        // 新增方法：更新视觉坐标
        public void UpdateVisionCoords(float x, float y)
        {
            _visionX = x;
            _visionY = y;
            // 写日志
            Logger.Write($"📤 视觉坐标已更新: X={x}, Y={y}");
        }

        public ModbusServer(Heater heater)
        {
            _heater = heater;
        }

        public void Start()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, 502);
                _tcpListener.Start();

                var factory = new ModbusFactory();
                IModbusSlave slave = factory.CreateSlave(1);
                _modbusSlave = slave;

                if (slave.DataStore is DefaultSlaveDataStore dataStore)
                {
                    dataStore.HoldingRegisters.WritePoints(0, new ushort[] { 0, 0 });
                }

                _modbusNetwork = factory.CreateSlaveNetwork(_tcpListener);
                _modbusNetwork.AddSlave(slave);
                _ = Task.Run(() => _modbusNetwork.ListenAsync());

                _ = Task.Run(() => UpdateRegistersAsync());

                Logger.Write("✅ Modbus TCP 服务器已启动 (端口 502)");
            }
            catch (Exception ex)
            {
                Logger.WriteError($"❌ Modbus 服务器启动失败: {ex.Message}");
            }
        }

        public void Stop()
        {
            _modbusNetwork?.Dispose();
            _tcpListener?.Stop();
            _modbusNetwork = null;
            _tcpListener = null;
            _modbusSlave = null;
        }

        public void Dispose()
        {
            Stop();
        }

        private async Task UpdateRegistersAsync()
        {
            while (_heater != null && _heater.IsRunning)
            {
                try
                {
                    float temp = _heater.GetCurrentTemp();
                    byte[] bytes = BitConverter.GetBytes(temp);
                    ushort low = BitConverter.ToUInt16(bytes, 0);
                    ushort high = BitConverter.ToUInt16(bytes, 2);
                    // 视觉 X → 寄存器 2
                    byte[] bytesX = BitConverter.GetBytes(_visionX);
                    ushort lowX = BitConverter.ToUInt16(bytesX, 0);
                    ushort highX = BitConverter.ToUInt16(bytesX, 2);
                    // 视觉 Y → 寄存器 4,5
                    byte[] bytesY = BitConverter.GetBytes(_visionY);
                    ushort lowY = BitConverter.ToUInt16(bytesY, 0);
                    ushort highY = BitConverter.ToUInt16(bytesY, 2);
                    if (_modbusSlave != null && _modbusSlave.DataStore is DefaultSlaveDataStore dataStore)
                    {
                        dataStore.HoldingRegisters.WritePoints(0, new ushort[] { low, high });
                        // 写入 X 到寄存器 2,3
                        dataStore.HoldingRegisters.WritePoints(2, new ushort[] { lowX, highX });
                        // 写入 Y 到寄存器 4,5
                        dataStore.HoldingRegisters.WritePoints(4, new ushort[] { lowY, highY });
                    }
                }
                catch { }
                await Task.Delay(500);
            }
        }
    }
}