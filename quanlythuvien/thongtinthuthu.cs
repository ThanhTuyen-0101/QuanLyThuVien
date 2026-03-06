using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class thongtinthuthu : Form
    {
        private bool isReturning = false;

        public thongtinthuthu()
        {
            InitializeComponent();
            this.FormClosed += thongtinthuthu_FormClosed;
        }

        private void thongtinthuthu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void TaiThongTinCaNhanThuThu()
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaThuThu))
            {
                MessageBox.Show("Không có thông tin thủ thư để hiển thị. Vui lòng đăng nhập.", "Lỗi");
                isReturning = true;
                this.Close();
                return;
            }
            try
            {
                string sql = "SELECT MaThuThu, HoTen, DiaChi, SoDienThoai, Email, NgaySinh, GioiTinh FROM ThuThu WHERE MaThuThu = @MaThuThu";
                SqlParameter[] param = { new SqlParameter("@MaThuThu", thongtindangnhap.MaThuThu) };

                DataTable dt = qltt.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtmathuthu2.Text = dr["MaThuThu"].ToString();
                    txttenthuthu2.Text = dr["HoTen"].ToString();
                    txtdiachi2.Text = dr["DiaChi"].ToString();
                    txtsodienthoai2.Text = dr["SoDienThoai"].ToString();
                    txtemail2.Text = dr["Email"].ToString();
                    if (dr["NgaySinh"] != DBNull.Value)
                    {
                        txtngaysinh2.Text = Convert.ToDateTime(dr["NgaySinh"]).ToString("dd/MM/yyyy");
                    }

                    txtgioitinh2.Text = dr["GioiTinh"].ToString();
                    txtmathuthu2.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin.", "Thông báo");
                    isReturning = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
                isReturning = true;
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmathuthu2.Text))
            {
                MessageBox.Show("Chưa có mã thủ thư để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txttenthuthu2.Text) ||
                string.IsNullOrWhiteSpace(txtdiachi2.Text) ||
                string.IsNullOrWhiteSpace(txtsodienthoai2.Text) ||
                string.IsNullOrWhiteSpace(txtemail2.Text) ||
                string.IsNullOrWhiteSpace(txtngaysinh2.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin (Họ tên, Địa chỉ, SĐT, Email, Ngày sinh)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(txtsodienthoai2.Text.Trim(), phonePattern))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!\nVui lòng nhập đúng 10 số và bắt đầu bằng số 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtsodienthoai2.Focus();
                return;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtemail2.Text.Trim(), emailPattern))
            {
                MessageBox.Show("Email không hợp lệ!\nVui lòng nhập đúng định dạng (VD: ten@gmail.com).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtemail2.Focus();
                return;
            }

            DateTime ngaySinh;
            if (!DateTime.TryParseExact(txtngaysinh2.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ!\nVui lòng nhập đúng định dạng dd/MM/yyyy (VD: 01/01/1980).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtngaysinh2.Focus();
                return;
            }

            try
            {
                string sql = @"
            UPDATE ThuThu SET 
                HoTen = @HoTen, 
                DiaChi = @DiaChi, 
                SoDienThoai = @SoDienThoai, 
                Email = @Email, 
                NgaySinh = @NgaySinh, 
                GioiTinh = @GioiTinh 
            WHERE MaThuThu = @MaThuThu";

                SqlParameter[] parameters = {
            new SqlParameter("@HoTen", txttenthuthu2.Text.Trim()),
            new SqlParameter("@DiaChi", txtdiachi2.Text.Trim()),
            new SqlParameter("@SoDienThoai", txtsodienthoai2.Text.Trim()),
            new SqlParameter("@Email", txtemail2.Text.Trim()),
            new SqlParameter("@NgaySinh", ngaySinh),
            new SqlParameter("@GioiTinh", txtgioitinh2.Text.Trim()),
            new SqlParameter("@MaThuThu", txtmathuthu2.Text.Trim())
        };

                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiThongTinCaNhanThuThu();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    MessageBox.Show("Email hoặc thông tin duy nhất này đã tồn tại trong hệ thống. Vui lòng kiểm tra lại!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (sqlEx.Number == 547)
                {
                    string errorMsg = sqlEx.Message;

                    if (errorMsg.Contains("CK_ThuThu_GioiTinh"))
                    {
                        MessageBox.Show("Giới tính không hợp lệ! Vui lòng chỉ nhập 'Nam', 'Nữ' hoặc 'Khác'.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_ThuThu_NgaySinh"))
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ! Ngày sinh không thể lớn hơn ngày hiện tại.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_ThuThu_SDT"))
                    {
                        MessageBox.Show("Số điện thoại bị từ chối bởi cơ sở dữ liệu. Vui lòng kiểm tra lại.", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (errorMsg.Contains("CK_ThuThu_Email"))
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
                MessageBox.Show("Đã xảy ra lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        }

        private void thongtinthuthu_Load_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaThuThu))
            {
                MessageBox.Show("Vui lòng đăng nhập lại.");
                isReturning = true;
                this.Close();
                return;
            }
            TaiThongTinCaNhanThuThu();
        }
    }
}