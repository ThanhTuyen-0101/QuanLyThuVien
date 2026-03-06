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
using System.Text.RegularExpressions;

namespace quanlythuvien
{
    public partial class thongtindocgia : Form
    {
        private bool isReturning = false;

        public thongtindocgia()
        {
            InitializeComponent();
            this.FormClosed += thongtindocgia_FormClosed;
        }

        private void thongtindocgia_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void TaiThongTinCaNhan()
        {
            string sql = "SELECT MaDocGia, HoTen, DiaChi, SoDienThoai, Email, GioiTinh, NgaySinh FROM DocGia WHERE MaDocGia = N'" + thongtindangnhap.MaDocGia.Replace("'", "''") + "'";
            DataTable dt = qltt.ExecuteQuery(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtmadocgia1.Text = row["MaDocGia"] + "";
                txttendocgia1.Text = row["HoTen"] + "";
                txtdiachi1.Text = row["DiaChi"] + "";
                txtsodienthoai1.Text = row["SoDienThoai"] + "";
                txtemail1.Text = row["Email"] + "";
                txtgioitinh1.Text = row["GioiTinh"] + "";

                if (row["NgaySinh"] != DBNull.Value && row["NgaySinh"].ToString() != "")
                {
                    DateTime ns = DateTime.Parse(row["NgaySinh"].ToString());
                    txtngaysinh1.Text = ns.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtngaysinh1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin độc giả.");
            }
        }

        private void LoadLichSuMuonTra()
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaDocGia))
            {
                MessageBox.Show("Mã độc giả không hợp lệ. Vui lòng đăng nhập lại!");
                isReturning = true;
                this.Close();
                return;
            }
            string sql = @"
                SELECT mt.MaDocGia AS [Mã Độc Giả], 
                       dg.HoTen AS [Họ Tên], 
                       s.TieuDe AS [Tên Sách], 
                       mt.NgayMuon AS [Ngày Mượn], 
                       mt.NgayTra AS [Ngày Trả], 
                       mt.TrangThai AS [Trạng Thái], 
                       mt.GhiChu AS [Ghi Chú]
                FROM MuonTra mt 
                JOIN DocGia dg ON mt.MaDocGia = dg.MaDocGia 
                JOIN Sach s ON mt.MaSach = s.MaSach 
                WHERE mt.MaDocGia = N'" + thongtindangnhap.MaDocGia + "'";

            dgv.DataSource = qltt.ExecuteQuery(sql);
        }

        private void TimSachTheoTen(string tenSach)
        {
            string sql = @"
                SELECT s.MaSach AS [Mã Sách], 
                       s.TieuDe AS [Tiêu Đề], 
                       t.TenTacGia AS [Tác Giả], 
                       tl.TenTheLoai AS [Thể Loại], 
                       nxb.TenNhaXuatBan AS [Nhà Xuất Bản], 
                       s.NamXuatBan AS [Năm Xuất Bản], 
                       s.ISBN AS [Mã ISBN], 
                       s.SoLuong AS [Số Lượng], 
                       s.SoLuongConLai AS [Còn Lại]
                FROM Sach s 
                JOIN TacGia t ON s.MaTacGia = t.MaTacGia 
                JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai 
                JOIN NhaXuatBan nxb ON s.MaNhaXuatBan = nxb.MaNhaXuatBan 
                WHERE s.TieuDe LIKE N'%" + tenSach.Replace("'", "''") + "%'";

            DataTable dt = qltt.ExecuteQuery(sql);
            dgv.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sách phù hợp.");
            }
        }

        private void btncapnhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtmadocgia1.Text))
            {
                MessageBox.Show("Bạn chưa có thông tin để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txttendocgia1.Text) || string.IsNullOrWhiteSpace(txtdiachi1.Text) ||
                string.IsNullOrWhiteSpace(txtsodienthoai1.Text) || string.IsNullOrWhiteSpace(txtemail1.Text) ||
                string.IsNullOrWhiteSpace(txtngaysinh1.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(txtsodienthoai1.Text.Trim(), phonePattern))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtsodienthoai1.Focus();
                return;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtemail1.Text.Trim(), emailPattern))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtemail1.Focus();
                return;
            }

            DateTime ngaySinh;
            if (!DateTime.TryParseExact(txtngaysinh1.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ! (dd/MM/yyyy)", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtngaysinh1.Focus();
                return;
            }

            try
            {
                string sql = @"
            UPDATE DocGia 
            SET HoTen = @HoTen,
                DiaChi = @DiaChi,
                NgaySinh = @NgaySinh,   
                SoDienThoai = @SoDienThoai,
                Email = @Email,
                GioiTinh = @GioiTinh
            WHERE MaDocGia = @MaDocGia";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@HoTen", txttendocgia1.Text.Trim()),
                    new SqlParameter("@NgaySinh", ngaySinh),
                    new SqlParameter("@DiaChi", txtdiachi1.Text.Trim()),
                    new SqlParameter("@SoDienThoai", txtsodienthoai1.Text.Trim()),
                    new SqlParameter("@Email", txtemail1.Text.Trim()),
                    new SqlParameter("@GioiTinh", txtgioitinh1.Text.Trim()),
                    new SqlParameter("@MaDocGia", txtmadocgia1.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);

                MessageBox.Show("Cập Nhật Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiThongTinCaNhan();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    MessageBox.Show("Email hoặc thông tin duy nhất đã tồn tại cho một độc giả khác!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (sqlEx.Number == 547)
                {
                    string errorMsg = sqlEx.Message;

                    if (errorMsg.Contains("CK_DocGia_GioiTinh"))
                    {
                        MessageBox.Show("Giới tính không hợp lệ! Vui lòng chỉ nhập 'Nam', 'Nữ' hoặc 'Khác'.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_DocGia_NgaySinh"))
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ! Ngày sinh không thể lớn hơn ngày hiện tại.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_DocGia_SDT"))
                    {
                        MessageBox.Show("Số điện thoại bị từ chối bởi cơ sở dữ liệu. Vui lòng kiểm tra lại.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_DocGia_Email"))
                    {
                        MessageBox.Show("Định dạng Email bị từ chối bởi cơ sở dữ liệu.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu nhập vào vi phạm quy định của cơ sở dữ liệu. Vui lòng kiểm tra lại.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi cơ sở dữ liệu SQL: " + sqlEx.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void txttimsach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tensach = txttimsach.Text.Trim();
                if (tensach == "")
                    LoadLichSuMuonTra();
                else
                    TimSachTheoTen(tensach);
                e.SuppressKeyPress = true;
            }
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            string maDocGia = txtmadocgia1.Text;
            if (string.IsNullOrEmpty(maDocGia))
            {
                MessageBox.Show("Mã độc giả không hợp lệ.");
                return;
            }
            this.Hide();
            new GiaHanSachDG(maDocGia).ShowDialog();
            this.Show();
        }

        private void dangxuat_Click_1(object sender, EventArgs e)
        {
            isReturning = true;
            Application.Restart();
        }

        private void thongtindocgia_Load_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaDocGia))
            {
                MessageBox.Show("Vui lòng đăng nhập lại.");
                isReturning = true;
                this.Close();
                return;
            }
            TaiThongTinCaNhan();
            LoadLichSuMuonTra();
        }
    }
}