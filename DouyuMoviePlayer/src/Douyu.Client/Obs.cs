using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Douyu.Client
{
    public static class Obs
    {
        static Obs()
        {
            if (!Directory.Exists(ObsDir))
                Directory.CreateDirectory(ObsDir);
        }

        public static void SetCurrentMovie(string movieName)
        {
            File.WriteAllText(ObsDir + "MovieName.txt", movieName, Encoding.UTF8);
        }

        private static string ObsDir
        {
            get { return JApplication.RootPath + @"OBS\"; }
        }
    }
}
