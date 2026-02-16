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
    public partial class quanlysach : Form
    {
        
        public quanlysach()
        {
            InitializeComponent();
            this.txttimkiemqls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txttimkiemqls_KeyDown);
        }
        private void quanlysach_Load(object sender, EventArgs e)
        {
            taisach();
        }
        private void taisach(string tieude = "")
        {
            string sql;
            if (string.IsNullOrEmpty(tieude))
                sql = "SELECT * FROM Sach";
            else
                sql = "SELECT * FROM Sach WHERE TieuDe LIKE N'%" + tieude + "%'";
            dgvsach.DataSource = qltt.ExecuteQuery(sql);
        }
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvsach.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvsach.SelectedRows[0];
                txtmasach.Text = row.Cells["MaSach"].Value.ToString();
                txttensach.Text = row.Cells["TieuDe"].Value.ToString();
                txttacgia.Text = row.Cells["MaTacGia"].Value.ToString();
                txtnxb.Text = row.Cells["MaNhaXuatBan"].Value.ToString();
                txtnam.Text = row.Cells["NamXuatBan"].Value.ToString();
                txtmatheloai.Text = row.Cells["MaTheLoai"].Value.ToString();
                txtisbn.Text = row.Cells["ISBN"].Value.ToString();
                txtsoluong.Text = row.Cells["SoLuong"].Value.ToString();
                txtconlai.Text = row.Cells["SoLuongConLai"].Value.ToString();
            }
        }

        private void btnthemqls_Click(object sender, EventArgs e)
        {
            try
            {
                String sql = "INSERT INTO Sach (TieuDe, MaTacGia, MaNhaXuatBan, NamXuatBan, MaTheLoai, ISBN, SoLuong, SoLuongConLai) VALUES(N'" + txttensach.Text + "', " + txttacgia.Text + ", " + txtnxb.Text + ", " + txtnam.Text + ", " + txtmatheloai.Text + ", '" + txtisbn.Text + "', " + txtsoluong.Text + ", " + txtconlai.Text + ")";
                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Thêm Thành Công");
                taisach();
            }
            catch
            {

            }
        }
        private void btncapnhatqls_Click(object sender, EventArgs e)
        {
            if (txtmasach.Text != "")
            {
                try
                {
                    string sql = "UPDATE Sach SET ";
                    sql += "TieuDe = N'" + txttensach.Text + "', ";
                    sql += "MaTacGia = " + txttacgia.Text + ", ";
                    sql += "MaNhaXuatBan = " + txtnxb.Text + ", ";
                    sql += "NamXuatBan = " + txtnam.Text + ", ";
                    sql += "MaTheLoai = " + txtmatheloai.Text + ", ";
                    sql += "ISBN = '" + txtisbn.Text + "', ";
                    sql += "SoLuong = " + txtsoluong.Text + ", ";
                    sql += "SoLuongConLai = " + txtconlai.Text + " ";
                    sql += "WHERE MaSach = " + txtmasach.Text;
                    qltt.ExecuteNonQuery(sql);
                    MessageBox.Show("Cập Nhật Thành Công");
                    taisach();
                }
                catch
                {
                    MessageBox.Show("Lỗi khi cập nhật. Kiểm tra lại dữ liệu.");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dòng cần sửa.");
            }
        }
        private void tnxoaqls_Click(object sender, EventArgs e)
        {
            if (txtmasach.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn dòng cần xóa.");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM Sach WHERE MaSach = " + txtmasach.Text;
                    qltt.ExecuteNonQuery(sql);
                    MessageBox.Show("Xóa Thành Công");
                    taisach();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("FK_"))
                        MessageBox.Show("Sách đang được mượn, không xóa được.");
                    else
                        MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
                catch
                {
                    MessageBox.Show("Lỗi không xác định khi xóa.");
                }
            }
        }

        private void txttimkiemqls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                taisach(txttimkiemqls.Text);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtisbn_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
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

        private void pbtheloai_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
        }

        private void lbtheloai_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
        }

        private void pbtacgia_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void lbtacgia_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TacGia().ShowDialog();
            this.Show();
        }

        private void pbnhaxuatban_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }

        private void lbnhaxuatban_Click(object sender, EventArgs e)
        {
            this.Hide();
            new NhaXuatBan().ShowDialog();
            this.Show();
        }
       
        private void lbsach_Click(object sender, EventArgs e)
        {

        }

        
    }
}
