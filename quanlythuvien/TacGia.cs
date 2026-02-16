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
    public partial class TacGia : Form
    {
        public TacGia()
        {
            InitializeComponent();
        }
        private void TacGia_Load(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM TacGia";
            DataTable dt = qltt.ExecuteQuery(sql);
            this.dgvsach.DataSource = dt;
            dgvsach.DataSource = dt;
        }

        private void quảnLýSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Show();
        }
        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new thongtinthuthu().ShowDialog();
            this.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new trangchu().ShowDialog();
            this.Show();
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

        private void btnaocao_Click(object sender, EventArgs e)
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

        private void btndangxuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            new dangnhap().ShowDialog();
            this.Close();
        }

        private void pbsach_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Close();
        }

        private void lbsach_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quanlysach().ShowDialog();
            this.Close();
        }

        private void pbtheloai_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Close();
        }

        private void lbtheloai_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Close();
        }

        private void pbnhaxuatban_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Close();
        }

        private void lbnhaxuatban_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Close();
        }
        

        private void label12_Click(object sender, EventArgs e)
        {

        }

       
    }
}
