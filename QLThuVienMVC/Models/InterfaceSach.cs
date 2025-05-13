using QLThuVienMVC.Models.Detail_Lib;
namespace QLThuVienMVC.Models
{
    public interface InterfaceSach
    {
        public IQueryable<Sach> LaySach();
        public Task<List<Sach>> LayDanhSachTheoIdAsync(List<string> id);

        public Task ThemPhieuMuon(string idDocGia, List<string> dsid);
        

    }
}