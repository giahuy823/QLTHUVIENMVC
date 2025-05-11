using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuVienMVC.Models;
using QLThuVienMVC.ViewModels;


namespace QLThuVienMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private InterfaceSach SachRepo;
        public int pageSize = 8;
        public HomeController(ILogger<HomeController> logger, InterfaceSach _sachRepo)
        {
            _logger = logger;
            SachRepo = _sachRepo;
        }
        [HttpGet]
        public IActionResult Index(string? tag, int page = 1)
        {
            var Sach = tag == null ? SachRepo.LaySach().ToList() : SachRepo.LaySach()
                 .Where(c => c.TheLoai == tag).ToList();
            int totalItems = tag == null ? SachRepo.LaySach().Count() : SachRepo.LaySach()
                 .Where(c => c.TheLoai == tag).Count();
            var sachPage = Sach
                .Where(p => tag == null || p.TheLoai == tag)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();   
            var vm = new SachViewModel
            {
                Sachs = sachPage,
                Paging = new Paging
                {
                    ItemPerPage = pageSize,
                    TotalItems = totalItems,
                    CurrentPage = page
                },
                currentTag = tag
            };

            return View(vm);
        }


        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
