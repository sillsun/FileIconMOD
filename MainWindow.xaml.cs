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
            bool isClean = CheckBox_CleanCfg.IsChecked == true;
            if (!CheckInput(ref fileType, ref iconPath, ref openProgram))
            {
                return;
            }


            StringBuilder argBuilder = new StringBuilder();
            if (fileType != null)
                argBuilder.Append($"-type \"{fileType}\" ");
            if (iconPath != null)
                argBuilder.Append($"-icon \"{iconPath}\" ");
            if (openProgram != null)
                argBuilder.Append($"-open \"{openProgram}\" ");
            if (isClean)
                argBuilder.Append($"-clean");
            System.Diagnostics.Process.Start("FileIconConsole.exe", argBuilder.ToString());


        }

        private bool CheckInput(ref string fileType, ref string iconPath, ref string openProgram)
        {
            if (fileType == "" || fileType == null)
            {
                MessageBox.Show("请输入需要修改图标的类型\n例如  .xlsx或将文件拖入 ");
                return false;
            }
            else if (fileType.ToLower() == ".lnk" || fileType.ToLower() == "lnk"
                  || fileType.ToLower() == ".exe" || fileType.ToLower() == "exe")
            {
                MessageBox.Show("本工具不支持修改.lnk和.exe文件的图标");
                return false;
            }
            else
            {
                if (fileType.IndexOf(".") < 0)
                {
                    fileType = "." + fileType;
                }
            }

            if (CheckBox_IconPath.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(iconPath))
                {
                    iconPath = "";
                }
                else if (!File.Exists(iconPath))
                {
                    MessageBox.Show("图标路径有误\n提示：可以输入图标路径或将图标文件拖入");
                    return false;

                }
                else
                {
                    var iconFile = new FileInfo(iconPath);
                    var iconExt = iconFile.Extension.ToLower();
                    if (iconExt != ".bmp" && iconExt != ".jpg" && iconExt != ".jpeg" && iconExt != ".png")
                    {
                        MessageBox.Show("图标格式不支持\n提示：当前支持.bmp .jpg .jpeg .png格式的图标");
                        return false;
                    }
                }
            }
            else
            {
                iconPath = null;
            }

            if (CheckBox_OpenExe.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(openProgram))
                {
                    openProgram = "";
                }
                else if (!File.Exists(openProgram))
                {
                    MessageBox.Show("程序路径错误\n提示：可以输入程序路径或将程序文件拖入");
                    return false;
                }
            }
            else
            {
                openProgram = null;
            }
            return true;
        }



        private void Text_FilePath_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Text_FilePath_PreviewDrop(object sender, DragEventArgs e)
        {
            var txb = ((TextBox)sender);
            txb.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            txb.Focus();
        }
        private void TextBox_FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txb = ((TextBox)sender);
        }

        private void Text_FileType_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Text_FileType_PreviewDrop(object sender, DragEventArgs e)
        {
            var path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];


            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                var ext = file.Extension.ToLower();

                ((TextBox)sender).Text = ext;


            }

        }


        private void TextBox_FileType_LostFocus(object sender, RoutedEventArgs e)
        {
            var txb = ((TextBox)sender);
            txb.SelectionStart = txb.Text.Length;

        }
    }
}
