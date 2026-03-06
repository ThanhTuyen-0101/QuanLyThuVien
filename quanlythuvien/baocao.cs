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
    public partial class baocao : Form
    {
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

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            string sql = "";
            DataTable dt;

            if (checkyeuthich.Checked)
            {
                sql = @"
            SELECT 
                s.MaSach AS [MÃ SÁCH],
                s.TieuDe AS [TIÊU ĐỀ],
                COUNT(mt.MaSach) AS [SỐ LẦN MƯỢN]
            FROM MuonTra mt
            INNER JOIN Sach s ON mt.MaSach = s.MaSach
            GROUP BY s.MaSach, s.TieuDe
            HAVING COUNT(mt.MaSach) >= 3  
            ORDER BY [SỐ LẦN MƯỢN] DESC";
            }
            else if (checkdangmuon.Checked)
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], MaSach AS [MÃ SÁCH], 
                               NgayMuon AS [NGÀY MƯỢN], NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], 
                               GhiChu AS [GHI CHÚ] FROM MuonTra WHERE TrangThai = N'Đang Mượn'";
            }
            else
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], MaSach AS [MÃ SÁCH], 
                               NgayMuon AS [NGÀY MƯỢN], NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], 
                               GhiChu AS [GHI CHÚ] FROM MuonTra WHERE TrangThai = N'Đã Trả'";
            }

            dt = qltt.ExecuteQuery(sql);
            GridBaoCao.DataSource = dt;
            GridBaoCao.Font = new Font("Times New Roman", 10);
            GridBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            lbTong.Text = $"Tổng số sách: {dt.Rows.Count}";

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            this.Hide();
            new thongtinthuthu().ShowDialog();
            this.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            isReturning = true;
            this.Close();
        }
    }
}