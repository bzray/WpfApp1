using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.form
{
    /// <summary>
    /// WFScreen.xaml 的交互逻辑
    /// </summary>
    public partial class WFScreen : Window
    {

        //滚动速度
        int speed = 100;
        //滚动方式(0:随机,1:顺序)
        int sort = 0;
        //单次选中
        int count = 1;

        //启动滚动
        bool isStart = false;

        //滚动内容
        List<string> supplyList = new List<string>();
        Thread bThread;

        //构造函数
        public WFScreen()
        {
            InitializeComponent();
        }

        //窗口加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //加载待滚屏数据
            FileStream fileStream = new FileStream(utils.AppCode.CONTENT_PATH, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fileStream);
            string val = null;
            while ((val = reader.ReadLine()) != null)
            {
                if(val!=null && !"".Equals(val.Trim()))
                    supplyList.Add(val);
            }
            reader.Close();
            fileStream.Close();

            //如果没有，则初始化数据初始化
            if (supplyList.Count == 0)
            {
                for (var i = 0; i < 10; i++)
                {
                    supplyList.Add((i+1).ToString()+".测试供应商选项");
                }
            }

            //加载滚动方式
            utils.IniUtil iniUtil = new utils.IniUtil(null);
            string rdo = iniUtil.IniReadValue("set", "rdo");
            if ("1".Equals(rdo))
                sort = 1;

            string cnt = iniUtil.IniReadValue("set", "cnt");
            if(cnt!=null && !"".Equals(cnt))
            {
                try
                {
                    int idx = int.Parse(cnt);
                    if (idx > 1 && idx < 6)
                        count = idx;
                }
                catch (Exception) { }
            }

            //滚动速度
            string spd = iniUtil.IniReadValue("set", "spd");
            if(spd!=null && !"".Equals(spd))
            {
                try
                {
                    int idx = int.Parse(spd);
                    if (idx > 1)
                        speed = idx;
                }
                catch (Exception) { }
            }

            //字体大小
            LabelSupply.FontSize = 60;
            string fnt = iniUtil.IniReadValue("set", "fnt");
            if (fnt != null && !"".Equals(fnt))
            {
                try
                {
                    int idx = int.Parse(fnt);
                    if (idx >= 16 && idx<=172)
                        LabelSupply.FontSize = idx;
                }
                catch (Exception) { }
            }

            BtnEnd.IsEnabled = false;
        }

        //开始按钮
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (supplyList.Count <= 0)
            {
                MessageBox.Show("当前还没有滚动内容，请到【系统选项】设置", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LabelSupply.Content = "开始执行";
            isStart = true;
            BtnEnd.IsEnabled = isStart;
            BtnStart.IsEnabled = !isStart;

            BackGroundThread background1 = new BackGroundThread(speed, sort, supplyList, updateStatusWordInThread);

            bThread = new Thread(new ThreadStart(background1.RunLoop));
            bThread.Name = "后台线程";
            bThread.IsBackground = true;
            bThread.Start();
        }

        //结束按钮
        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            isStart = false;
            BtnEnd.IsEnabled = isStart;
            BtnStart.IsEnabled = !isStart;

            try
            {
                if (bThread != null && bThread.IsAlive)
                {
                    bThread.Abort();
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1);
            }
        }

        //声明非界面线程委托
        delegate void updateStatusStripDelegate(String obj);

        //实际修界面空间属性
        private void updateStatusWordInDelegate(string text)
        {
            LabelSupply.Content = text;
        }

        //调用委托实例申请变更
        private void updateStatusWordInThread(string text)
        {
            updateStatusStripDelegate d = new updateStatusStripDelegate(updateStatusWordInDelegate);
            this.Dispatcher.Invoke(d, text);
        }

        //窗口关闭
        private void WScreen_Closed(object sender, EventArgs e)
        {
            if (isStart)
            {
                try
                {
                    if (bThread != null && bThread.IsAlive)
                    {
                        bThread.Abort();
                    }
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1);
                }
            }
        }

        private void WScreen_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //开始按钮
            Thickness strMargin = BtnStart.Margin;
            strMargin.Top = e.NewSize.Height - 100;

            //结束按钮
            Thickness endMargin = BtnEnd.Margin;
            endMargin.Top = e.NewSize.Height - 100;

            double unit = (e.NewSize.Width - BtnStart.Width - BtnEnd.Width)/3;

            strMargin.Left = unit;
            endMargin.Left = unit * 2 + BtnStart.Width;

            BtnEnd.Margin = endMargin;
            BtnStart.Margin = strMargin;

            //滚屏位置
            Thickness vbx = LabelViewBox.Margin;
            vbx.Top = (e.NewSize.Height - LabelViewBox.Height)*2 / 5;
            LabelViewBox.Margin = vbx;

            LabelViewBox.Width = e.NewSize.Width;
            LabelSupply.Width = e.NewSize.Width;
        }

        //空格键触发(开始-结束)
        private void WScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                if (isStart)
                {
                    BtnEnd_Click(sender, e);
                }
                else
                {
                    BtnStart_Click(sender, e);
                }
            }
        }
    }

    //声明线程回调委托类型
    public delegate void CallBackDelegate(string message);
    

    //后调线程调用的执行方法
    class BackGroundThread
    {
        // 间隔时间
        int Sleep = 500;

        //滚动方向(0:随机,1:顺序)
        int sort = 0;
        // 数据
        List<string> Supply = new List<string>();
        // 回调委托
        private CallBackDelegate callback;

        public BackGroundThread(int sleep, int sort, List<string> supply, CallBackDelegate callbackDelegate)
        {
            this.Sleep = sleep;
            this.sort = sort;
            this.Supply = supply;
            this.callback = callbackDelegate;
        }

        public void RunLoop()
        {
            string threadName = Thread.CurrentThread.Name;
            int idx = 0;
            for (var i = 0; ; i++)
            {
                var rdx = new Random().Next(0, Supply.Count);

                if (sort > 0)
                {
                    rdx = idx++;
                }

                Console.WriteLine("第{0}个：{1}", rdx, Supply[rdx].ToString());

                if (callback != null) callback(Supply[rdx].ToString());
                Thread.Sleep(Sleep);

                if (idx >= Supply.Count)
                    idx = 0;
            }
        }
    }
}
