using NUnit.Framework;
using NUnit.Framework.Internal;
using QLTV.BusinessLogic;
namespace TestProjectQLTV_Debug
{
    [TestFixture]
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
            Assert.That(result.MaKetQua, Is.EqualTo("CHUA_CHON_DONG"));
        }
    }
}
