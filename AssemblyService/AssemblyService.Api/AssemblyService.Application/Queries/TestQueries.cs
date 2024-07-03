using WarehouseService.DTOs;
using WarehouseService.Repositories;

namespace WarehouseService.Queries
{
    public class TestQueries: ITestQueries
    {
        private readonly ITestQueriesRepository _repository;
        public TestQueries(ITestQueriesRepository repository)
        {
            _repository = repository;
        }
        public TestDTO FindById(int testId)
        {
            var emp = _repository.GetById(testId);
            return new TestDTO
            {
                Id = emp.Id,
                Name = emp.Name,
            };
        }
    }
}
