using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.IO;

namespace Douyu.Client
{
    public class ScrollFile
    {
        public ScrollFile(string fileName)
        {
            FileName = fileName;
            Messages = new List<string>();
        }

        public string FileName { get; set; }
        public List<string> Messages { get; private set; }
        public int CurrentIndex { get; private set; }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        public void ShowNext()
        {
            File.WriteAllText(ObsDir + FileName, Messages[CurrentIndex++], Encoding.UTF8);
            if (CurrentIndex >= Messages.Count)
                CurrentIndex = 0;
        }

        static string ObsDir
        {
            get { return JApplication.RootPath + @"OBS\"; }
        }
    }
}
