using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window 
    {
        //构造函数
        public MainWindow()
        {
            InitializeComponent();
        }

        //窗口加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            form.WFScreen screen = new form.WFScreen();
            if (screen.ShowDialog() == true)
            {

            }
        }

        //系统
        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            form.WFSetting screen = new form.WFSetting();
            if (screen.ShowDialog() == true)
            {

            }
        }
        //屏幕
        private void BtnScn_Click(object sender, RoutedEventArgs e)
        {
            form.WFScreen screen = new form.WFScreen();
            if (screen.ShowDialog() == true)
            {

            }
        }
        //选项
        private void BtnSup_Click(object sender, RoutedEventArgs e)
        {
            form.WFSupply screen = new form.WFSupply();
            if (screen.ShowDialog() == true)
            {

            }
        }
        //关于
        private void BtnAbt_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("版本V1.0", "关于", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            AboutBox1 screen = new AboutBox1();
            if (screen.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {

            }
        }
    }
        
}
