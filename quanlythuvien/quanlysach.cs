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
        private bool isReturning = false;

        public quanlysach()
        {
            InitializeComponent();
            this.txttimkiemqls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txttimkiemqls_KeyDown);
            this.FormClosed += quanlysach_FormClosed;
        }

        private void quanlysach_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void taisach(string tieude = "")
        {
            string sql;
            DataTable dt;
            if (string.IsNullOrEmpty(tieude))
            {
                sql = @"SELECT MaSach AS [MÃ SÁCH], TieuDe AS [TIÊU ĐỀ], MaTacGia AS [MÃ TÁC GIẢ], 
                               MaNhaXuatBan AS [MÃ NHÀ XUẤT BẢN], NamXuatBan AS [NĂM XUẤT BẢN], 
                               MaTheLoai AS [MÃ THỂ LOẠI], ISBN AS [ISBN], SoLuong AS [SỐ LƯỢNG], 
                               SoLuongConLai AS [CÒN LẠI] FROM Sach";
                dt = qltt.ExecuteQuery(sql);
            }
            else
            {
                sql = @"SELECT MaSach AS [MÃ SÁCH], TieuDe AS [TIÊU ĐỀ], MaTacGia AS [MÃ TÁC GIẢ], 
                               MaNhaXuatBan AS [MÃ NHÀ XUẤT BẢN], NamXuatBan AS [NĂM XUẤT BẢN], 
                               MaTheLoai AS [MÃ THỂ LOẠI], ISBN AS [ISBN], SoLuong AS [SỐ LƯỢNG], 
                               SoLuongConLai AS [CÒN LẠI] FROM Sach WHERE TieuDe LIKE @TieuDe";
                SqlParameter[] param = { new SqlParameter("@TieuDe", "%" + tieude + "%") };
                dt = qltt.ExecuteQuery(sql, param);
            }

            dgvsach.DataSource = dt;
            dgvsach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnthemqls_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"INSERT INTO Sach (TieuDe, MaTacGia, MaNhaXuatBan, NamXuatBan, MaTheLoai, ISBN, SoLuong, SoLuongConLai) 
                               VALUES (@TieuDe, @MaTacGia, @MaNhaXuatBan, @NamXuatBan, @MaTheLoai, @ISBN, @SoLuong, @SoLuongConLai)";

                SqlParameter[] parameters = {
                    new SqlParameter("@TieuDe", txttensach.Text.Trim()),
                    new SqlParameter("@MaTacGia", txttacgia.Text.Trim()),
                    new SqlParameter("@MaNhaXuatBan", txtnxb.Text.Trim()),
                    new SqlParameter("@NamXuatBan", txtnam.Text.Trim()),
                    new SqlParameter("@MaTheLoai", txtmatheloai.Text.Trim()),
                    new SqlParameter("@ISBN", txtisbn.Text.Trim()),
                    new SqlParameter("@SoLuong", txtsoluong.Text.Trim()),
                    new SqlParameter("@SoLuongConLai", txtconlai.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Thêm Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                taisach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sách, vui lòng kiểm tra lại các trường mã số.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncapnhatqls_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmasach.Text))
            {
                MessageBox.Show("Bạn chưa chọn dòng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"UPDATE Sach SET 
                               TieuDe = @TieuDe, 
                               MaTacGia = @MaTacGia, 
                               MaNhaXuatBan = @MaNhaXuatBan, 
                               NamXuatBan = @NamXuatBan, 
                               MaTheLoai = @MaTheLoai, 
                               ISBN = @ISBN, 
                               SoLuong = @SoLuong, 
                               SoLuongConLai = @SoLuongConLai 
                               WHERE MaSach = @MaSach";

                SqlParameter[] parameters = {
                    new SqlParameter("@TieuDe", txttensach.Text.Trim()),
                    new SqlParameter("@MaTacGia", txttacgia.Text.Trim()),
                    new SqlParameter("@MaNhaXuatBan", txtnxb.Text.Trim()),
                    new SqlParameter("@NamXuatBan", txtnam.Text.Trim()),
                    new SqlParameter("@MaTheLoai", txtmatheloai.Text.Trim()),
                    new SqlParameter("@ISBN", txtisbn.Text.Trim()),
                    new SqlParameter("@SoLuong", txtsoluong.Text.Trim()),
                    new SqlParameter("@SoLuongConLai", txtconlai.Text.Trim()),
                    new SqlParameter("@MaSach", txtmasach.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Cập Nhật Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                taisach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật. Kiểm tra lại dữ liệu nhập vào.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tnxoaqls_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmasach.Text))
            {
                MessageBox.Show("Bạn chưa chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa cuốn sách này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM Sach WHERE MaSach = @MaSach";
                    SqlParameter[] param = { new SqlParameter("@MaSach", txtmasach.Text.Trim()) };

                    qltt.ExecuteNonQuery(sql, param);
                    MessageBox.Show("Xóa Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    taisach();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("FK_"))
                        MessageBox.Show("Sách đang được mượn hoặc có dữ liệu liên quan, không thể xóa.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi không xác định khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txttimkiemqls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                taisach(txttimkiemqls.Text.Trim());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
        }

        private void btnsach_Click(object sender, EventArgs e)
        {

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
            isReturning = true;
            Application.Restart();
        }

        private void lbtheloai_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
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

        private void lbsach_Click(object sender, EventArgs e)
        {

        }

        private void dgvsach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvsach.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvsach.SelectedRows[0];
                txtmasach.Text = row.Cells["MÃ SÁCH"].Value.ToString();
                txttensach.Text = row.Cells["TIÊU ĐỀ"].Value.ToString();
                txttacgia.Text = row.Cells["MÃ TÁC GIẢ"].Value.ToString();
                txtnxb.Text = row.Cells["MÃ NHÀ XUẤT BẢN"].Value.ToString();
                txtnam.Text = row.Cells["NĂM XUẤT BẢN"].Value.ToString();
                txtmatheloai.Text = row.Cells["MÃ THỂ LOẠI"].Value.ToString();
                txtisbn.Text = row.Cells["ISBN"].Value.ToString();
                txtsoluong.Text = row.Cells["SỐ LƯỢNG"].Value.ToString();
                txtconlai.Text = row.Cells["CÒN LẠI"].Value.ToString();
            }
        }

        private void quanlysach_Load(object sender, EventArgs e)
        {
            dgvsach.Font = new Font("Times New Roman", 10);
            taisach();
        }

        private void pbsach_Click(object sender, EventArgs e)
        {

        }

        private void pbtheloai_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new TheLoai().ShowDialog();
            this.Show();
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
    }
}