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
        public async Task<List<PhieuMuonSach>> LayPhieuMuon(string maDocGia)
        {
            return await context.PhieuMuonSach
                       .Where(p => p.MaDocGia == maDocGia)
                       .ToListAsync();
        }
        public async Task<List<PhieuMuonSach>> LayPhieuMuonTheoIdPm(string maPM)
        {
            return await context.PhieuMuonSach
                       .Where(p => p.MaPhieuMuon == maPM)
                       .ToListAsync();
        }
        public async Task<List<ChiTietMuonSach>> LayChiTietPhieuMuon(string maPM)
        {
            return await context.ChiTietMuonSach.Where(p => p.MaPhieuMuon == maPM && p.TinhTrangMuon == "Đang chờ duyệt")
                       .ToListAsync(); ;
        }
        public async Task<bool> XacNhanPhieuMuon(string maPhieuMuon, string maNhanVien)
        {
            
            var phieu = await context.PhieuMuonSach
                .FirstOrDefaultAsync(p => p.MaPhieuMuon == maPhieuMuon);

            if (phieu == null)
                return false;

           
            phieu.MaNhanVien = maNhanVien;

            
            var chiTietList = await context.ChiTietMuonSach
                .Where(ct => ct.MaPhieuMuon == maPhieuMuon && ct.TinhTrangMuon == "Đang chờ duyệt")
                .ToListAsync();

            foreach (var ct in chiTietList)
            {
                ct.TinhTrangMuon = "Đã duyệt";
                ct.NgayPhaiTra = DateTime.Now.AddDays(7); 
            }

            await context.SaveChangesAsync();
            return true;
        }
    }
}
