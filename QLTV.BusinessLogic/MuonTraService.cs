using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV.BusinessLogic
{
    public class MuonTraService
    {
        public class KetQuaSua
        {
            public string MaKetQua { get; set; }
            public string ThongBaoNguoiDung { get; set; }
            public DateTime NgayMuon { get; set; }
            public DateTime? NgayTra { get; set; }
            public string TrangThaiMoi { get; set; }
            public bool TangKho { get; set; }
            public bool GiamKho { get; set; }
        }

        public KetQuaSua XuLySua(int selectedRowCount, string txtDateMuon, string txtDateTra, bool checkTra, string trangThaiCu)
        {
            if (selectedRowCount == 0)
            {
                return new KetQuaSua
                {
                    MaKetQua = "CHUA_CHON_DONG",
                    ThongBaoNguoiDung = "Chọn dòng cần sửa!"
                };
            }

            DateTime ngayMuon = DateTime.Parse(txtDateMuon.Trim());
            DateTime? ngayTra = null;

            if (!string.IsNullOrEmpty(txtDateTra))
            {
                DateTime ngayTraTmp = DateTime.Parse(txtDateTra.Trim());
                if (checkTra && ngayTraTmp < ngayMuon)
                {
                    return new KetQuaSua
                    {
                        MaKetQua = "NGAY_TRA_NHO_HON_NGAY_MUON",
                        ThongBaoNguoiDung = "Ngày trả không được nhỏ hơn ngày mượn!"
                    };
                }
                ngayTra = ngayTraTmp;
            }

            string trangThaiMoi = checkTra ? "Đã Trả" : "Đang Mượn";
            if (checkTra && ngayTra == null)
            {
                ngayTra = DateTime.Now.Date;
            }

            bool tangKho = false;
            bool giamKho = false;

            if (trangThaiCu == "Đang Mượn" && trangThaiMoi == "Đã Trả")
            {
                tangKho = true;
            }
            else if (trangThaiCu == "Đã Trả" && trangThaiMoi == "Đang Mượn")
            {
                giamKho = true;
            }

            return new KetQuaSua
            {
                MaKetQua = "OK",
                ThongBaoNguoiDung = "Cập nhật thành công!",
                NgayMuon = ngayMuon,
                NgayTra = ngayTra,
                TrangThaiMoi = trangThaiMoi,
                TangKho = tangKho,
                GiamKho = giamKho
            };
        }
    }
}
