using System.Collections.Generic;
using System.Linq;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IEmployeeRepository
    {
        EmployeeEntity GetSingle(int id);
        void Add(EmployeeEntity item);
        void Delete(int id);
        EmployeeEntity Update(int id, EmployeeEntity item);
        IQueryable<EmployeeEntity> GetAll(QueryParameters queryParameters);
        IEnumerable<EmployeeEntity> GetAll();
    IEnumerable<EmployeeEntity> GetAllBySearchString(string searchString);

    ICollection<EmployeeEntity> GetRandomMeal();
        int Count();

        bool Save();
    }
}
