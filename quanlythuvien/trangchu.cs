using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class trangchu : Form
    {
        private bool isReturning = false;

        public trangchu()
        {
            InitializeComponent();
            this.FormClosed += trangchu_FormClosed;
        }

        private void trangchu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {

        }

        private void btndangxuat_Click(object sender, EventArgs e)
        {
            isReturning = true;
            Application.Restart();
        }

        private void btbnsach_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }

        private void btndocgia_Click(object sender, EventArgs e)
        {
            this.Hide();
            new docgia().ShowDialog();
            this.Show();
        }

        private void btnmuontra_Click(object sender, EventArgs e)
        {
            this.Hide();
            new muontra().ShowDialog();
            this.Show();
        }

        private void btnbaocao_Click(object sender, EventArgs e)
        {
            this.Hide();
            new baocao().ShowDialog();
            this.Show();
        }

        private void btntaikhoan_Click(object sender, EventArgs e)
        {
            this.Hide();
            new thongtinthuthu().ShowDialog();
            this.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Hide();
            new docgia().ShowDialog();
            this.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            new docgia().ShowDialog();
            this.Show();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
            new muontra().ShowDialog();
            this.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            new muontra().ShowDialog();
            this.Show();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new baocao().ShowDialog();
            this.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new baocao().ShowDialog();
            this.Show();
        }
    }
}