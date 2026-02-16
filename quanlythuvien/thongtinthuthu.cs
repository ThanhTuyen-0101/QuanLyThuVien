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
    public partial class thongtinthuthu : Form
    {
        public thongtinthuthu()
        {
            InitializeComponent();
        }
        private void TaiThongTinCaNhanThuThu()
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaThuThu))
            {
                MessageBox.Show("Không có thông tin thủ thư để hiển thị. Vui lòng đăng nhập.", "Lỗi");
                this.Close();
                return;
            }
            try
            {
                string sql = "SELECT MaThuThu, HoTen, DiaChi, SoDienThoai, Email, NgaySinh, GioiTinh " +
                             "FROM ThuThu WHERE MaThuThu = N'" + thongtindangnhap.MaThuThu + "'";
                DataTable dt = qltt.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtmathuthu2.Text = dr["MaThuThu"].ToString();
                    txttenthuthu2.Text = dr["HoTen"].ToString();
                    txtdiachi2.Text = dr["DiaChi"].ToString();
                    txtsodienthoai2.Text = dr["SoDienThoai"].ToString();
                    txtemail2.Text = dr["Email"].ToString();
                    txtngaysinh2.Text = Convert.ToDateTime(dr["NgaySinh"]).ToString("dd/MM/yyyy");
                    txtgioitinh2.Text = dr["GioiTinh"].ToString();
                    txtmathuthu2.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin.", "Thông báo");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmathuthu2.Text))
            {
                MessageBox.Show("Chưa có mã thủ thư.", "Lỗi");
                return;
            }

            try
            {
                string sql = "UPDATE ThuThu SET " +
                             "HoTen = N'" + txttenthuthu2.Text + "', " +
                             "DiaChi = N'" + txtdiachi2.Text + "', " +
                             "SoDienThoai = '" + txtsodienthoai2.Text + "', " +
                             "Email = '" + txtemail2.Text + "', " +
                             "NgaySinh = '" + txtngaysinh2.Text + "', " +
                             "GioiTinh = N'" + txtgioitinh2.Text + "' " +
                             "WHERE MaThuThu = '" + txtmathuthu2.Text + "'";

                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                TaiThongTinCaNhanThuThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
            }
        }
        private void thongtinthuthu_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(thongtindangnhap.MaThuThu))
            {
                MessageBox.Show("Vui lòng đăng nhập lại.");
                this.Close();
                return;
            }
            TaiThongTinCaNhanThuThu();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmathuthu2.Text))
            {
                MessageBox.Show("Chưa có mã thủ thư.", "Lỗi");
                return;
            }

            try
            {
                string sql = "UPDATE ThuThu SET " +
                             "HoTen = N'" + txttenthuthu2.Text + "', " +
                             "DiaChi = N'" + txtdiachi2.Text + "', " +
                             "SoDienThoai = '" + txtsodienthoai2.Text + "', " +
                             "Email = '" + txtemail2.Text + "', " +
                             "NgaySinh = '" + txtngaysinh2.Text + "', " +
                             "GioiTinh = N'" + txtgioitinh2.Text + "' " +
                             "WHERE MaThuThu = '" + txtmathuthu2.Text + "'";

                qltt.ExecuteNonQuery(sql);
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                TaiThongTinCaNhanThuThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
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
            this.Hide();
            new dangnhap().ShowDialog();
            this.Close();
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
        
    }
}
