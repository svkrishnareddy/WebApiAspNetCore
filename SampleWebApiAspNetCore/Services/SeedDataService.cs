using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;
using System;
using System.Threading.Tasks;

namespace SampleWebApiAspNetCore.Services
{
  public class SeedDataService : ISeedDataService
  {
    public async Task Initialize(HolDbContext context)
    {
      context.Employees.Add(new EmployeeEntity() { Id = 1, FirstName = "Fanme1", LastName = "LName1", Email = "name1@gmail.com", Phone = "98564453531", CreatedAt = DateTime.Now });
      context.Employees.Add(new EmployeeEntity() { Id = 2, FirstName = "Fname2", LastName = "LName2", Email = "name2@gmail.com", Phone = "98564453532", CreatedAt = DateTime.Now });
      context.Employees.Add(new EmployeeEntity() { Id = 3, FirstName = "Fname3", LastName = "LName3", Email = "name3@gmail.com", Phone = "98564453533", CreatedAt = DateTime.Now });
      context.Employees.Add(new EmployeeEntity() { Id = 4, FirstName = "Fname4", LastName = "LName4", Email = "name4@gmail.com", Phone = "98564453534", CreatedAt = DateTime.Now });
      await context.SaveChangesAsync();
    }
  }
}
