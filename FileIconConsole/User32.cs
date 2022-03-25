using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileIconConsole
{
    internal class User32
    {
        public delegate bool CallBack(int hwnd, int y);

        //该函数枚举所有屏幕上的顶层窗口，并将窗口句柄传送给应用程序定义的回调函数。
        //回调函数返回FALSE将停止枚举，否则EnumWindows函数继续到所有顶层窗口枚举完为止。
        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack x, int y);

        //该函数将指定窗口的标题条文本（如果存在）拷贝到一个缓存区内
        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hwnd, StringBuilder lptrString, int nMaxCount);

        //该函数获得一个指定子窗口的父窗口句柄
        [DllImport("user32.dll")]
        public static extern int GetParent(int hwnd);

        //该函数获得给定窗口的可视状态。
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int hwnd);

        //获取窗体类名
        [DllImport("User32.Dll ")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);

        //获取窗体的子窗体句柄
        [DllImport("User32.dll ")]
        public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childe, string strclass, string FrmText);


        /// 根据句柄获取类名
        public static string GetFormClassName(IntPtr ptr)
        {
            StringBuilder nameBiulder = new StringBuilder(255);
            GetClassName(ptr, nameBiulder, 255);
            return nameBiulder.ToString();
        }

        /// 根据句柄获取窗口标题
        public static string GetFormTitle(IntPtr ptr)
        {
            StringBuilder titleBiulder = new StringBuilder(255);
            GetWindowText((int)ptr, titleBiulder, 255);
            return titleBiulder.ToString();
        }



    }
}
