using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using QLThuVienMVC.Models;
using QLThuVienMVC.Models.UserModel;
using QLThuVienMVC.ViewModels;

namespace QLThuVienMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LibDataContext _Context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,LibDataContext libDataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Context = libDataContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email and arent incorrect");
                    return View(model);
                }
               
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                if (model.Role == "NhanVien")
                {

                    var lastNhanVien = await _Context.NhanVien
                    .OrderByDescending(nv => nv.MaNhanVien)
                    .FirstOrDefaultAsync();

                    string newMaNhanVien = GenerateCode.GenerateNextCode(lastNhanVien?.MaNhanVien, "NV");

                    var NhanVien = new NhanVien()
                    {
                        MaNhanVien = newMaNhanVien,
                        HoTen = model.Name,
                        DiaChi = null,
                        SoDienThoai = null,
                        BangCap = "Thạc sĩ",
                        BoPhan = "Phòng nhân viên",
                        ChucVu = "Nhân viên",

                    };

                    _Context.NhanVien.Add(NhanVien);
                    await _Context.SaveChangesAsync();
                    return await CreateUserAndRole(model, "NhanVien", newMaNhanVien);
                }
                if (model.Role == "DocGia")
                {

                    var lastDocGia = await _Context.DocGia
                    .OrderByDescending(nv => nv.MaDocGia)
                    .FirstOrDefaultAsync();

                    string newDocGia = GenerateCode.GenerateNextCode(lastDocGia?.MaNhanVien, "DG");

                    var DocGia = new DocGia()
                    {
                        MaDocGia = newDocGia,
                        MaNhanVien = "NV001",
                        HoTen = model.Name,
                        Email = model.Email,
                        NgayLapThe = DateTime.Now

                    };

                    _Context.DocGia.Add(DocGia);
                    await _Context.SaveChangesAsync();
                    return await CreateUserAndRole(model, "DocGia",null ,newDocGia);

                }
                if (model.Role == "Admin")
                {

                    var lastNhanVien = await _Context.NhanVien
                    .OrderByDescending(nv => nv.MaNhanVien)
                    .FirstOrDefaultAsync();

                    string newMaNhanVien = GenerateCode.GenerateNextCode(lastNhanVien?.MaNhanVien, "NV");

                    var NhanVien = new NhanVien()
                    {
                        MaNhanVien = newMaNhanVien,
                        HoTen = model.Name,
                        DiaChi = null,
                        SoDienThoai = null,
                        BangCap = "Thạc sĩ",
                        BoPhan = "Phòng nhân viên",
                        ChucVu = "Quản trị viên",

                    };

                    _Context.NhanVien.Add(NhanVien);
                    await _Context.SaveChangesAsync();
                    return await CreateUserAndRole(model, "Admin", newMaNhanVien);
                }

            }
            return View(model);
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public IActionResult ChangePass()
        {
            return View();
        }
        private async Task<IActionResult> CreateUserAndRole(RegisterViewModel model, string role, string maNhanVien = null, string maDocGia = null)
        {
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                MaNhanVien = maNhanVien,
                MaDocGia = maDocGia
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
