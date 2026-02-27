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
    public partial class dangki : Form
    {
        private bool isReturning = false;

        public dangki()
        {
            InitializeComponent();
            dtpngaysinhdangki.Value = DateTime.Now.AddYears(-18);
            dtpngaysinhdangki.MaxDate = DateTime.Now;
            txtmatkhaudangki.UseSystemPasswordChar = true;
            this.FormClosed += dangki_FormClosed;
        }

        private void dangki_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private string SinhMaDocGiaMoi()
        {
            string sql = "SELECT TOP 1 MaDocGia FROM DocGia ORDER BY MaDocGia DESC";
            DataTable dt = qltt.ExecuteQuery(sql);
            string maMoi = "GN0001";
            if (dt.Rows.Count > 0)
            {
                string maCu = dt.Rows[0]["MaDocGia"].ToString();
                int so = int.Parse(maCu.Substring(2));
                so++;
                maMoi = "GN" + so.ToString("D4");
            }
            return maMoi;
        }

        private void btndangki_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txttendangnhapdangki.Text) ||
                string.IsNullOrWhiteSpace(txthovatendangki.Text) ||
                string.IsNullOrWhiteSpace(txtemaildangki.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin bắt buộc!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string matKhau = txtmatkhaudangki.Text;

            if (!kiemtramatkhau(matKhau))
            {
                MessageBox.Show("Mật khẩu yếu. Vui lòng đặt mật khẩu mạnh hơn (ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string gioiTinh = rbnamdangki.Checked ? "Nam" : (rbnudangki.Checked ? "Nữ" : "");
            string maDocGiaMoi = SinhMaDocGiaMoi();

            string sql = @"
            INSERT INTO DocGia
            (MaDocGia, HoTen, TenDangNhap, MatKhau, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email)
            VALUES
            (@MaDocGia, @HoTen, @TenDangNhap, @MatKhau, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@MaDocGia", maDocGiaMoi),
                new SqlParameter("@HoTen", txthovatendangki.Text.Trim()),
                new SqlParameter("@TenDangNhap", txttendangnhapdangki.Text.Trim()),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@NgaySinh", dtpngaysinhdangki.Value),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@DiaChi", txtdiachidangki.Text.Trim()),
                new SqlParameter("@SoDienThoai", txtsodienthoaidangki.Text.Trim()),
                new SqlParameter("@Email", txtemaildangki.Text.Trim())
            };

            try
            {
                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Đăng ký thành công! Hệ thống sẽ chuyển bạn về trang Đăng nhập.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                isReturning = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng ký thất bại! Có thể Tên đăng nhập hoặc Email này đã được sử dụng.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool kiemtramatkhau(string password)
        {
            if (password.Length < 8)
                return false;
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"[0-9]");
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':;{}/|[\]\\]");
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
        }
    }
}