using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Configuration;

namespace UITest_Automation
{
    [TestClass]
    public class LibraryAutomationTests
    {
        private readonly string appPath =
    System.IO.Path.GetFullPath(
        ConfigurationManager.AppSettings["AppPath"]);

        private Application app;
        private UIA3Automation automation;

        [TestInitialize]
        public void Setup()
        {
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
        }

        [TestCleanup]
        public void Cleanup()
        {
            automation?.Dispose();
            app?.Close();
        }

        private void DangNhap(Application app, UIA3Automation automation)
        {
            var window = app.GetMainWindow(automation);
            window.FindFirstDescendant(cf => cf.ByAutomationId("txttendangnhap"))
                .AsTextBox().Text = "Tuyen";

            window.FindFirstDescendant(cf => cf.ByAutomationId("txtmatkhaudangnhap"))
                .AsTextBox().Text = "1234@Ntt";

            window.FindFirstDescendant(cf => cf.ByAutomationId("btndagnhap"))
                .AsButton().Click();

            Thread.Sleep(2000);
            Keyboard.Press(VirtualKeyShort.ENTER);
            Thread.Sleep(2000);
        }


        [TestMethod]
        public void KichBan1_DangNhap_ThanhCong()
        {
            var window = app.GetMainWindow(automation);
            Assert.IsNotNull(window, "Không mở được app");

            DangNhap(app, automation);

            var mainWindow = app.GetMainWindow(automation);
            Assert.IsTrue(mainWindow?.Title != window.Title, "Đăng nhập thất bại");

        }

        [TestMethod]
        public void KichBan2_ThemSachMoi_ThanhCong()
        {

            DangNhap(app, automation);

            var trangChu = app.GetMainWindow(automation);
            trangChu.FindFirstDescendant(cf => cf.ByAutomationId("btbnsach"))
                .AsButton().Click();
            Thread.Sleep(2000);

            var formSach = app.GetMainWindow(automation);
            Assert.IsNotNull(formSach, "Không mở Form Sách");

            Random rnd = new Random();

            // Tạo tên sách: "Sách Auto Test HHmmss_random"
            int soNgauNhien = rnd.Next(1000, 9999);
            string tenSachMoi = $"Sách Auto Test {DateTime.Now:HHmmss}_{soNgauNhien}";

            // Tạo ISBN: "978-604-2-xxxxx-y"
            int soGiua = rnd.Next(10000, 99999); // 5 chữ số
            int soCuoi = rnd.Next(0, 9);         // 1 chữ số
            string isbn = $"978-604-2-{soGiua}-{soCuoi}";


            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txttensach"))
                .AsTextBox().Text = tenSachMoi;
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txttacgia"))
                .AsTextBox().Text = "1";
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtnxb"))
                .AsTextBox().Text = "1";
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtnam"))
                .AsTextBox().Text = "2024";
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtmatheloai"))
                .AsTextBox().Text = "1";
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtisbn"))
                .AsTextBox().Text = isbn;
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtsoluong"))
                .AsTextBox().Text = "10";
            formSach.FindFirstDescendant(cf => cf.ByAutomationId("txtconlai"))
                .AsTextBox().Text = "10";

            formSach.FindFirstDescendant(cf => cf.ByAutomationId("btnthemqls"))
                .AsButton().Click();
            Thread.Sleep(2000);
            Keyboard.Press(VirtualKeyShort.ENTER);
            Thread.Sleep(3000);

            var txtTimKiem = formSach.FindFirstDescendant(cf => cf.ByAutomationId("txttimkiemqls"))
                .AsTextBox();
            txtTimKiem.Text = tenSachMoi;
            Keyboard.Press(VirtualKeyShort.ENTER);
            Thread.Sleep(5000);

            string tenSachUI = formSach.FindFirstDescendant(cf => cf.ByAutomationId("txttensach"))
                    .AsTextBox().Text;
            Assert.AreEqual(tenSachMoi, tenSachUI);

        }

        [TestMethod]
        public void KichBan3_TinhPhiPhat_MuonTraSach()
        {
             DangNhap(app, automation);


                var trangChu = app.GetMainWindow(automation);
                trangChu.FindFirstDescendant(cf => cf.ByAutomationId("btnmuontra"))
                    .AsButton().Click();
                Thread.Sleep(2000);

                var formMuonTra = app.GetMainWindow(automation);
                Assert.IsNotNull(formMuonTra, "Không mở Form Mượn Trả");

                EnterText(formMuonTra, "txtDateMuon", "05/03/2026");
                EnterText(formMuonTra, "txtDateTra", "05/11/2026");

            // Check "Đã Trả"
            var chkTra = formMuonTra.FindFirstDescendant(cf => cf.ByAutomationId("Checktra"));
                if (chkTra != null) chkTra.AsCheckBox().Click();
                Thread.Sleep(2000);

                // 4. Assert
                var txtTienPhat = formMuonTra.FindFirstDescendant(cf => cf.ByAutomationId("txtTienPhat"));
                string tienPhat = txtTienPhat.AsTextBox().Text.Trim();
                Assert.IsTrue(double.TryParse(tienPhat, out double phi) && phi > 0, $"Không có phí phạt");
        }


        private void EnterText(AutomationElement parent, string automationId, string text)
        {
            var txtBox = parent.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
            if (txtBox != null)
            {
                txtBox.AsTextBox().Text = text; 
            }
        }


    }
}
