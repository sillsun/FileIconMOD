using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileIconConsole
{
    internal class RegBuilder
    {
        StringBuilder sb = new StringBuilder();

        string Extension = null;
        string IconPath = null;
        string ExePath = null;
        bool IsClean = false;


        public override string ToString()
        {
            return sb.ToString();
        }

        public RegBuilder(Dictionary<string, string[]> argDic)
        {
            if (argDic.TryGetValue("-type", out var type))
            {
                Extension = type[0].StartsWith(".") ? type[0].Substring(1) : type[0];
            }

            if (argDic.ContainsKey("-clean"))
            {
                IsClean = true;
            }

            if (argDic.TryGetValue("-icon", out var icon))
            {
                IconPath = icon[0];
            }

            if (argDic.TryGetValue("-open", out var open))
            {
                ExePath = open[0];
            }


            Create();
            AppendIcon();
            AppendOpenCommand();
        }



        void Create()
        {
            sb.AppendLine("Windows Registry Editor Version 5.00");
            if (IsClean)
            {
                sb.AppendLine($"[-HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.{Extension}]");
                sb.AppendLine($"[-HKEY_CLASSES_ROOT\\.{Extension}]");
                sb.AppendLine($"[-HKEY_CLASSES_ROOT\\{Extension}_xxfile\\DefaultIcon]");
                sb.AppendLine($"[-HKEY_CLASSES_ROOT\\{Extension}_xxfile\\shell\\open\\command]");
            }

            if (IconPath != null || ExePath != null)
            {
                sb.AppendLine($"[HKEY_CLASSES_ROOT\\.{Extension}]");
                sb.AppendLine($"@=\"{Extension}_xxfile\"");
            }

        }



        public void AppendIcon()
        {
            if (IconPath != null)
            {
                var newIconPath = CopyIcon(IconPath);
                sb.AppendLine($"[HKEY_CLASSES_ROOT\\{Extension}_xxfile\\DefaultIcon]");
                sb.AppendLine($"@=\"{newIconPath.Replace("\\", "\\\\")}\"");
            }

        }

        static string CopyIcon(string iconPath)
        {
            if (iconPath == null)
            {
                return iconPath;
            }
            string xxIconSavePath = @"C:\Windows\System32\xxicon";
            if (!Directory.Exists(xxIconSavePath))
            {
                Directory.CreateDirectory(xxIconSavePath);
            }

            if (File.Exists(iconPath))
            {
                var iconFile = new FileInfo(iconPath);
                var iconMd5 = MD5Helper.CalcMD5L(iconFile);
                var newIconName = $"{iconMd5}{iconFile.Extension}";

                var newPath = $"{xxIconSavePath}\\{newIconName}";
                if (!File.Exists(newPath))
                {
                    File.Copy(iconPath, newPath);
                }
                return newPath;
            }
            else
            {
                return iconPath;
            }
        }





        public void AppendOpenCommand()
        {
            if (ExePath != null)
            {
                if (File.Exists(ExePath))
                {
                    var fileInfo = new FileInfo(ExePath);
                    if (fileInfo.Extension == ".lnk")
                    {
                        ExePath = LnkHelper.GetShortcutInfo(ExePath).TargetPath;
                    }
                }
                sb.AppendLine($"[HKEY_CLASSES_ROOT\\{Extension}_xxfile\\shell\\open\\command]");
                sb.AppendLine($"@=\"\\\"{ExePath.Replace("\\", "\\\\")}\\\" \\\"%1\\\"\"");
            }

        }


    }
}
