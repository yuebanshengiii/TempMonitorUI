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
        private Random _rand = new Random();
        //Random _rand= new Random()创建一个随机生成数实例,并将这个示例存储在_rand这个字段中
        //字段是类的成员,用于存储数据,放在方法外部,而方法内部称为局部变量
        public void Start()
        //无返回值要写void,C#的语法要求,其中()是方法的标志,可以传入参数可以不传入
        {
            while (true)
            {
                //无限循环
                float delta = (float)(_rand.NextDouble() * 13 - 5);
                //_rand.NextDouble()其中_rand是实例一个随机数,NextDouble()调用实例里面的成员,随机0到1的小数
                //因为返回的是double类型8字节浮点数,所以(float)将此数改为4字节浮点数
                _currentTemp += delta;
                if (_currentTemp < 0) _currentTemp = 0;
                if (_currentTemp > 120) _currentTemp = 120;

                if (_currentTemp > 80)
                {
                    OnTemperatureChanged?.Invoke(_currentTemp);
                    //其中OnTemperatureChanged是事件,而?.表示如果时间没有被订阅则不要Invoke(_currentTemp)
                    //如果有则触发Invoke(_currentTemp)把_currentTemp传给每个订阅者
                }
                Thread.Sleep(1000);
                //当前线程暂定1秒
            }

        }
    }
}
