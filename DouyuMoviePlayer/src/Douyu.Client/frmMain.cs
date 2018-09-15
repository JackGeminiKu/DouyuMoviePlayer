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
            tmrScrollPlayTips.Start();
            tmrScrollScoreTips.Start();
            txtRoomId.Text = AppSettings.RoomId.ToString();
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

        bool ValidateOperation(string message)
        {
            var password = "";
            if ((password = PasswordBox.ShowDialog(message)) == "") {
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

        ScrollFile _scoreTipsFile;
        ScrollFile _playTipsFile;

        private void tmrScrollScoreTips_Tick(object sender, EventArgs e)
        {
            if (_scoreTipsFile == null) {
                _scoreTipsFile = new ScrollFile("ScoreTips.txt");
                _scoreTipsFile.AddMessage("【1弹幕=100鱼丸=1赞=100分~1弱=400分】");
                _scoreTipsFile.AddMessage("【办卡1.5万~飞机28万~火箭150万~超级火箭=666万】");
                _scoreTipsFile.AddMessage("【查询积分命令：#查询】");
            }
            _scoreTipsFile.ShowNext();
        }

        private void tmrScrollPlayTips_Tick(object sender, EventArgs e)
        {
            if (_playTipsFile == null) {
                _playTipsFile = new ScrollFile("PlayTips.txt");
                _playTipsFile.AddMessage("↓按积分多少排序播放↓");
                _playTipsFile.AddMessage("点播命令：#电影名-积分");
            }
            _playTipsFile.ShowNext();
        }        
    }
}
