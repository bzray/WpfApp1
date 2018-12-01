using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
    /// WFSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WFSetting : Window
    {
        public WFSetting()
        {
            InitializeComponent();
        }

        

        //加载数据库
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            utils.IniUtil iniUtil = new utils.IniUtil(null);
            string rdo = iniUtil.IniReadValue("set", "rdo");
            string cnt = iniUtil.IniReadValue("set", "cnt");
            string spd = iniUtil.IniReadValue("set", "spd");
            string fnt = iniUtil.IniReadValue("set", "fnt");

            Console.WriteLine("{0}:{1}", rdo, cnt);


            Rdo01.IsChecked = true;
            Rdo02.IsChecked = false;

            //顺序
            if ("1".Equals(rdo))
            {
                Rdo01.IsChecked = false;
                Rdo02.IsChecked = true;
            }
            
            //速度
            if(spd!=null && !"".Equals(spd))
            {
                TbSpeed.Text = spd;
            }

            //字体
            if (fnt != null && !"".Equals(fnt))
            {
                TBSize.Text = fnt;
            }

            //saveIniFile(iniUtil);
        }

        bool saveIniFile(utils.IniUtil iniUtil) {
            if (iniUtil == null)
            {
                iniUtil = new utils.IniUtil(null);
            }

            try
            {
                int siz = int.Parse(TBSize.Text);
                if (siz < 16)
                {
                    MessageBox.Show("字体值不能小于16", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }else if (siz > 172)
                {
                    MessageBox.Show("字体值不能大于172", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("字体只允许录入数字", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                int spd = int.Parse(TbSpeed.Text);
                if (spd <= 0)
                {
                    MessageBox.Show("速度值需要大于0", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("速度只允许录入数字", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            iniUtil.IniWriteValue("set", "rdo", Rdo01.IsChecked == true ? "0" : "1");
            iniUtil.IniWriteValue("set", "cnt", "1");
            iniUtil.IniWriteValue("set", "spd", TbSpeed.Text);
            iniUtil.IniWriteValue("set", "fnt", TBSize.Text);

            return true;



        }

        //恢复初始化
        private void BtnRset_Click(object sender, RoutedEventArgs e)
        {
            Rdo01.IsChecked = true;
            Rdo02.IsChecked = false;
            TbSpeed.Text = "200";
            TBSize.Text = "16";
            BtnSave_Click(sender, e);
        }

        //保存
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (saveIniFile(null))
            {
                if (MessageBox.Show("保存成功", "提醒", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            
        }

    }
}
