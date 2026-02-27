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
    public partial class GiaHanSachDG : Form
    {
        private string maDocGia;

        public GiaHanSachDG(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
        }

        private void LoadThongTin(string maDocGia)
        {
            string sql = @"
        SELECT mt.MaMuonTra, mt.MaDocGia, mt.MaSach, s.TieuDe, mt.NgayMuon, mt.NgayTra, mt.TrangThai, mt.GhiChu
        FROM MuonTra mt
        INNER JOIN Sach s ON mt.MaSach = s.MaSach
        WHERE mt.MaDocGia = N'" + maDocGia.Replace("'", "''") + "'";

            DataTable dt = qltt.ExecuteQuery(sql);
            dvgGiaHanDG.DataSource = dt;
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (txtDieuKien.Text != "Đủ điều kiện")
            {
                MessageBox.Show("Không đủ điều kiện gia hạn!");
                return;
            }
            string maMuonTra = dvgGiaHanDG.SelectedRows[0].Cells["MaMuonTra"].Value.ToString();
            string sqlUpdate = "UPDATE MuonTra SET NgayTra = NULL, TrangThai = N'Đang Mượn', GhiChu = ISNULL(GhiChu, '') + N' | Gia hạn' WHERE MaMuonTra = @MaMuonTra";
            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@MaMuonTra", maMuonTra)
            };
            qltt.ExecuteNonQuery(sqlUpdate, parameters);
            MessageBox.Show("Gia hạn thành công!");
            LoadThongTin(this.maDocGia);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dvgGiaHanDG_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dvgGiaHanDG.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dvgGiaHanDG.SelectedRows[0];

                txtMaDocGia.Text = row.Cells["MaDocGia"].Value?.ToString();
                txtMaSach.Text = row.Cells["MaSach"].Value?.ToString();

                DateTime? ngayMuon = row.Cells["NgayMuon"].Value == null || row.Cells["NgayMuon"].Value == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(row.Cells["NgayMuon"].Value);

                DateTime? ngayTra = row.Cells["NgayTra"].Value == null || row.Cells["NgayTra"].Value == DBNull.Value || row.Cells["NgayTra"].Value.ToString() == ""
                    ? (DateTime?)null
                    : Convert.ToDateTime(row.Cells["NgayTra"].Value);

                string trangThai = row.Cells["TrangThai"].Value?.ToString();

                txtNgayMuon.Text = ngayMuon.HasValue ? ngayMuon.Value.ToString("dd/MM/yyyy") : "";
                txtNgayTra.Text = ngayTra.HasValue ? ngayTra.Value.ToString("dd/MM/yyyy") : "";
                txtNgayGiaHan.Text = "";
                string dieuKien = "";

                if (trangThai == "Đang Mượn")
                {
                    dieuKien = "Không đủ điều kiện";
                }
                else if (ngayMuon.HasValue && ngayTra.HasValue)
                {
                    int soNgay = (ngayTra.Value - ngayMuon.Value).Days;
                    if (soNgay < 14)
                        dieuKien = "Đủ điều kiện";
                    else
                        dieuKien = "Không đủ điều kiện";
                }
                else
                {
                    dieuKien = "Không đủ điều kiện";
                }

                txtDieuKien.Text = dieuKien;
            }
        }

        private void GiaHanSachDG_Load(object sender, EventArgs e)
        {
            LoadThongTin(this.maDocGia);
        }
    }
}