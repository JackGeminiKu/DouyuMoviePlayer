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
        public static void StartPlay(int roomId)
        {
            PlayEnabled = true;
            // 当前正在播放?
            if (IsPlaying) {
                if (IsPlayingAdvert) {
                    WaitPlayingFinish();
                } else {
                    WaitPlayingFinish();
                    if (PlayEnabled)
                        PlayAdvert(roomId);
                }
            }

            // 按序播放
            do {
                if (PlayEnabled)
                    PlayMovie(roomId);
                if (PlayEnabled)
                    PlayAdvert(roomId);
                if (!PlayEnabled)
                    MyThread.Wait(1000);
            } while (PlayEnabled);
        }

        public static void StopPlay()
        {
            PlayEnabled = false;

            var processes = Process.GetProcessesByName(Settings.Default.PlayerProcessName);
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
            var advertFile = DbService.GetAdvertMovie(roomId);
            if (!File.Exists(advertFile)) {
                MessageBox.Show("没有找到广告文件: " + advertFile, "播放广告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PlayMovie(advertFile);
            WaitPlayingFinish();
            MyThread.Wait(1000);
        }

        static void PlayMovie(int roomId)
        {
            var movieFile = "";
            var movieName = "";
            DbService.GetTopMovie(roomId, out movieName, out movieFile);
            var advertFile = DbService.GetAdvertMovie(roomId);
            if (!File.Exists(advertFile)) {
                MessageBox.Show("没有找到广告文件: " + advertFile, "播放广告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PlayMovie(movieFile);
            DbService.ClearMovieScore(roomId, movieFile);
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
            if (StartingPlayMovie != null)
                StartingPlayMovie(movieName);
        }

        static bool PlayEnabled { get; set; }

        public static event Action<string> StartingPlayMovie;
    }
}
