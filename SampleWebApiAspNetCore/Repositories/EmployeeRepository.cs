using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HolDbContext _holDbContext;

        public EmployeeRepository(HolDbContext holDbContext)
        {
            _holDbContext = holDbContext;
        }

        public EmployeeEntity GetSingle(int id)
        {
            return _holDbContext.Employees.FirstOrDefault(x => x.Id == id);
        }

        public void Add(EmployeeEntity item)
        {
            _holDbContext.Employees.Add(item);
        }

        public void Delete(int id)
        {
            EmployeeEntity holItem = GetSingle(id);
            _holDbContext.Employees.Remove(holItem);
        }

        public EmployeeEntity Update(int id, EmployeeEntity item)
        {
            _holDbContext.Employees.Update(item);
            return item;
        }

        public IQueryable<EmployeeEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<EmployeeEntity> _allItems = _holDbContext.Employees.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());


            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

    public IEnumerable<EmployeeEntity> GetAll()
    {
      var _allItems = _holDbContext.Employees.OrderBy(x =>x.Id).ToList();

      return _allItems;
    }

    public int Count()
        {
            return _holDbContext.Employees.Count();
        }

        public bool Save()
        {
            return (_holDbContext.SaveChanges() >= 0);
        }

        public ICollection<EmployeeEntity> GetRandomMeal()
        {
            List<EmployeeEntity> toReturn = new List<EmployeeEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private EmployeeEntity GetRandomItem(string type)
        {
            return _holDbContext.Employees
                .OrderBy(o => o.Id)
                .FirstOrDefault();
        }

    public IEnumerable<EmployeeEntity> GetAllBySearchString(string searchString)
    {
      var _allItems = _holDbContext.Employees
        .Where(x => x.LastName.Contains(searchString) 
        || x.FirstName.Contains(searchString)
        || x.Phone.Contains(searchString)
        || x.Email.Contains(searchString)).ToList();

      return _allItems;
    }
  }
}
