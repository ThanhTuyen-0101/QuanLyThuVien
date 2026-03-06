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
    public partial class muontra : Form
    {
        private bool isReturning = false;

        public muontra()
        {
            InitializeComponent();
            ComboxMaDG();
            ComboxMaSach();
            this.FormClosed += muontra_FormClosed;
        }

        private void muontra_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        private void TaiDuLieu(string keyword = "")
        {
            string sql;
            DataTable dt;
            if (string.IsNullOrEmpty(keyword))
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], MaSach AS [MÃ SÁCH], 
                               NgayMuon AS [NGÀY MƯỢN], NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], 
                               GhiChu AS [GHI CHÚ] FROM MuonTra";
                dt = qltt.ExecuteQuery(sql);
            }
            else
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], MaSach AS [MÃ SÁCH], 
                               NgayMuon AS [NGÀY MƯỢN], NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], 
                               GhiChu AS [GHI CHÚ] FROM MuonTra WHERE MaDocGia LIKE @Keyword";
                SqlParameter[] param = { new SqlParameter("@Keyword", "%" + keyword + "%") };
                dt = qltt.ExecuteQuery(sql, param);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Font = new Font("Times New Roman", 10);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void XoaTrang()
        {
            txtDateMuon.Text = string.Empty;
            txtDateTra.Text = string.Empty;
            Checktra.Checked = false;
            txtGhiChu.Text = string.Empty;
            txtTienPhat.Text = "0";
        }

        private void ComboxMaDG()
        {
            try
            {
                string sql = "SELECT MaDocGia FROM DocGia";
                DataTable dt = qltt.ExecuteQuery(sql);
                cbMaDG.DataSource = dt;
                cbMaDG.ValueMember = "MaDocGia";
                cbMaDG.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboxMaSach()
        {
            try
            {
                string sql = "SELECT MaSach FROM Sach";
                DataTable dt = qltt.ExecuteQuery(sql);
                cbMaSach.DataSource = dt;
                cbMaSach.ValueMember = "MaSach";
                cbMaSach.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbMaDG.Text) || string.IsNullOrEmpty(cbMaSach.Text) || string.IsNullOrEmpty(txtDateMuon.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin!");
                    return;
                }

                // --- 1. KIỂM TRA LOGIC TỒN KHO ---
                string maSach = cbMaSach.Text.Trim();
                DataTable dtSach = qltt.ExecuteQuery("SELECT SoLuongConLai FROM Sach WHERE MaSach = " + maSach);
                if (dtSach.Rows.Count > 0)
                {
                    int soLuongCon = Convert.ToInt32(dtSach.Rows[0]["SoLuongConLai"]);
                    if (soLuongCon <= 0)
                    {
                        MessageBox.Show("Sách này đã hết trong kho, không thể cho mượn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Chặn không cho thêm
                    }
                }

                DateTime ngayMuon = DateTime.Parse(txtDateMuon.Text.Trim());
                object ngayTraObj = DBNull.Value;

                if (!string.IsNullOrEmpty(txtDateTra.Text))
                {
                    DateTime ngayTra = DateTime.Parse(txtDateTra.Text.Trim());
                    // --- 2. KIỂM TRA LOGIC NGÀY THÁNG ---
                    if (ngayTra < ngayMuon)
                    {
                        MessageBox.Show("Ngày trả không được nhỏ hơn ngày mượn!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ngayTraObj = ngayTra;
                }

                string trangThai = Checktra.Checked ? "Đã Trả" : "Đang Mượn";
                if (Checktra.Checked && ngayTraObj == DBNull.Value)
                {
                    ngayTraObj = DateTime.Now.Date;
                }

                string sql = @"INSERT INTO MuonTra (MaDocGia, MaSach, NgayMuon, NgayTra, TrangThai, GhiChu) 
                               VALUES (@MaDocGia, @MaSach, @NgayMuon, @NgayTra, @TrangThai, @GhiChu)";

                SqlParameter[] parameters = {
                    new SqlParameter("@MaDocGia", cbMaDG.Text.Trim()),
                    new SqlParameter("@MaSach", int.Parse(maSach)),
                    new SqlParameter("@NgayMuon", ngayMuon),
                    new SqlParameter("@NgayTra", ngayTraObj),
                    new SqlParameter("@TrangThai", trangThai),
                    new SqlParameter("@GhiChu", txtGhiChu.Text.Trim())
                };

                qltt.ExecuteNonQuery(sql, parameters);

                // --- 3. CẬP NHẬT TRỪ SỐ LƯỢNG KHO ---
                if (trangThai == "Đang Mượn")
                {
                    qltt.ExecuteNonQuery("UPDATE Sach SET SoLuongConLai = SoLuongConLai - 1 WHERE MaSach = " + maSach);
                }

                MessageBox.Show("Thêm thành công!");
                TaiDuLieu();
                XoaTrang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi định dạng ngày tháng hoặc dữ liệu: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn dòng cần sửa!");
                return;
            }

            try
            {
                int maMuonTra = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MÃ MƯỢN TRẢ"].Value);
                string trangThaiCu = dataGridView1.SelectedRows[0].Cells["TRẠNG THÁI"].Value.ToString();
                string maSach = cbMaSach.Text.Trim();

                DateTime ngayMuon = DateTime.Parse(txtDateMuon.Text.Trim());
                object ngayTraObj = DBNull.Value;

                if (!string.IsNullOrEmpty(txtDateTra.Text))
                {
                    DateTime ngayTra = DateTime.Parse(txtDateTra.Text.Trim());
                    // --- KIỂM TRA LOGIC NGÀY THÁNG ---
                    if (Checktra.Checked && ngayTra < ngayMuon)
                    {
                        MessageBox.Show("Ngày trả không được nhỏ hơn ngày mượn!", "Lỗi Logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ngayTraObj = ngayTra;
                }

                string trangThaiMoi = Checktra.Checked ? "Đã Trả" : "Đang Mượn";
                if (Checktra.Checked && ngayTraObj == DBNull.Value)
                {
                    ngayTraObj = DateTime.Now.Date;
                }

                string sql = @"UPDATE MuonTra SET 
                               MaDocGia = @MaDocGia, 
                               MaSach = @MaSach, 
                               NgayMuon = @NgayMuon, 
                               NgayTra = @NgayTra, 
                               TrangThai = @TrangThai, 
                               GhiChu = @GhiChu 
                               WHERE MaMuonTra = @MaMuonTra";

                SqlParameter[] parameters = {
                    new SqlParameter("@MaDocGia", cbMaDG.Text.Trim()),
                    new SqlParameter("@MaSach", int.Parse(maSach)),
                    new SqlParameter("@NgayMuon", ngayMuon),
                    new SqlParameter("@NgayTra", ngayTraObj),
                    new SqlParameter("@TrangThai", trangThaiMoi),
                    new SqlParameter("@GhiChu", txtGhiChu.Text.Trim()),
                    new SqlParameter("@MaMuonTra", maMuonTra)
                };

                qltt.ExecuteNonQuery(sql, parameters);

                // --- 4. CẬP NHẬT LẠI KHO KHI TRẢ SÁCH ---
                if (trangThaiCu == "Đang Mượn" && trangThaiMoi == "Đã Trả")
                {
                    // Người dùng mang trả -> Cộng lại 1 cuốn vào kho
                    qltt.ExecuteNonQuery("UPDATE Sach SET SoLuongConLai = SoLuongConLai + 1 WHERE MaSach = " + maSach);
                }
                else if (trangThaiCu == "Đã Trả" && trangThaiMoi == "Đang Mượn")
                {
                    // Thủ thư lỡ bấm nhầm Đã trả, giờ sửa lại thành Đang Mượn -> Trừ đi 1 cuốn
                    qltt.ExecuteNonQuery("UPDATE Sach SET SoLuongConLai = SoLuongConLai - 1 WHERE MaSach = " + maSach);
                }

                MessageBox.Show("Cập nhật thành công!");
                TaiDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát không?", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
            {
                isReturning = true;
                this.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn dòng cần xóa!");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                int maMuonTra = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MÃ MƯỢN TRẢ"].Value);
                string trangThai = dataGridView1.SelectedRows[0].Cells["TRẠNG THÁI"].Value.ToString();
                string maSach = dataGridView1.SelectedRows[0].Cells["MÃ SÁCH"].Value.ToString();

                string sql = "DELETE FROM MuonTra WHERE MaMuonTra = @MaMuonTra";
                SqlParameter[] param = { new SqlParameter("@MaMuonTra", maMuonTra) };
                qltt.ExecuteNonQuery(sql, param);

                // Nếu xóa phiếu đang mượn đi, phải trả lại sách cho kho
                if (trangThai == "Đang Mượn")
                {
                    qltt.ExecuteNonQuery("UPDATE Sach SET SoLuongConLai = SoLuongConLai + 1 WHERE MaSach = " + maSach);
                }

                MessageBox.Show("Xóa thành công!");
                TaiDuLieu();
                XoaTrang();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemMaDG.Text.Trim();
            TaiDuLieu(keyword);
        }

        private void btntrangchu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new trangchu().ShowDialog();
            this.Show();
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

        }

        private void btnbaocao_Click(object sender, EventArgs e)
        {
            this.Hide();
            new baocao().ShowDialog();
            this.Show();
        }

        private void btbntaikhoan_Click(object sender, EventArgs e)
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int maMuonTra = Convert.ToInt32(selectedRow.Cells["MÃ MƯỢN TRẢ"].Value);
                this.lbMaPhieu.Text = $"Mã Phiếu:{maMuonTra}";
                this.cbMaDG.Text = selectedRow.Cells["MÃ ĐỘC GIẢ"].Value.ToString();
                this.cbMaSach.Text = selectedRow.Cells["MÃ SÁCH"].Value.ToString();
                this.txtDateMuon.Text = selectedRow.Cells["NGÀY MƯỢN"].Value.ToString();
                this.txtDateTra.Text = selectedRow.Cells["NGÀY TRẢ"].Value.ToString();
                this.txtGhiChu.Text = selectedRow.Cells["GHI CHÚ"].Value.ToString();

                string trangThai = selectedRow.Cells["TRẠNG THÁI"].Value.ToString();
                Checktra.Checked = (trangThai == "Đã Trả");
            }
        }

        // TỰ ĐỘNG TÍNH PHÍ PHẠT KHI CHECK "ĐÃ TRẢ"
        private void Checktra_CheckedChanged_1(object sender, EventArgs e)
        {
            if (Checktra.Checked)
            {
                try
                {
                    DateTime ngayMuon = DateTime.Parse(txtDateMuon.Text);
                    DateTime ngayTra = DateTime.Now; // Mặc định là hôm nay

                    //  Nếu thủ thư đã điền tay ngày trả, phải lấy theo ngày đó
                    if (!string.IsNullOrEmpty(txtDateTra.Text))
                    {
                        ngayTra = DateTime.Parse(txtDateTra.Text);
                    }
                    else
                    {
                        // Nếu đang rỗng, tự động điền ngày hôm nay vào ô DateTra
                        txtDateTra.Text = ngayTra.ToString("dd/MM/yyyy");
                    }

                    // Nếu ngày trả < ngày mượn, không tính phạt và để hàm Thêm/Sửa báo lỗi
                    if (ngayTra < ngayMuon)
                    {
                        txtTienPhat.Text = "0";
                        return;
                    }

                    TienPhatCalculator calc = new TienPhatCalculator();
                    double tienPhat = calc.TinhTienPhat(ngayMuon, ngayTra);
                    txtTienPhat.Text = tienPhat.ToString("N0");
                }
                catch
                {
                    // Lỗi định dạng ngày, tạm thời bỏ qua không tính phạt
                    txtTienPhat.Text = "0";
                }
            }
            else
            {
                txtTienPhat.Text = "0";
            }
        }

        private void muontra_Load_1(object sender, EventArgs e)
        {
            TaiDuLieu();
            XoaTrang();
        }
    }
}