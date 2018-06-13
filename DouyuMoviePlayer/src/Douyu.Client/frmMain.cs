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
            MovieService.StartPlayMovie += new Action<string>(MovieService_StartPlayMovie);
        }

        void MovieService_StartPlayMovie(string movieName)
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
            txtRoomId.Text = Properties.Settings.Default.SavedRoom.ToString();
            StartPlay();
        }

        private void btnStartPlay_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("系统中没有找到电影", "没有电影", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            MovieService.StartPlay(RoomId);
        }

        private void btnStopPlay_Click(object sender, EventArgs e)
        {
            btnStopPlay.Enabled = false;
            txtRoomId.Enabled = btnStartPlay.Enabled = true;
            lblMovieName.Text = "已停止播放";
            MovieService.StopPlay();
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
            MessageBox.Show("导入电影完成", "导入电影");
        }

        private void btnSaveRoom_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SavedRoom.ToString();
            Properties.Settings.Default.Save();
            MessageBox.Show("房间" + txtRoomId.Text + "保存完成!", "保存房间");
        }

        int RoomId
        {
            get { return int.Parse(txtRoomId.Text); }
        }
    }
}
