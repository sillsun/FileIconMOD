using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileIconMOD
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string fileType = TextBox_FileType.Text;
            string iconPath = TextBox_IconPath.Text;
            string openProgram = TextBox_OpenExe.Text;

            if (fileType==""||fileType == null)
            {
                MessageBox.Show("请输入需要修改图标的类型\n例如  .x ");
                return;
            }
            else
            {
                if (fileType.IndexOf(".")<0)
                {
                    fileType = "." + fileType;
                }
            }
            string savePath = @"C:\Windows\System32\xxicon";
            string iconName = "";
            if (iconPath!=""&& iconPath!=null )
            {
                if (!System.IO.File.Exists(iconPath))
                {
                    MessageBox.Show("图标路径有误，请检查\n提示：将图标拖入文本内即可");
                    return;
                }

                if (!System.IO.Directory.Exists(savePath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(savePath + "\r\n" + "文件夹创建失败");
                        return;
                    }
                }
                //将图标复制到指定的文件夹
                 iconName =  System.IO.Path.GetFileName(iconPath);//返回带扩展名的文件名 
                System.IO.File.Copy(iconPath, savePath+"\\"+iconName,true);
            }

            if (openProgram!="" && openProgram!=null )
            {
                if (!System.IO.File.Exists(openProgram))
                {
                    MessageBox.Show("程序路径有误，请检查\n提示：将程序拖入文本内即可");
                    return;
                }
            }
            //创建reg文件
            string fileName = fileType.Substring(1, fileType.Length - 1) +"_xxfile";
            string regTxt= "Windows Registry Editor Version 5.00\r\n\r\n";

            //HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\

            regTxt += ("[-HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + fileType + "]\r\n\r\n");

            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileType + "]\r\n");
            regTxt += ("@=\"" + ToRegString(fileName) + "\"\r\n\r\n" );
            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileName + "]\r\n\r\n");
            //[HKEY_CLASSES_ROOT\mdfile\DefaultIcon]
            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileName + "\\DefaultIcon]\r\n");
            if (iconPath!="")
            {
                regTxt += ("@=\"" + ToRegString(savePath + "\\" + iconName) + "\"\r\n\r\n");
            }
            else
            {
                regTxt += "\r\n";
            }

            //[HKEY_CLASSES_ROOT\mdfile\shell]
            //[HKEY_CLASSES_ROOT\mdfile\shell\open]
            //[HKEY_CLASSES_ROOT\mdfile\shell\open\command]
            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileName + "\\shell]\r\n\r\n");
            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileName + "\\shell\\open]\r\n\r\n");
            regTxt += ("[HKEY_CLASSES_ROOT\\" + fileName + "\\shell\\open\\command]\r\n");
            if (openProgram != "")
            {
                regTxt += ("@=\"" + ToRegString("\""+openProgram+ "\"") + " \\\"%1\\\"\"" );
            }
            else
            {
                regTxt += "\r\n";
            }


            try
            {
                using (StreamWriter sr = new StreamWriter("run.reg", false, Encoding.Default))
                {
                    sr.Write(regTxt);
                }
   
                System.Diagnostics.Process.Start("regedit.exe", "/s run.reg");
                System.Diagnostics.Process.Start("re.bat");
            }
            catch (Exception)
            {

            }
        }

        private void Text_FilePath_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Text_FilePath_PreviewDrop(object sender, DragEventArgs e)
        {
            ((TextBox)sender).Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private string ToRegString(string word)
        {
            string newWord = word;
            newWord = newWord.Replace("\\","\\\\");
            newWord = newWord.Replace("\"", "\\\"");
            return newWord;
        }
    }
}
