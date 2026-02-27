using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class quenmatkhau : Form
    {
        private string maXacNhan = "";
        private string vaiTro = "";
        private bool isReturning = false;

        public quenmatkhau()
        {
            InitializeComponent();
            txtMaXacNhan.Enabled = false;
            txtmk.Enabled = false;
            txtlmk.Enabled = false;
            txtMaXacNhan.KeyDown += txtMaXacNhan_KeyDown;
            txtlmk.KeyDown += txtlmk_KeyDown;
            this.FormClosed += quenmatkhau_FormClosed;
        }

        private void quenmatkhau_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void btnxacnhatqmk_Click(object sender, EventArgs e)
        {
            string username = txttendangnhapqmk.Text.Trim();
            string email = txtemailqmk.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Bạn vui lòng điền đầy đủ Tên đăng nhập và Email nhé!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (KiemTraVaGuiMa("ThuThu", username, email)) return;
            if (KiemTraVaGuiMa("DocGia", username, email)) return;
            MessageBox.Show("Tên đăng nhập này không tồn tại. Bạn kiểm tra lại nhé!", "Sai Tên đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool KiemTraVaGuiMa(string bang, string username, string emailInput)
        {
            string sqlCheckUser = $"SELECT Email FROM {bang} WHERE TenDangNhap = @user";
            SqlParameter[] paramUser = { new SqlParameter("@user", username) };
            DataTable dt = qltt.ExecuteQuery(sqlCheckUser, paramUser);

            if (dt.Rows.Count > 0)
            {
                string emailDB = dt.Rows[0]["Email"].ToString();

                if (emailDB == emailInput)
                {
                    vaiTro = bang;
                    maXacNhan = new Random().Next(100000, 999999).ToString();

                    if (GuiEmail(emailInput, maXacNhan))
                    {
                        MessageBox.Show("Hệ thống đã gửi một mã gồm 6 số về email của bạn.\nBạn hãy kiểm tra hộp thư, nhập mã đó vào ô và nhấn phím Enter nhé!", "Gửi mã thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        txtMaXacNhan.Enabled = true;
                        txtMaXacNhan.Focus();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Email bạn nhập không đúng với email đã đăng ký của tài khoản này.", "Sai Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            return false;
        }

        private bool GuiEmail(string emailNhan, string maOTP)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("tuyenthanhthanh3979@gmail.com");
                mail.To.Add(emailNhan);
                mail.Subject = "Mã xác nhận khôi phục mật khẩu";
                mail.Body = $"Chào bạn,\n\nMã xác nhận 6 số để đổi mật khẩu của bạn là: {maOTP}\n\nVui lòng không chia sẻ mã này cho bất kỳ ai nhé!\n\nTrân trọng.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("tuyenthanhthanh3979@gmail.com", "mdxcxgviflqurfjn");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi gửi email. Bạn kiểm tra lại mạng nhé.\nChi tiết: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
        }

        private void quenmatkhau_Load_1(object sender, EventArgs e)
        {

        }

        private void txtMaXacNhan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMaXacNhan.Enabled)
            {
                if (txtMaXacNhan.Text.Trim() == maXacNhan)
                {
                    MessageBox.Show("Mã xác nhận đúng rồi! Mời bạn nhập mật khẩu mới và xác nhận lại mật khẩu, sau đó nhấn Enter ở ô cuối cùng để lưu nhé.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtMaXacNhan.Enabled = false;
                    txtmk.Enabled = true;
                    txtlmk.Enabled = true;
                    txtmk.Focus();
                }
                else
                {
                    MessageBox.Show("Mã xác nhận sai rồi. Bạn kiểm tra lại thư xem sao nhé!", "Sai mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtlmk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtlmk.Enabled)
            {
                string matKhauMoi = txtmk.Text.Trim();
                string nhapLaiMatKhau = txtlmk.Text.Trim();

                if (string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(nhapLaiMatKhau))
                {
                    MessageBox.Show("Bạn chưa nhập đủ thông tin mật khẩu mới kìa!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (matKhauMoi != nhapLaiMatKhau)
                {
                    MessageBox.Show("Hai mật khẩu không khớp nhau. Bạn gõ lại cẩn thận nhé!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = $"UPDATE {vaiTro} SET MatKhau = @pass WHERE TenDangNhap = @user";
                SqlParameter[] param = {
                    new SqlParameter("@pass", matKhauMoi),
                    new SqlParameter("@user", txttendangnhapqmk.Text.Trim())
                };

                try
                {
                    qltt.ExecuteNonQuery(sql, param);
                    MessageBox.Show("Tuyệt vời! Bạn đã đổi mật khẩu thành công. Giờ hệ thống sẽ đưa bạn về trang đăng nhập nhé.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isReturning = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ôi, có lỗi khi lưu mật khẩu rồi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}