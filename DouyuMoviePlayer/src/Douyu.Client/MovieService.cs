using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Douyu.Properties;
using System.IO;
using Jack4net.Log;

namespace Douyu.Client
{
    public static class MovieService
    {
        public static void StartPlay()
        {
            PlayEnabled = true;

            // 当前正在播放?
            if (IsPlaying) {
                WaitPlayingFinish();
                PlayAdvert();
            }

            // 按序播放
            do {
                if (PlayEnabled)
                    PlayMovie();
                if (PlayEnabled)
                    PlayAdvert();
                if (!PlayEnabled)
                    MyThread.Wait(1000);
            } while (PlayEnabled);
        }

        public static void StopPlay()
        {
            PlayEnabled = false;

            var processes = Process.GetProcessesByName(Settings.Default.PlayerProcessName);
            foreach (var process in processes) {
                process.Kill();
            }
        }

        static bool PlayEnabled { get; set; }

        public static bool IsPlaying
        {
            get { return Process.GetProcessesByName(Settings.Default.PlayerProcessName).Length != 0; }
        }

        static void PlayAdvert()
        {
            var advertFile = DbService.GetAdvertMovie(AppSettings.RoomId);
            DoPlayMovie(advertFile);
            WaitPlayingFinish();
        }

        static void PlayMovie()
        {
            var movieFile = "";
            var movieName = "";
            DbService.GetTopMovie(AppSettings.RoomId, out movieName, out movieFile);
            DoPlayMovie(movieFile);

            DbService.SetCurrentMovie(AppSettings.RoomId, movieName);
            Obs.SetCurrentMovie(movieName);
            DbService.UpdateMoviePlaytime(AppSettings.RoomId, movieName);
            DbService.ClearMovieScore(AppSettings.RoomId, movieName);
            WaitPlayingFinish();
        }

        static void WaitPlayingFinish()
        {
            do {
                MyThread.Wait(1000);
            } while (IsPlaying);
            MyThread.Wait(1000);
        }

        static void DoPlayMovie(string movieName)
        {
            if (!File.Exists(movieName)) {
                LogService.WarnFormat("没有找到播放文件: {0}", movieName);
                return;
            }

            Process.Start(Settings.Default.MoviePlayer, movieName);
            if (StartingPlayMovie != null)
                StartingPlayMovie(null, new StartPlayMovieEventArgs(movieName));
        }

        public static event EventHandler<StartPlayMovieEventArgs> StartingPlayMovie;
    }



    public class StartPlayMovieEventArgs : EventArgs
    {
        public StartPlayMovieEventArgs(string movieName)
        {
            MovieName = movieName;
        }

        public string MovieName { get; private set; }
    }
}
