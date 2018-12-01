using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// WFSupply.xaml 的交互逻辑
    /// </summary>
    public partial class WFSupply : Window
    {
        public WFSupply()
        {
            InitializeComponent();
        }

        string filePath = utils.AppCode.CONTENT_PATH;

        //读取文件，加载到文本框
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /**
            List<Block> blocks = RtxContent.Document.Blocks.ToList();
            for (var idx = 0; idx < blocks.Count; idx++)
            {
                RtxContent.Document.Blocks.Remove(blocks[idx]);
            }
            **/ 

            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fileStream);
            string val = null;
            while ((val= reader.ReadLine())!=null)
            {
                RtxContent.AppendText(val);
                RtxContent.AppendText(System.Environment.NewLine);
            }
            reader.Close();
            fileStream.Close();
        }

        //保存
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            var cnt = 0;
            using (System.IO.FileStream file = new System.IO.FileStream(filePath, FileMode.Create))
            {
                StreamWriter streamWriter = new StreamWriter(file);
                List<Block> blocks = RtxContent.Document.Blocks.ToList();
                for (var idx = 0; idx < blocks.Count; idx++)
                {
                    TextRange textRange = new TextRange(blocks[idx].ContentStart, blocks[idx].ContentEnd);
                    string txt = textRange.Text;
                    if (txt != null && !"".Equals(txt) && !"".Equals(txt.Trim()))
                    {
                        streamWriter.WriteLine(txt.Trim());
                        cnt++;
                    }
                }
                streamWriter.Close();
                file.Close();
            }
            /**
            TextRange textRange = new TextRange(RtxContent.Document.ContentStart, RtxContent.Document.ContentEnd);
            Console.WriteLine(textRange.Text);
            **/
            if (cnt > 0)
            {
                if (MessageBox.Show("保存成功", "提醒", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                if (MessageBox.Show("您还没有保存任何数据，确认关闭？", "提醒", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            
        }
    }
}
