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
                MessageBox.Show("Chưa có mã thủ thư.", "Lỗi");
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
                    new SqlParameter("@NgaySinh", DateTime.ParseExact(txtngaysinh2.Text, "dd/MM/yyyy", null)),
                    new SqlParameter("@GioiTinh", txtgioitinh2.Text.Trim()),
                    new SqlParameter("@MaThuThu", txtmathuthu2.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                TaiThongTinCaNhanThuThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật, kiểm tra lại định dạng ngày sinh (dd/MM/yyyy). \nChi tiết: " + ex.Message, "Lỗi");
            }
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
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