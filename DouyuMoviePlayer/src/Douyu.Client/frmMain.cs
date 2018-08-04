using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Douyu.Properties;
using System.IO;
using System.Threading;
using System.Reflection;

namespace Douyu.Client
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            SetFormLocation();
            ShowAppVersion();
            MovieService.StartingPlayMovie += MovieService_StartingPlayMovie;
        }

        void SetFormLocation()
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Size.Width,
                Screen.PrimaryScreen.WorkingArea.Height - Size.Height);
        }

        void ShowAppVersion()
        {
            this.Text += " v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        void MovieService_StartingPlayMovie(object sender, StartPlayMovieEventArgs e)
        {
            lblMovieName.SetTextCrossThread(Path.GetFileNameWithoutExtension(e.MovieName));
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            StartPlay();
        }

        #region 播放控制

        private void btnStartPlay_Click(object sender, EventArgs e)
        {
            StartPlay();
        }

        void StartPlay()
        {
            btnStartPlay.Enabled = false;
            btnStopPlay.Enabled = true;
            MovieService.StartPlay();
        }

        private void btnStopPlay_Click(object sender, EventArgs e)
        {
            if (!ValidateOperation("要停止播放电影, 请输入操作密码!"))
                return;

            lblMovieName.Text = "正在停止中...";
            btnStopPlay.Enabled = false;
            MovieService.StopPlay();

            var watch = Stopwatch.StartNew();
            do {
                if (!MovieService.IsPlaying)
                    break;
                MyThread.Wait(100);
            } while (watch.ElapsedMilliseconds < 3000);
            if (MovieService.IsPlaying)
                MessageBox.Show("停止关闭失败, 播放器进程还在!");

            lblMovieName.Text = "已停止播放";
            btnStartPlay.Enabled = true;
        }

        #endregion

        private void btnCreateAlias_Click(object sender, EventArgs e)
        {
            var aliasForm = new AliasForm();
            aliasForm.ShowDialog();
        }

        private void btnImportMovie_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            foreach (string file in Directory.GetFiles(dialog.SelectedPath)) {
                DbService.ImportMovie(MovieService.RoomId, file);
            }
            MessageBox.Show("导入电影完成!", "导入电影");
        }

        private void btnImportAdvert_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            foreach (string file in Directory.GetFiles(dialog.SelectedPath)) {
                DbService.ImportAdvert(file);
            }
            MessageBox.Show("导入广告完成!", "导入广告");
        }

        bool ValidateOperation(string message)
        {
            var password = "";
            if (PasswordBox.ShowDialog(message, out password) == DialogResult.Cancel) {
                return false;
            }
            if (password != "123456") {
                MessageBox.Show("密码错误", "密码", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}
