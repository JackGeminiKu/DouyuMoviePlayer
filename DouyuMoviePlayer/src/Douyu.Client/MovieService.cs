using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Douyu.Properties;
using System.IO;

namespace Douyu.Client
{
    public static class MovieService
    {
        static MovieService()
        {
            PlayingEnabled = true;
        }

        public static void StartPlay(int roomId)
        {
            PlayingEnabled = true;
            // 当前正在播放?
            if (MovieService.IsPlaying) {
                if (MovieService.IsPlayingAdvert) {
                    MovieService.WaitPlayingFinish();
                } else {
                    MovieService.WaitPlayingFinish();
                    if (MovieService.PlayingEnabled) MovieService.PlayAdvert(roomId);
                }
            }

            // 按序播放
            do {
                if (MovieService.PlayingEnabled) MovieService.PlayMovie(roomId);
                if (MovieService.PlayingEnabled) MovieService.PlayAdvert(roomId);
                if (!MovieService.PlayingEnabled) MyThread.Wait(1000);
            } while (true);
        }

        public static void StopPlay()
        {
            PlayingEnabled = false;

            Process[] processes = Process.GetProcessesByName(Settings.Default.PlayerProcessName);
            if (processes.Length == 1) {
                processes[0].Kill();
            }
        }

        static bool IsPlaying
        {
            get { return Process.GetProcessesByName(Settings.Default.PlayerProcessName).Length != 0; }
        }

        static bool IsPlayingAdvert
        {
            get
            {
                return MessageBox.Show("正在播放广告?", "正在播放", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes;
            }
        }

        static void PlayAdvert(int roomId)
        {
            string advertFile = Properties.Settings.Default.MovieDir + "\\" + roomId + ".mp4";
            if (!File.Exists(advertFile)) {
                MessageBox.Show("没有找到视频文件: " + advertFile);
                return;
            }
            PlayMovie(advertFile);
            WaitPlayingFinish();
            MyThread.Wait(1000);
        }

        static void PlayMovie(int roomId)
        {
            string movieFile;
            string movieName;
            DbService.GetTopMovie(roomId, out movieName, out movieFile);
            PlayMovie(movieFile);
            DbService.ClearTopMovie(roomId, movieFile);
            DbService.SetCurrentMovie(roomId, movieName);
            WaitPlayingFinish();
            MyThread.Wait(1000);
        }

        static void WaitPlayingFinish()
        {
            do {
                MyThread.Wait(1000);
            } while (IsPlaying);
        }

        static void PlayMovie(string movieName)
        {
            Process.Start(Settings.Default.MoviePlayer, movieName);
            if (StartPlayMovie != null)
                StartPlayMovie(movieName);
        }

        static bool PlayingEnabled { get; set; }

        static bool IsNumber(string text)
        {
            int result;
            return int.TryParse(text, out result);
        }

        public static event Action<string> StartPlayMovie;
    }
}
