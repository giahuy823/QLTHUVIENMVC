using QLThuVienMVC.Models.Detail_Lib;
namespace QLThuVienMVC.Models
{
    public interface InterfaceSach
    {
        public IQueryable<Sach> LaySach();
        public Task<List<Sach>> LayDanhSachTheoIdAsync(List<string> id);

        public Task ThemPhieuMuon(string idDocGia, List<string> dsid);
        public Task<List<PhieuMuonSach>> LayPhieuMuon(string maDocGia);
        public Task<List<PhieuMuonSach>> LayPhieuMuonTheoIdPm(string maPM);
        public Task<List<ChiTietMuonSach>> LayChiTietPhieuMuon(string maPm);
        public Task<bool> XacNhanPhieuMuon(string maPhieuMuon, string maNhanVien);
    }
}