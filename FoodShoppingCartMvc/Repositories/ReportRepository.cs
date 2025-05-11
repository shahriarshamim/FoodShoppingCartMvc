using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FoodShoppingCartMvc.Repositories
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ReportRepository: IReportRepository
    {
        private readonly ApplicationDbContext _context;
        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TopNSoldItemModel>> GetTopNSellingItemsByDate(DateTime startDate, DateTime endDate)
        {
            var startDateParam = new SqlParameter("@startDate", startDate);
            var endDateParam = new SqlParameter("@endDate", endDate);
            var topFiveSoldItems = await _context.Database.SqlQueryRaw<TopNSoldItemModel>("exec Usp_GetTopNSellingItemsByDate @startDate,@endDate", startDateParam, endDateParam).ToListAsync();
            return topFiveSoldItems;
        }
    }
    public interface IReportRepository
    {
        Task<IEnumerable<TopNSoldItemModel>> GetTopNSellingItemsByDate(DateTime startDate, DateTime endDate);
    }
}
