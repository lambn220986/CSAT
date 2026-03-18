using CSAT.Models;
using System.Threading.Tasks;

namespace CSAT.Data
{
    public class CustomerSatisfactionRepository : BaseRepository
    {
        public CustomerSatisfactionRepository(DbConnectionFactory factory)
            : base(factory)
        {
        }
        public async Task<int> Insert(CustomerSatisfaction entity)
        {
            string sql = @"
                INSERT INTO TT_CUSTOMER_SATISFACTION
                (
                    VoteValue,
                    UserId,
                    DepartmentId,
                    DeviceName,
                    IPAddress,
                    Note
                )
                VALUES
                (
                    @VoteValue,
                    @UserId,
                    @DepartmentId,
                    @DeviceName,
                    @IPAddress,
                    @Note
                );";
            return await ExecuteAsync(sql, entity);
        }
    }
}
