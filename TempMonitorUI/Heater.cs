using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempSimulator
//里面只能放类型定义(类,委托,接口,结构体,枚举等)
//只有using和顶级语句可以放在namespace外面
//顶级语句可以直接写逻辑,编译器会自动帮你生成namespace,class,Main
//所以一个文件只能有一个顶级语句块,而且用了顶级语句就不能自己写Main方法了
{
    public delegate void TemperatureChangedHandler(float temperature);
    //public访问修饰符,表示整个项目都可以访问
    //float参数类型,这里表示只能接受单精度浮点数作为参数
    //委托规定订阅者的方法长什么样子,可以定义在命名空间下
    public class Heater
    {

        public event TemperatureChangedHandler OnTemperatureChanged;
        //事件属于类的成员,得定义在类里面
        private float _currentTemp = 25f;
        //private访问修饰符,只有当前类内部可以访问
        //_是一种程序员之间的约定不是强制的,表示这是私有字段
        //25f声明25是float类型,是4字节浮点数,double是8字节浮点数,int是整数
        private float _targetTemp = 25f;
        private Random _rand = new Random();
        //Random _rand= new Random()创建一个随机生成数实例,并将这个示例存储在_rand这个字段中
        //字段是类的成员,用于存储数据,放在方法外部,而方法内部称为局部变量
        private CancellationTokenSource _cts; 


        public void SetTarget(float target)
        {
            _targetTemp = Math.Clamp(target, 0, 120);
        }
        public float GetTarget() => _targetTemp;
        public void Start()
        {
            Stop(); // 确保没有旧循环
            IsRunning = true;
            _cts = new CancellationTokenSource();
            Task.Run(() => RunLoop());
        }
        public void Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
            IsRunning = false;
        }
        private void RunLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {

                    // 计算温度变化：向目标靠近，并加入随机扰动
                    float diff = _targetTemp - _currentTemp;
                    float step = 0.5f; // 每步最大调节幅度

                    // 如果偏差较大，快速逼近；偏差小则慢速逼近
                    float adjust = Math.Clamp(diff, -step, step);
                    // 加入随机扰动（±0.5度）
                    float noise = (float)(_rand.NextDouble() * 1.0 - 0.5);
                    float delta = adjust + noise * 0.3f; // 扰动幅度较小

                    _currentTemp += delta;
                    // 限制范围
                    _currentTemp = Math.Clamp(_currentTemp, 0, 120);

                    // 触发事件（每次变化都触发）
                    OnTemperatureChanged?.Invoke(_currentTemp);

                    Thread.Sleep(1000);
                }
            }
            finally
            {
                IsRunning = false;   // 👈 确保循环退出后重置状态
            }
        }
        public float GetCurrentTemp() => _currentTemp;
        public bool IsRunning { get; private set; } = false;

    }
}
