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
    public partial class baocao : Form
    {
        public baocao()
        {
            InitializeComponent();
            checkdangmuon.Checked = true;
        }
       
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            string sql = "";
            DataTable dt;

            if (checkyeuthich.Checked)
            {
                sql = @"
            SELECT 
                s.MaSach,
                s.TieuDe,
                COUNT(mt.MaSach) AS SoLanMuon
            FROM MuonTra mt
            INNER JOIN Sach s ON mt.MaSach = s.MaSach
            GROUP BY s.MaSach, s.TieuDe
            HAVING COUNT(mt.MaSach) >= 3  
            ORDER BY SoLanMuon DESC";
            }
            else if (checkdangmuon.Checked)
            {
                sql = "SELECT * FROM MuonTra WHERE TrangThai = N'Đang Mượn'";
            }
            else
            {
                sql = "SELECT * FROM MuonTra WHERE TrangThai = N'Đã Trả'";
            }

            dt = qltt.ExecuteQuery(sql);
            GridBaoCao.DataSource = dt;
            lbTong.Text = $"Tổng số sách: {dt.Rows.Count}";

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btntrangchu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new trangchu().ShowDialog();
            this.Show();
        }

        private void btndangxuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            new dangnhap().ShowDialog();
            this.Close();
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
        

        private void btnThoat_Click(object sender, EventArgs e)
        {

        }
    }
}
