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
    public partial class TheLoai : Form
    {
        private bool isReturning = false;

        public TheLoai()
        {
            InitializeComponent();
            this.FormClosed += TheLoai_FormClosed;
        }

        private void TheLoai_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
        }

        private void btndangxuat_Click(object sender, EventArgs e)
        {
            isReturning = true;
            Application.Restart();
        }

        private void btnsach_Click(object sender, EventArgs e)
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

        private void lbsach_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }

        private void lbtacgia_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void lbnhaxuatban_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pbsach_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pbtacgia_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void pbnhaxuatban_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void TheLoai_Load_1(object sender, EventArgs e)
        {
            string sql = "SELECT MaTheLoai AS [MÃ THỂ LOẠI], TenTheLoai AS [TÊN THỂ LOẠI] FROM TheLoai";
            DataTable dt = qltt.ExecuteQuery(sql);

            dgvsach.DataSource = dt;
            dgvsach.Font = new Font("Times New Roman", 10);
            dgvsach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}