using Microsoft.EntityFrameworkCore;
using QLThuVienMVC.Controllers;
using QLThuVienMVC.Models.Detail_Lib;

namespace QLThuVienMVC.Models
{
    public class SachRepository : InterfaceSach
    {
        private readonly LibDataContext context;
        
        public SachRepository(LibDataContext _context)
        {
            this.context = _context;
        }
       
        public IQueryable<Sach> LaySach()
        {
            return context.Sach;
        }
        public async Task<List<Sach>> LayDanhSachTheoIdAsync(List<string> ids) {
            return await context.Sach
            .Where(s => ids.Contains(s.MaSach)).ToListAsync();
        }
        public async Task ThemPhieuMuon(string idDocGia,List<string> dsidSach)
        {
            var lastCode = await context.PhieuMuonSach
             .OrderByDescending(pm => pm.MaPhieuMuon)
             .Select(pm => pm.MaPhieuMuon) 
             .FirstOrDefaultAsync();

            var maPM = GenerateCode.GenerateNextCode(lastCode, "PM");

            var PhieuMuom = new PhieuMuonSach
            {
                MaPhieuMuon = maPM,
                MaDocGia = idDocGia,
                MaNhanVien = null,
                NgayMuon = DateTime.Now,
            };
            context.PhieuMuonSach.Add(PhieuMuom);
            foreach (var masach in dsidSach) {
                var ChiTietPm = new ChiTietMuonSach
                {
                    MaPhieuMuon = maPM,
                    MaSach = masach,
                    NgayPhaiTra = null,
                    TinhTrangMuon = "Đang chờ duyệt"
                };
                context.ChiTietMuonSach.Add(ChiTietPm);
            }
            await context.SaveChangesAsync();
        }
    }
}
