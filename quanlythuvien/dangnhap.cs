using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class dangnhap : Form
    {
        
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        // Hàm rút gọn để gọi cho tiện
        private void SetPlaceholder(Control control, string text)
        {
            SendMessage(control.Handle, EM_SETCUEBANNER, 0, text);
        }
        // =========================================================================

        public dangnhap()
        {
            InitializeComponent();
            this.btndagnhap.Click += btndagnhap_Click;
        }

        private void dangnhap_Load(object sender, EventArgs e)
        {
            // 2. GỌI HÀM SET PLACEHOLDER TẠI ĐÂY
            // Lưu ý: Placeholder sẽ tự biến mất khi bạn nhập chữ
            SetPlaceholder(txttendangnhap, "Nhập tên đăng nhập");
            SetPlaceholder(txtmatkhaudangnhap, "Nhập mật khẩu");

            // Mẹo: Đôi khi winform tự focus vào ô đầu tiên làm mất placeholder
            // Dòng này giúp focus vào nút đăng nhập hoặc hình ảnh để hiện rõ placeholder ở 2 ô kia
            this.ActiveControl = label2;
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
            string sqlThuThu = @"
        SELECT MaThuThu 
        FROM ThuThu 
        WHERE TenDangNhap = @user AND MatKhau = @pass";

            SqlParameter[] paramThuThu =
            {
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password)
            };

            // Lưu ý: Bạn cần đảm bảo biến 'qltt' đã được khởi tạo ở đâu đó trong class này hoặc static
            // Nếu code cũ chạy được thì giữ nguyên, mình chỉ chèn placeholder thôi.
            DataTable dt = qltt.ExecuteQuery(sqlThuThu, paramThuThu);
            if (dt.Rows.Count > 0)
            {
                thongtindangnhap.MaThuThu = dt.Rows[0]["MaThuThu"].ToString();
                thongtindangnhap.VaiTro = "ThuThu";

                MessageBox.Show("Xin chào Thủ thư!", "Đăng nhập thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new trangchu().ShowDialog();
                this.Close();
                return;
            }
            string sqlDocGia = @"
        SELECT MaDocGia 
        FROM DocGia 
        WHERE TenDangNhap = @user AND MatKhau = @pass";

            SqlParameter[] paramDG =
            {
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password)
            };

            dt = qltt.ExecuteQuery(sqlDocGia, paramDG);

            if (dt.Rows.Count > 0)
            {
                thongtindangnhap.MaDocGia = dt.Rows[0]["MaDocGia"].ToString();
                thongtindangnhap.VaiTro = "DocGia";

                MessageBox.Show("Xin chào Độc giả!", "Đăng nhập thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new thongtindocgia().ShowDialog();
                this.Close();
                return;
            }
            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
            txtmatkhaudangnhap.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new quenmatkhau().ShowDialog();
            this.Show();
        }
        

        private void btnDK_Click(object sender, EventArgs e)
        {

        }
    }
}
