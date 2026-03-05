using QLTV.BusinessLogic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace quanlythuvien
{
    public partial class baocao : Form
    {
        private readonly ThongKeService thongKeService = new ThongKeService();
        private bool isReturning = false;

        public baocao()
        {
            InitializeComponent();
            checkdangmuon.Checked = true;
            this.FormClosed += baocao_FormClosed;
        }

        private void baocao_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isReturning)
            {
                Application.Exit();
            }
        }

        // ✅ FIX 3: TÊN METHOD ĐÚNG + 3 PARAM (Yêu thích/Đang mượn/Đã trả)
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            // Tên checkbox thật (thay checkdattr a nếu khác)
            var ketQua = thongKeService.XuLyThongKe(  // ✅ XuLyThongKe (không phải XyLy)
                checkyeuthich.Checked,      // 1️⃣ Yêu thích
                checkdangmuon.Checked,      // 2️⃣ Đang mượn
                checkdatra.Checked);        // 3️⃣ Đã trả ← THÊM PARAM NÀY

            if (ketQua.MaKetQua != "OK")
            {
                MessageBox.Show(ketQua.ThongBaoNguoiDung, "Lỗi thống kê",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable dt = qltt.ExecuteQuery(ketQua.SqlQuery);  // ✅ qltt OK

                GridBaoCao.DataSource = dt;
                GridBaoCao.Font = new Font("Times New Roman", 10);
                GridBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                int tong = dt.Rows.Count;
                lbTong.Text = $"Tổng số sách: {tong}";

                if (tong == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu.", "Thông báo",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}", "Lỗi DB",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Các button khác GIỮ NGUYÊN
        private void btntrangchu_Click(object sender, EventArgs e) { isReturning = true; this.Close(); }
        private void btndangxuat_Click(object sender, EventArgs e) { isReturning = true; Application.Restart(); }
        private void btnsach_Click(object sender, EventArgs e) { this.Hide(); new quanlysach().ShowDialog(); this.Show(); }
        private void btndocgia_Click(object sender, EventArgs e) { this.Hide(); new docgia().ShowDialog(); this.Show(); }
        private void btnmuontra_Click(object sender, EventArgs e) { this.Hide(); new muontra().ShowDialog(); this.Show(); }
        private void btnbaocao_Click(object sender, EventArgs e) { this.Hide(); new baocao().ShowDialog(); this.Show(); }
        private void btntaikhoan_Click(object sender, EventArgs e) { this.Hide(); new thongtinthuthu().ShowDialog(); this.Show(); }
        private void btnThoat_Click(object sender, EventArgs e) { isReturning = true; this.Close(); }
    }
}
