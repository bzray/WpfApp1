using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.utils
{
    class IniUtil
    {
        string inipath;

        public IniUtil(string inipath)
        {
            if(inipath==null)
            {
                this.inipath = Directory.GetCurrentDirectory() + "\\" + "set.ini";

            }
            else
            {
                this.inipath = inipath;
            }
            if (!File.Exists(this.inipath))
            {
                FileStream file = new FileStream(this.inipath, FileMode.Create);
                file.Close();
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            /**
            StringBuilder s = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, "", s, 1024, this.inipath);
            Console.WriteLine(s.ToString());
            **/

            StringBuilder temp = new StringBuilder(1024*1024);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            string val = temp.ToString();
            Console.WriteLine(val);

            return val;
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
    }
}
