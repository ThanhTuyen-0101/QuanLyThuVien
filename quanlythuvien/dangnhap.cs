using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class dangnhap : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private void SetPlaceholder(Control control, string text)
        {
            SendMessage(control.Handle, EM_SETCUEBANNER, 0, text);
        }

        public dangnhap()
        {
            InitializeComponent();
            this.btndagnhap.Click += btndagnhap_Click;
            this.FormClosed += dangnhap_FormClosed;
        }

        private void dangnhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LuuThongTinDangNhap(string user, string pass)
        {
            if (rdbnmk.Checked)
            {
                Properties.Settings.Default.Username = user;
                Properties.Settings.Default.Password = pass;
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.RememberMe = false;
            }
            Properties.Settings.Default.Save();
        }

        private void btndagnhap_Click(object sender, EventArgs e)
        {
            string username = txttendangnhap.Text.Trim();
            string password = txtmatkhaudangnhap.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.");
                return;
            }

            string sqlThuThu = @"SELECT MaThuThu FROM ThuThu WHERE TenDangNhap = @user AND MatKhau = @pass";
            SqlParameter[] paramThuThu = {
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password)
            };

            DataTable dt = qltt.ExecuteQuery(sqlThuThu, paramThuThu);
            if (dt.Rows.Count > 0)
            {
                LuuThongTinDangNhap(username, password);

                thongtindangnhap.MaThuThu = dt.Rows[0]["MaThuThu"].ToString();
                thongtindangnhap.VaiTro = "ThuThu";

                MessageBox.Show("Xin chào Thủ thư!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new trangchu().ShowDialog();
                this.Close();
                return;
            }

            string sqlDocGia = @"SELECT MaDocGia FROM DocGia WHERE TenDangNhap = @user AND MatKhau = @pass";
            SqlParameter[] paramDG = {
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password)
            };

            dt = qltt.ExecuteQuery(sqlDocGia, paramDG);
            if (dt.Rows.Count > 0)
            {
                LuuThongTinDangNhap(username, password);

                thongtindangnhap.MaDocGia = dt.Rows[0]["MaDocGia"].ToString();
                thongtindangnhap.VaiTro = "DocGia";

                MessageBox.Show("Xin chào Độc giả!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new thongtindocgia().ShowDialog();
                this.Close();
                return;
            }

            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
            txtmatkhaudangnhap.Clear();
        }

        private void btnDK_Click(object sender, EventArgs e)
        {
            this.Hide();
            new dangki().ShowDialog();
            this.Show();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new quenmatkhau().ShowDialog();
            this.Show();
        }

        private void dangnhap_Load_1(object sender, EventArgs e)
        {
            SetPlaceholder(txttendangnhap, "Nhập tên đăng nhập");
            SetPlaceholder(txtmatkhaudangnhap, "Nhập mật khẩu");

            if (Properties.Settings.Default.RememberMe)
            {
                txttendangnhap.Text = Properties.Settings.Default.Username;
                txtmatkhaudangnhap.Text = Properties.Settings.Default.Password;
                rdbnmk.Checked = true;
            }

            this.ActiveControl = label2;
        }
    }
}