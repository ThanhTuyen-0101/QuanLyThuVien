using NUnit.Framework.Internal;
using QLTV.BusinessLogic;

namespace TestProjectQLTV_Debug
{
    [TestFixture]
    public class ThongKeServiceTests
    {
        private ThongKeService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ThongKeService();
        }

        [Test]
        public void XuLyThongKe_KhongChonLoai_TraVeLoi()
        {
            var result = _service.XuLyThongKe(false, false, false);
            Assert.That(result.MaKetQua, Is.EqualTo("KHONG_CHON_LOAI"));
            Assert.That(result.ThongBaoNguoiDung, Is.EqualTo("Phải chọn ít nhất 1 loại thống kê!"));
        }

        [Test]
        public void XuLyThongKe_YeuThichTrue_TraVeSqlYeuThich()
        {
            var result = _service.XuLyThongKe(true, false, false);
            Assert.That(result.MaKetQua, Is.EqualTo("OK"));
            Assert.That(result.LoaiThongKe, Is.EqualTo("YeuThich"));
            Assert.That(result.SqlQuery, Does.Contain("HAVING COUNT(mt.MaSach) >= 3"));
        }

        [Test]
        public void XuLyThongKe_DangMuonTrue_TraVeSqlDangMuon()
        {
            var result = _service.XuLyThongKe(false, true, false);
            Assert.That(result.MaKetQua, Is.EqualTo("OK"));
            Assert.That(result.LoaiThongKe, Is.EqualTo("DangMuon"));
            Assert.That(result.SqlQuery, Does.Contain("WHERE TrangThai = N'Đang Mượn'"));
        }

        [Test]
        public void XuLyThongKe_DaTraTrue_TraVeSqlDaTra()
        {
            var result = _service.XuLyThongKe(false, false, true);
            Assert.That(result.MaKetQua, Is.EqualTo("OK"));
            Assert.That(result.LoaiThongKe, Is.EqualTo("DaTra"));
            Assert.That(result.SqlQuery, Does.Contain("WHERE TrangThai = N'Đã Trả'"));
        }

        [Test]
        public void XuLyThongKe_MultiCheckbox_PriorityYeuThich()
        {
            var result = _service.XuLyThongKe(true, true, true);
            Assert.That(result.LoaiThongKe, Is.EqualTo("YeuThich")); 
        }
    }
}
