using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileIconConsole
{
    internal class ConsoleHelper
    {

        public static Dictionary<string, string[]> ConvertToArgDic(string[] args)
        {
            var argDic = new Dictionary<string, string[]>();
            if (args != null)
            {
                string key = null;
                List<string> value = new List<string>();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-"))
                    {
                        if (key != null)
                        {
                            if (!argDic.ContainsKey(key))
                            {
                                argDic.Add(key, value.ToArray());
                            }
                        }
                        key = args[i].ToLower();
                        value.Clear();
                    }
                    else
                    {
                        value.Add(args[i]);
                    }
                }
                if (key != null)
                {
                    if (!argDic.ContainsKey(key))
                    {
                        argDic.Add(key, value.ToArray());
                    }
                }
            }
            return argDic;
        }

    }
}
