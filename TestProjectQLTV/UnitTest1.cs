using NUnit.Framework;
using quanlythuvien; 

namespace TestProjectQLTV
{
    public class MuonTraServiceTests
    {
        private MuonTraService _service;

        [SetUp]
        public void Setup()
        {
            _service = new MuonTraService();
        }

        [Test]
        public void XuLySua_KhongChonDong_TraVeChuaChonDong()
        {
            var result = _service.XuLySua(
                selectedRowCount: 0,
                txtDateMuon: "2025-01-01",
                txtDateTra: "",
                checkTra: false,
                trangThaiCu: "Đang Mượn"
            );

            Assert.AreEqual("CHUA_CHON_DONG", result.MaKetQua);
        }
    }
}
