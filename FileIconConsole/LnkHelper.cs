using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileIconConsole
{
    internal class LnkHelper
    {
        public static IWshShortcut GetShortcutInfo(string path)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(path);
            return shortcut;
        }
    }
}
