using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class muontra : Form
    {
        public muontra()
        {
            InitializeComponent();
            ComboxMaDG();
            ComboxMaSach();

        }

        private void muontra_Load(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM MuonTra";
            DataTable dt = qltt.ExecuteQuery(sql);
            this.dataGridView1.DataSource = dt;
            XoaTrang();

        }
        private void XoaTrang()
        {
            txtDateMuon.Text = string.Empty;
            txtDateTra.Text = string.Empty;
            Checktra.Checked = false;
            txtGhiChu.Text = string.Empty;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int maMuonTra = Convert.ToInt32(selectedRow.Cells["MaMuonTra"].Value);
                this.lbMaPhieu.Text = $"Mã Phiếu:{maMuonTra}";
                this.cbMaDG.Text = this.dataGridView1.SelectedRows[0].Cells["MaDocGia"].Value.ToString();
                this.cbMaSach.Text = this.dataGridView1.SelectedRows[0].Cells["MaSach"].Value.ToString();
                this.txtDateMuon.Text = this.dataGridView1.SelectedRows[0].Cells["NgayMuon"].Value.ToString();
                this.txtDateTra.Text = this.dataGridView1.SelectedRows[0].Cells["NgayTra"].Value.ToString();
                this.txtGhiChu.Text = this.dataGridView1.SelectedRows[0].Cells["GhiChu"].Value.ToString();
            }
        }
        private void ComboxMaDG()
        {
            try
            {
                string sql = "SELECT MaDocGia FROM MuonTra";

                DataTable dt = qltt.ExecuteQuery(sql);

                cbMaDG.DataSource = dt;
                cbMaDG.ValueMember = "MaDocGia";
                cbMaDG.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ComboxMaSach()
        {
            try
            {
                string sql = "SELECT MaSach FROM MuonTra";

                DataTable dt = qltt.ExecuteQuery(sql);

                cbMaSach.DataSource = dt;
                cbMaSach.ValueMember = "MaSach";
                cbMaSach.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbMaDG.Text) || string.IsNullOrEmpty(cbMaSach.Text) || string.IsNullOrEmpty(txtDateMuon.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin!");
                    return;
                }

                string MaDocGia = cbMaDG.Text.Trim();
                int MaSach = int.Parse(cbMaSach.Text.Trim());
                DateTime NgayMuon = DateTime.Parse(txtDateMuon.Text.Trim());
                string NgayMuonSQL = NgayMuon.ToString("yyyy-MM-dd");

                string NgayTraSQL = "NULL";
                if (!string.IsNullOrEmpty(txtDateTra.Text))
                {
                    DateTime NgayTra = DateTime.Parse(txtDateTra.Text.Trim());
                    NgayTraSQL = $"'{NgayTra:yyyy-MM-dd}'";
                }

                string TrangThai = Checktra.Checked ? "Đã Trả" : "Đang Mượn";
                if (Checktra.Checked && NgayTraSQL == "NULL")
                {
                    NgayTraSQL = $"'{DateTime.Now:yyyy-MM-dd}'";
                }

                string GhiChu = txtGhiChu.Text.Replace("'", "''");

                string sql = $"INSERT INTO MuonTra (MaDocGia, MaSach, NgayMuon, NgayTra, TrangThai, GhiChu) " +
                             $"VALUES (N'{MaDocGia}', {MaSach}, '{NgayMuonSQL}', {NgayTraSQL}, N'{TrangThai}', N'{GhiChu}')";

                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Thêm thành công!");
                muontra_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn dòng cần sửa!");
                return;
            }

            try
            {
                int MaMuonTra = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaMuonTra"].Value);

                string MaDocGia = cbMaDG.Text.Trim();
                int MaSach = int.Parse(cbMaSach.Text.Trim());
                DateTime NgayMuon = DateTime.Parse(txtDateMuon.Text.Trim());
                string NgayMuonSQL = NgayMuon.ToString("yyyy-MM-dd");

                string NgayTraSQL = "NULL";
                if (!string.IsNullOrEmpty(txtDateTra.Text))
                {
                    DateTime NgayTra = DateTime.Parse(txtDateTra.Text.Trim());
                    NgayTraSQL = $"'{NgayTra:yyyy-MM-dd}'";
                }

                string TrangThai = Checktra.Checked ? "Đã Trả" : "Đang Mượn";
                if (Checktra.Checked && NgayTraSQL == "NULL")
                {
                    NgayTraSQL = $"'{DateTime.Now:yyyy-MM-dd}'";
                }

                string GhiChu = txtGhiChu.Text.Replace("'", "''");

                string sql = $"UPDATE MuonTra SET " +
                             $"MaDocGia = N'{MaDocGia}', " +
                             $"MaSach = {MaSach}, " +
                             $"NgayMuon = '{NgayMuonSQL}', " +
                             $"NgayTra = {NgayTraSQL}, " +
                             $"TrangThai = N'{TrangThai}', " +
                             $"GhiChu = N'{GhiChu}' " +
                             $"WHERE MaMuonTra = {MaMuonTra}";

                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Trả sách thành công!");
                muontra_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private static int ExecuteNonQuery(string sql, SqlParameter pMaDocGia, SqlParameter pMaSach, SqlParameter pNgayMuon, SqlParameter pNgayTra, SqlParameter pTrangThai, SqlParameter pGhiChu, SqlParameter pMaMuonTra)
        {
            throw new NotImplementedException();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)// xóa 
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn dòng cần xóa!");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                int MaMuonTra = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaMuonTra"].Value);
                string sql = $"DELETE FROM MuonTra WHERE MaMuonTra = {MaMuonTra}";
                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Xóa thành công!");
                muontra_Load(sender, e);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemMaDG.Text.Trim();
            string sql;

            if (string.IsNullOrEmpty(keyword))
                sql = "SELECT * FROM MuonTra";
            else
                sql = $"SELECT * FROM MuonTra WHERE MaDocGia LIKE N'{keyword}%'";

            dataGridView1.DataSource = qltt.ExecuteQuery(sql);
        }

        private void Checktra_CheckedChanged(object sender, EventArgs e)
        {
            if (Checktra.Checked)
            {
                DateTime ngayMuon = DateTime.Parse(txtDateMuon.Text);
                DateTime ngayTra = DateTime.Now;
                TienPhatCalculator calc = new TienPhatCalculator();
                double tienPhat = calc.TinhTienPhat(ngayMuon, ngayTra);
                txtTienPhat.Text = tienPhat.ToString("N0");

            }
            else
            {
                txtTienPhat.Text = "0";
            }
        }



        private void txtTienPhat_TextChanged(object sender, EventArgs e)
        {

        }
        private void TinhTienPhat()
        {
            try
            {


                DateTime ngayMuon = DateTime.Parse(txtDateMuon.Text);
                DateTime ngayTra = DateTime.Parse(txtDateTra.Text);

                TimeSpan khoangThoiGian = ngayTra - ngayMuon;
                int soNgayMuon = khoangThoiGian.Days;

                double tienPhat = 0;
                if (soNgayMuon > 7)
                {
                    int soNgayTre = soNgayMuon - 7;
                    tienPhat = soNgayTre * 5000;
                }

                txtTienPhat.Text = tienPhat.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính tiền phạt: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtDateMuon_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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

        private void btnbaocao_Click(object sender, EventArgs e)
        {
            this.Hide();
            new baocao().ShowDialog();
            this.Show();
        }

        private void btbntaikhoan_Click(object sender, EventArgs e)
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
        
    }
}
