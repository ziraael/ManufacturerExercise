using WarehouseService.DTOs;

namespace WarehouseService.Queries
{
    public interface ITestQueries
    {
        TestDTO FindById(int testId);
    }
}
