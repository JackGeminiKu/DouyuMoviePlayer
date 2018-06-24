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
            MovieService.StartingPlayMovie += new Action<string>(MovieService_StartingPlayMovie);
        }

        void MovieService_StartingPlayMovie(string movieName)
        {
            lblMovieName.SetTextCrossThread(Path.GetFileNameWithoutExtension(movieName));
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

        private void frmMain_Shown(object sender, EventArgs e)
        {
            // 检查电影是否都存在


            txtRoomId.Text = Properties.Settings.Default.SavedRoom.ToString();
            StartPlay();
        }

        void CheckMovies()
        {
            var movies = DbService.GetAllMovies();
            var notFound = new List<string>();
            foreach (var movie in movies) {
                if (!File.Exists(movie))
                    notFound.Add(movie);
            }
            if (notFound.Count != 0) {
                MessageBox.Show("以下电影没有找到: \n" + string.Join("\n", notFound), "检查电影", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartPlay_Click(object sender, EventArgs e)
        {
            if (!ValidateOperation("要开始播放电影, 请输入操作密码!"))
                return;
            StartPlay();
        }

        void StartPlay()
        {
            txtRoomId.Enabled = btnStartPlay.Enabled = false;
            btnStopPlay.Enabled = true;
            if (!bwMoviePlayer.IsBusy) bwMoviePlayer.RunWorkerAsync();
        }

        private void bwMoviePlayer_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!DbService.HasMovie(RoomId)) {
                MessageBox.Show("系统中没有找到电影", "没有电影", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MovieService.StartPlay(RoomId);
        }

        private void btnStopPlay_Click(object sender, EventArgs e)
        {
            if (!ValidateOperation("要停止播放电影, 请输入操作密码!"))
                return;
            btnStopPlay.Enabled = false;
            lblMovieName.Text = "正在停止中...";
            MovieService.StopPlay();
            MyThread.Wait(3000);
            lblMovieName.Text = "已停止播放";
            txtRoomId.Enabled = btnStartPlay.Enabled = true;
        }

        private void btnCreateAlias_Click(object sender, EventArgs e)
        {
            new AliasForm().ShowDialog();
        }

        private void btnImportMovie_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            foreach (string file in Directory.GetFiles(dialog.SelectedPath)) {
                DbService.ImportMovie(RoomId, file);
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

        private void btnSaveRoom_Click(object sender, EventArgs e)
        {
            if (!ValidateOperation("要保存房间号, 请输入操作密码!"))
                return;
            Properties.Settings.Default.SavedRoom.ToString();
            Properties.Settings.Default.Save();
            MessageBox.Show("房间" + txtRoomId.Text + "保存完成!", "保存房间");
        }

        int RoomId
        {
            get { return int.Parse(txtRoomId.Text); }
        }

        bool ValidateOperation(string message)
        {
            var password = "";
            if (PasswordBox.ShowDialog(message, out password) == DialogResult.Cancel) {
                return false;
            }
            if (password != "52664638") {
                MessageBox.Show("密码错误", "密码", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
