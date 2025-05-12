using Microsoft.AspNetCore.Mvc;

namespace QLThuVienMVC.Controllers
{
    public class PhieuMuonSachController : Controller
    {
        public IActionResult PhieuMuonSach()
        {
            return View("PhieuMuonSach");
        }
    }
}
