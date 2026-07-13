using TempSimulator;
using System.Threading.Tasks;
using System.Drawing;


namespace TempMonitorUI
{
    public partial class Form1 : Form
    //Form是基类,实现了一些基础功能
    //:(冒号)表示Form1这个类继承Form类,就是我可以使用你的基础功能,然后我在Form1这个类里面自己写一些附属
    //partial表示这个类Form1可以存在于多个文件中编辑,不会造成同名冲突,都是指Form1这个类,
    //你可以移步到Form1.Designer.cs这个文件中查看
    {
        public Form1()
        //没有声明返回类型,是构造函数并不是方法,构造函数必须和类名相同
        //在创建类的实例时自动调用,也就是new Form1()时,自动执行其中的逻辑
        {
            InitializeComponent();
            //是WinForms设计器自动生成的一个方法,
            //该方法属于Form1类,被定义在Form1.Designer.cs文件中,你可以移步查看
            //在你创建控件,编辑控件的时候会自动编写InitializeComponent()方法
        }

        private void lblTemp_Click(object sender, EventArgs e)
        {

        }

        private Heater _heater;
        //声明有一个Heater的实例名字叫_heater只能在类方法内部访问,该实例还未创建,只是预先声明

        private void btnStart_Click(object sender, EventArgs e)
        {
            _heater = new Heater();
            //创建Heater实例
            _heater.OnTemperatureChanged += OnTemperatureChangedHandler;
            //左边OnTemperatureChanged是类Heater中定义的事件,可以移步文件Heater.cs查看
            //右边将OnTemperatureChangedHandler方法,订阅这个事件,也就是添加到这个事件的调用列表中
            //当事件中的代码运行到OnTemperatureChanged?.Invoke(_currentTemp);时
            //则会运行OnTemperatureChangedHandler方法

            //虽然此方法在下面代码才开始定义,但是提前订阅是没有问题的
            //程序在运行时会先扫描类里面的所有成员,此时他就会扫描到OnTemperatureChangedHandler方法
            //所以程序知道有这个方法,如果你把这个方法写在类外部,那么就需要指定完整的路径才可调用

            // 注意：Start() 里有 while(true)，直接调用会卡死 UI 线程
            // 所以用 Task.Run 把它放到后台线程去跑
            Task.Run(() => _heater.Start());
            //Task.Run()是.NET的方法,作用是,让括号内的代码在后台线程执行,主线程代码也会正常往下执行
            //() => _heater.Start(),Lambda表达式,明确表示这段代码在后台执行是一个Action委托
            //如果直接写Task.Run(_heater.Start)也行,_heater.Start是方法组会被隐式转换为Action委托
            //但是就不能传入参数了,比如Task.Run(_heater.Start(1000));的写法会报错,
            //_heater.Start()表示立刻执行Start方法,而Start方法返回void,无返回值,所以Task.Run()会报错
            //如果是Lambda表示的写法传入参数不会报错,比如() => _heater.Start(1000)还是会被切到后台执行

            AppendLog("加热器已启动");
            //调用AppendLog方法,该方法定义在后续代码
        }

        private void OnTemperatureChangedHandler(float temp)
            
        {
            this.Invoke(() =>
            //this指当前类也就是Form1,Invoke是当前类继承类Form的基类Control的方法,把后续任务交给主线程去做
            //为什么不像AppendLog方法那样先判断当前线程是否为主线程?
            //因为AppendLog方法即被UI线程调用也被后台线程调用,所以要判断
            {
                lblTemp.Text = $"{temp:F1} ℃";
                //lblTemp控件名称,把temp转化为保留一位小数的字符串+空格+℃传给该控件的Text属性

                if (temp > 80)
                {
                    lblTemp.ForeColor = Color.Red;
                    AppendLog($"🔥 高温警报！{temp:F1}℃");
                }
                else
                {
                    lblTemp.ForeColor = Color.Black;
                    AppendLog($"📡 当前温度：{temp:F1}℃");
                }
            });
        }

        private void AppendLog(string msg)
        {
            if (rtbLog.InvokeRequired)
            //rtbLog是富文本控件名称,rtbLog.InvokeRequired判断当前线程是否为UI线程,只有UI线程才能修改
            //当前面的方法执行AppendLog("加热器已启动");跳转于此,显然不是主线程判断为ture
            //执行以下代码
            {
                rtbLog.Invoke(() => AppendLog(msg));
                //rtbLog.Invoke();是将括号里面的任务交给主线程去做
                //主线程执行() => AppendLog(msg),也会跳转到这个方法
                return;
            }
            //主线程执行AppendLog(msg)时if中条件rtbLog.InvokeRequired判断为false
            //主线程执行以下if代码块以外,方法以内的代码,
            rtbLog.AppendText($"{DateTime.Now:HH:mm:ss} {msg}{Environment.NewLine}");
            //向富文本控件中追加一行消息,$""表示内插字符串,编译器会解析花括号自动转化为字符串插入
            //DateTime.Now获取当前时间;HH:mm;ss输出时:分:秒这种格式
            //msg是AppendLog方法传入的参数msg
            //Environment.NewLine是.NET提供的静态属性,作用是获取当前操作系统换行符
            rtbLog.ScrollToCaret();
            //上一行代码会把光标置于插入字符串的末尾,这一行就是把窗口滚动到光标所在位置
        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
