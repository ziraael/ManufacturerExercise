using WarehouseService.Models;

namespace WarehouseService.Repositories
{
    public interface ITestQueriesRepository
    {
        Test GetById(int testID);
    }
}
