using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Douyu.Client
{
    public partial class AliasForm : Form
    {
        public AliasForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtRoom.Text.Trim().Length == 0) {
                MessageBox.Show("请填入房间号!", "添加电影别名", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if (txtMovieName.Text.Trim().Length == 0) {
                MessageBox.Show("请填入电影名字!", "添加电影别名", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMovieAlias.Text.Trim().Length == 0) {
                MessageBox.Show("请填入电影别名", "添加电影别名", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DbService.SaveAliasName(int.Parse(txtRoom.Text), txtMovieName.Text.Trim(), txtMovieAlias.Text.Trim());
        }
    }
}
