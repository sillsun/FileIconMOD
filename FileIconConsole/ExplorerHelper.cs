using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileIconConsole
{
    internal class ExplorerHelper
    {
        public static List<string> GetOpenedDirectory()
        {
            var paths = new List<string>();
            User32.EnumWindows((hwnd, IParam) =>
            {
                int pHwnd = User32.GetParent(hwnd);
                //如果再没有父窗口并且为可视状态的窗口，则遍历
                if (pHwnd == 0 && User32.IsWindowVisible(hwnd) == true)
                {
                    IntPtr cabinetWClassIntPtr = new IntPtr(hwnd);
                    string cabinetWClassName = User32.GetFormClassName(cabinetWClassIntPtr);
                    //如果类名为CabinetWClass ，则为explorer窗口，可以通过spy++查看窗口类型
                    if (cabinetWClassName.Equals("CabinetWClass", StringComparison.OrdinalIgnoreCase))
                    {
                        //下面为一层层往下查找，直到找到资源管理器的地址窗体，通过他获取窗体地址
                        IntPtr workerWIntPtr = User32.FindWindowEx(cabinetWClassIntPtr, IntPtr.Zero, "WorkerW", null);
                        IntPtr reBarWindow32IntPtr = User32.FindWindowEx(workerWIntPtr, IntPtr.Zero, "ReBarWindow32", null);
                        IntPtr addressBandRootIntPtr = User32.FindWindowEx(reBarWindow32IntPtr, IntPtr.Zero, "Address Band Root", null);
                        IntPtr msctls_progress32IntPtr = User32.FindWindowEx(addressBandRootIntPtr, IntPtr.Zero, "msctls_progress32", null);
                        IntPtr breadcrumbParentIntPtr = User32.FindWindowEx(msctls_progress32IntPtr, IntPtr.Zero, "Breadcrumb Parent", null);
                        IntPtr toolbarWindow32IntPtr = User32.FindWindowEx(breadcrumbParentIntPtr, IntPtr.Zero, "ToolbarWindow32", null);


                        string title = User32.GetFormTitle(toolbarWindow32IntPtr);
                        int index = title.IndexOf(':') + 1;
                        string path = title.Substring(index, title.Length - index).TrimStart();
                        paths.Add(path);
                    }
                }
                return true;
            }, 0);

            return paths;
        }
    }
}
