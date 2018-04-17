namespace Douyu.Client
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.bwMoviePlayer = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMovieName = new System.Windows.Forms.Label();
            this.btnImportMovie = new System.Windows.Forms.Button();
            this.btnStartPlay = new System.Windows.Forms.Button();
            this.txtRoomId = new System.Windows.Forms.TextBox();
            this.btnSaveRoom = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStopPlay = new System.Windows.Forms.Button();
            this.btnCreateAlias = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bwMoviePlayer
            // 
            this.bwMoviePlayer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwMoviePlayer_DoWork);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMovieName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "正在播放";
            // 
            // lblMovieName
            // 
            this.lblMovieName.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMovieName.Location = new System.Drawing.Point(16, 22);
            this.lblMovieName.Name = "lblMovieName";
            this.lblMovieName.Size = new System.Drawing.Size(237, 50);
            this.lblMovieName.TabIndex = 0;
            this.lblMovieName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnImportMovie
            // 
            this.btnImportMovie.Location = new System.Drawing.Point(296, 76);
            this.btnImportMovie.Name = "btnImportMovie";
            this.btnImportMovie.Size = new System.Drawing.Size(121, 23);
            this.btnImportMovie.TabIndex = 3;
            this.btnImportMovie.Text = "导入电影";
            this.btnImportMovie.UseVisualStyleBackColor = true;
            this.btnImportMovie.Click += new System.EventHandler(this.btnImportMovie_Click);
            // 
            // btnStartPlay
            // 
            this.btnStartPlay.Location = new System.Drawing.Point(296, 47);
            this.btnStartPlay.Name = "btnStartPlay";
            this.btnStartPlay.Size = new System.Drawing.Size(121, 23);
            this.btnStartPlay.TabIndex = 4;
            this.btnStartPlay.Text = "开始播放";
            this.btnStartPlay.UseVisualStyleBackColor = true;
            this.btnStartPlay.Click += new System.EventHandler(this.btnStartPlay_Click);
            // 
            // txtRoomId
            // 
            this.txtRoomId.Location = new System.Drawing.Point(350, 12);
            this.txtRoomId.Name = "txtRoomId";
            this.txtRoomId.Size = new System.Drawing.Size(128, 25);
            this.txtRoomId.TabIndex = 5;
            this.txtRoomId.Text = "122402";
            // 
            // btnSaveRoom
            // 
            this.btnSaveRoom.Location = new System.Drawing.Point(494, 14);
            this.btnSaveRoom.Name = "btnSaveRoom";
            this.btnSaveRoom.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRoom.TabIndex = 6;
            this.btnSaveRoom.Text = "保存房间";
            this.btnSaveRoom.UseVisualStyleBackColor = true;
            this.btnSaveRoom.Click += new System.EventHandler(this.btnSaveRoom_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "房间号";
            // 
            // btnStopPlay
            // 
            this.btnStopPlay.Location = new System.Drawing.Point(448, 47);
            this.btnStopPlay.Name = "btnStopPlay";
            this.btnStopPlay.Size = new System.Drawing.Size(121, 23);
            this.btnStopPlay.TabIndex = 8;
            this.btnStopPlay.Text = "停止播放";
            this.btnStopPlay.UseVisualStyleBackColor = true;
            this.btnStopPlay.Click += new System.EventHandler(this.btnStopPlay_Click);
            // 
            // btnCreateAlias
            // 
            this.btnCreateAlias.Location = new System.Drawing.Point(448, 76);
            this.btnCreateAlias.Name = "btnCreateAlias";
            this.btnCreateAlias.Size = new System.Drawing.Size(121, 23);
            this.btnCreateAlias.TabIndex = 10;
            this.btnCreateAlias.Text = "添加别名";
            this.btnCreateAlias.UseVisualStyleBackColor = true;
            this.btnCreateAlias.Click += new System.EventHandler(this.btnCreateAlias_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 112);
            this.Controls.Add(this.btnCreateAlias);
            this.Controls.Add(this.btnStopPlay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSaveRoom);
            this.Controls.Add(this.txtRoomId);
            this.Controls.Add(this.btnStartPlay);
            this.Controls.Add(this.btnImportMovie);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "斗鱼电影播放系统";
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwMoviePlayer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnImportMovie;
        private System.Windows.Forms.Button btnStartPlay;
        private System.Windows.Forms.Label lblMovieName;
        private System.Windows.Forms.TextBox txtRoomId;
        private System.Windows.Forms.Button btnSaveRoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStopPlay;
        private System.Windows.Forms.Button btnCreateAlias;
    }
}

