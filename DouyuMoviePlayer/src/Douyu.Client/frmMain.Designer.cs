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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMovieName = new System.Windows.Forms.Label();
            this.btnStartPlay = new System.Windows.Forms.Button();
            this.btnStopPlay = new System.Windows.Forms.Button();
            this.tmrScrollPlayTips = new System.Windows.Forms.Timer(this.components);
            this.tmrScrollScoreTips = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // btnStartPlay
            // 
            this.btnStartPlay.Location = new System.Drawing.Point(296, 23);
            this.btnStartPlay.Name = "btnStartPlay";
            this.btnStartPlay.Size = new System.Drawing.Size(121, 35);
            this.btnStartPlay.TabIndex = 4;
            this.btnStartPlay.Text = "开始播放";
            this.btnStartPlay.UseVisualStyleBackColor = true;
            this.btnStartPlay.Click += new System.EventHandler(this.btnStartPlay_Click);
            // 
            // btnStopPlay
            // 
            this.btnStopPlay.Location = new System.Drawing.Point(296, 65);
            this.btnStopPlay.Name = "btnStopPlay";
            this.btnStopPlay.Size = new System.Drawing.Size(121, 35);
            this.btnStopPlay.TabIndex = 8;
            this.btnStopPlay.Text = "停止播放";
            this.btnStopPlay.UseVisualStyleBackColor = true;
            this.btnStopPlay.Click += new System.EventHandler(this.btnStopPlay_Click);
            // 
            // tmrScrollPlayTips
            // 
            this.tmrScrollPlayTips.Interval = 3000;
            this.tmrScrollPlayTips.Tick += new System.EventHandler(this.tmrScrollPlayTips_Tick);
            // 
            // tmrScrollScoreTips
            // 
            this.tmrScrollScoreTips.Interval = 5000;
            this.tmrScrollScoreTips.Tick += new System.EventHandler(this.tmrScrollScoreTips_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 112);
            this.Controls.Add(this.btnStopPlay);
            this.Controls.Add(this.btnStartPlay);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "斗鱼点播";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStartPlay;
        private System.Windows.Forms.Label lblMovieName;
        private System.Windows.Forms.Button btnStopPlay;
        private System.Windows.Forms.Timer tmrScrollPlayTips;
        private System.Windows.Forms.Timer tmrScrollScoreTips;
    }
}

