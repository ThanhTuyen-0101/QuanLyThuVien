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
    public partial class docgia : Form
    {
        private bool isReturning = false;

        public docgia()
        {
            InitializeComponent();
            timkiem.KeyDown += timkiem_KeyDown;
            this.FormClosed += docgia_FormClosed;
        }

        private void docgia_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void taidocgia(string madg = "")
        {
            string sql;
            DataTable dt;
            if (string.IsNullOrEmpty(madg))
            {
                sql = @"SELECT MaDocGia AS [MÃ ĐỘC GIẢ], HoTen AS [HỌ TÊN], NgaySinh AS [NGÀY SINH], 
                               GioiTinh AS [GIỚI TÍNH], DiaChi AS [ĐỊA CHỈ], SoDienThoai AS [SỐ ĐIỆN THOẠI], 
                               Email AS [EMAIL] FROM DocGia";
                dt = qltt.ExecuteQuery(sql);
            }
            else
            {
                sql = @"SELECT MaDocGia AS [MÃ ĐỘC GIẢ], HoTen AS [HỌ TÊN], NgaySinh AS [NGÀY SINH], 
                               GioiTinh AS [GIỚI TÍNH], DiaChi AS [ĐỊA CHỈ], SoDienThoai AS [SỐ ĐIỆN THOẠI], 
                               Email AS [EMAIL] FROM DocGia WHERE MaDocGia LIKE @Search OR HoTen LIKE @Search";
                SqlParameter[] param = { new SqlParameter("@Search", "%" + madg.Trim() + "%") };
                dt = qltt.ExecuteQuery(sql, param);
            }

            GridView.DataSource = dt;
            GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
                INSERT INTO DocGia
                (MaDocGia, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email)
                VALUES (@MaDocGia, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaDocGia", txtmadg.Text.Trim()),
                    new SqlParameter("@HoTen", txttendg.Text.Trim()),
                    new SqlParameter("@NgaySinh", txtngaysinh.Text.Trim()),
                    new SqlParameter("@GioiTinh", txtgioitinh.Text.Trim()),
                    new SqlParameter("@DiaChi", txtdiachi.Text.Trim()),
                    new SqlParameter("@SoDienThoai", txtsdt.Text.Trim()),
                    new SqlParameter("@Email", txtemail.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại!\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                taidocgia();
            }
        }

        private void capnhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtmadg.Text))
            {
                MessageBox.Show("Bạn chưa chọn độc giả để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"UPDATE DocGia
                           SET HoTen = @HoTen,
                               NgaySinh = @NgaySinh,
                               GioiTinh = @GioiTinh,
                               DiaChi = @DiaChi,
                               SoDienThoai = @SoDienThoai,
                               Email = @Email
                           WHERE MaDocGia = @MaDocGia";

            SqlParameter[] parameters =
            {
                new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = txttendg.Text.Trim() },
                new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = txtngaysinh.Text.Trim() },
                new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = txtgioitinh.Text.Trim() },
                new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = txtdiachi.Text.Trim() },
                new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = txtsdt.Text.Trim() },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = txtemail.Text.Trim() },
                new SqlParameter("@MaDocGia", SqlDbType.VarChar) { Value = txtmadg.Text.Trim() }
            };

            try
            {
                int result = qltt.ExecuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    taidocgia();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có muốn xóa không? (Sẽ xóa luôn lịch sử mượn sách!)",
                "Chú ý",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(txtmadg.Text))
                {
                    try
                    {
                        string deleteMuonTra = "DELETE FROM MuonTra WHERE MaDocGia = @MaDocGia";
                        SqlParameter[] param1 = { new SqlParameter("@MaDocGia", txtmadg.Text.Trim()) };
                        qltt.ExecuteNonQuery(deleteMuonTra, param1);

                        string sql = "DELETE FROM DocGia WHERE MaDocGia = @MaDocGia";
                        SqlParameter[] param2 = { new SqlParameter("@MaDocGia", txtmadg.Text.Trim()) };
                        qltt.ExecuteNonQuery(sql, param2);

                        MessageBox.Show("Xóa Thành Công");
                        taidocgia();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xóa Thất Bại: " + ex.Message, "Lỗi");
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn...", "Thông báo");
                }
            }
        }

        private void timkiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                taidocgia(timkiem.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            isReturning = true;
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

        private void GridView_SelectionChanged(object sender, EventArgs e)
        {
            if (GridView.SelectedRows.Count > 0)
            {
                this.txtmadg.Text = this.GridView.SelectedRows[0].Cells["MÃ ĐỘC GIẢ"].Value.ToString();
                this.txttendg.Text = this.GridView.SelectedRows[0].Cells["HỌ TÊN"].Value.ToString();
                this.txtngaysinh.Text = this.GridView.SelectedRows[0].Cells["NGÀY SINH"].Value.ToString();
                this.txtgioitinh.Text = this.GridView.SelectedRows[0].Cells["GIỚI TÍNH"].Value.ToString();
                this.txtdiachi.Text = this.GridView.SelectedRows[0].Cells["ĐỊA CHỈ"].Value.ToString();
                this.txtsdt.Text = this.GridView.SelectedRows[0].Cells["SỐ ĐIỆN THOẠI"].Value.ToString();
                this.txtemail.Text = this.GridView.SelectedRows[0].Cells["EMAIL"].Value.ToString();
            }
        }

        private void docgia_Load(object sender, EventArgs e)
        {
            GridView.Font = new Font("Times New Roman", 10);
            taidocgia();
        }
    }
}