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
    public partial class quenmatkhau : Form
    {
        
        public quenmatkhau()
        {
            InitializeComponent();
            dtpngaysinhqmk.Value = DateTime.Now.AddYears(-18);
            dtpngaysinhqmk.MaxDate = DateTime.Now;
            txtmatkhauqmk.Enabled = false;
            txtmatkhauqmk.KeyDown += txtmatkhauqmk_KeyDown;
        }
        private void quenmatkhau_Load(object sender, EventArgs e)
        {
        }
        private void btnxacnhatqmk_Click(object sender, EventArgs e)
        {
            if (!rbnamqmk.Checked && !rbnuqmk.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return;
            }
            string gioiTinh = rbnamqmk.Checked ? "Nam" : "Nữ";
            string ngaySinh = dtpngaysinhqmk.Value.ToString("yyyy-MM-dd");
            string dieuKien =
                "TenDangNhap = N'" + txttendangnhapqmk.Text + "' AND " +
                "HoTen = N'" + txthovatenqmk.Text + "' AND " +
                "Email = N'" + txtemailqmk.Text + "' AND " +
                "SoDienThoai = N'" + txtsodienthoaiqmk.Text + "' AND " +
                "NgaySinh = '" + ngaySinh + "' AND " +
                "GioiTinh = N'" + gioiTinh + "'";
            string sqlDocGia = "SELECT * FROM DocGia WHERE " + dieuKien;
            DataTable dt = qltt.ExecuteQuery(sqlDocGia);
            if (dt != null && dt.Rows.Count > 0)
            {
                txtmatkhauqmk.Enabled = true;
                txtmatkhauqmk.Tag = "DocGia";
                txtmatkhauqmk.Focus();
                MessageBox.Show("Xác minh thành công (DocGia).");
            }
            else
            {
                string sqlThuThu = "SELECT * FROM ThuThu WHERE " + dieuKien;
                DataTable dtThuThu = qltt.ExecuteQuery(sqlThuThu);

                if (dtThuThu != null && dtThuThu.Rows.Count > 0)
                {
                    txtmatkhauqmk.Enabled = true;
                    txtmatkhauqmk.Tag = "ThuThu";
                    txtmatkhauqmk.Focus();
                    MessageBox.Show("Xác minh thành công (ThuThu).");
                }
                else
                {
                    txtmatkhauqmk.Enabled = false;
                    txtmatkhauqmk.Tag = null;
                    MessageBox.Show("Thông tin không khớp.", "Thông báo");
                }
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            new dangnhap().ShowDialog();
            this.Close();
        }

        private void txtmatkhauqmk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtmatkhauqmk.Enabled && !string.IsNullOrEmpty(txtmatkhauqmk.Text))
            {
                if (txtmatkhauqmk.Tag == null) return;

                string sql = "UPDATE " + txtmatkhauqmk.Tag.ToString() +
                             " SET MatKhau = N'" + txtmatkhauqmk.Text + "' WHERE " +
                             "TenDangNhap = N'" + txttendangnhapqmk.Text + "'";
                try
                {
                    qltt.ExecuteNonQuery(sql);
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    new dangnhap().ShowDialog();
                    this.Close();
                }
                catch
                {

                }
            }
        }
        
    }
}
