using System;
using System.Data;

namespace QLTV.BusinessLogic
{
    public class ThongKeService
    {
        public class KetQuaThongKe
        {
            public string MaKetQua { get; set; } = "OK";
            public string ThongBaoNguoiDung { get; set; } = "";
            public string SqlQuery { get; set; } = "";
            public string LoaiThongKe { get; set; } = "";
            public int TongDuKien { get; set; } = 0;
        }
        public KetQuaThongKe XuLyThongKe(bool checkYeuThich, bool checkDangMuon, bool checkDaTra)
        {
            if (!checkYeuThich && !checkDangMuon && !checkDaTra)
            {
                return new KetQuaThongKe
                {
                    MaKetQua = "KHONG_CHON_LOAI",
                    ThongBaoNguoiDung = "Phải chọn ít nhất 1 loại thống kê!"
                };
            }

            string sql = "";
            string loai = "";

            if (checkYeuThich)
            {
                sql = @"
                    SELECT s.MaSach AS [MÃ SÁCH], s.TieuDe AS [TIÊU ĐỀ], 
                           COUNT(mt.MaSach) AS [SỐ LẦN MƯỢN]
                    FROM MuonTra mt INNER JOIN Sach s ON mt.MaSach = s.MaSach
                    GROUP BY s.MaSach, s.TieuDe HAVING COUNT(mt.MaSach) >= 3  
                    ORDER BY [SỐ LẦN MƯỢN] DESC";
                loai = "YeuThich";
            }
            else if (checkDangMuon)
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], 
                               MaSach AS [MÃ SÁCH], NgayMuon AS [NGÀY MƯỢN], 
                               NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], GhiChu AS [GHI CHÚ] 
                        FROM MuonTra WHERE TrangThai = N'Đang Mượn'";
                loai = "DangMuon";
            }
            else if (checkDaTra) 
            {
                sql = @"SELECT MaMuonTra AS [MÃ MƯỢN TRẢ], MaDocGia AS [MÃ ĐỘC GIẢ], 
                               MaSach AS [MÃ SÁCH], NgayMuon AS [NGÀY MƯỢN], 
                               NgayTra AS [NGÀY TRẢ], TrangThai AS [TRẠNG THÁI], GhiChu AS [GHI CHÚ] 
                        FROM MuonTra WHERE TrangThai = N'Đã Trả'";
                loai = "DaTra";
            }

            return new KetQuaThongKe
            {
                MaKetQua = "OK",
                SqlQuery = sql,
                LoaiThongKe = loai,
                TongDuKien = 0
            };
        }
    }
}
