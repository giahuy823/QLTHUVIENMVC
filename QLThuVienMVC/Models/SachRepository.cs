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
    }
}
