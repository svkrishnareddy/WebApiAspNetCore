using System;
using System.Linq;
using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Repositories;
using System.Collections.Generic;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Helpers;
using System.Text.Json;

namespace SampleWebApiAspNetCore.v1.Controllers
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v1/[controller]")]
  public class EmployeeController : ControllerBase
  {
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUrlHelper _urlHelper;
    private readonly IMapper _mapper;

    public EmployeeController(
        IUrlHelper urlHelper,
        IEmployeeRepository employeeRepository,
        IMapper mapper)
    {
      _employeeRepository = employeeRepository;
      _mapper = mapper;
      _urlHelper = urlHelper;
    }

    [HttpGet(Name = nameof(GetAllEmployees))]
    public ActionResult GetAllEmployees(string searchText = null)
    {
      List<EmployeeEntity> employeeItems;

      if(string.IsNullOrEmpty(searchText))
      {
        employeeItems = _employeeRepository.GetAll().ToList();
      }
      else
      {
        employeeItems = _employeeRepository.GetAllBySearchString(searchText).ToList();
      }

      var allItemCount = employeeItems.Count();
      var links = CreateLinksForCollection(allItemCount);
      var toReturn = employeeItems.Select(x => ExpandSingleEmployeeItem(x, ApiVersion.Default));

      return Ok(new
      {
        value = toReturn,
        links = allItemCount > 0 ? links : new List<LinkDto>()
      }); ;
    }

    [HttpGet]
    [Route("{id:int}", Name = nameof(GetSingleEmployee))]
    public ActionResult GetSingleEmployee(ApiVersion version, int id)
    {
      EmployeeEntity employeeItem = _employeeRepository.GetSingle(id);

      if (employeeItem == null)
      {
        return NotFound();
      }

      return Ok(ExpandSingleEmployeeItem(employeeItem, version));
    }

    [HttpPost(Name = nameof(AddEmployee))]
    public ActionResult<EmployeeEntity> AddEmployee(ApiVersion version, [FromBody] EmployeeEntity employeeCreateDto)
    {
      if (employeeCreateDto == null)
      {
        return BadRequest();
      }

      _employeeRepository.Add(employeeCreateDto);

      if (!_employeeRepository.Save())
      {
        throw new Exception("Creating a employeeitem failed on save.");
      }

      EmployeeEntity newEmployeeItem = _employeeRepository.GetSingle(employeeCreateDto.Id);

      return CreatedAtRoute(nameof(GetSingleEmployee),
          new { version = version.ToString(), id = newEmployeeItem.Id },
          _mapper.Map<EmployeeEntity>(newEmployeeItem));
    }

    [HttpDelete]
    [Route("{id:int}", Name = nameof(RemoveEmployee))]
    public ActionResult RemoveEmployee(int id)
    {
      EmployeeEntity employeeItem = _employeeRepository.GetSingle(id);

      if (employeeItem == null)
      {
        return NotFound();
      }

      _employeeRepository.Delete(id);

      if (!_employeeRepository.Save())
      {
        throw new Exception("Deleting a employeeitem failed on save.");
      }

      return NoContent();
    }

    private List<LinkDto> CreateLinksForCollection(int totalCount = 0)
    {
      var links = new List<LinkDto>();
      links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEmployees), null), "update", "PUT"));
      links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllEmployees), null), "delete", "DELEET"));


      return links;
    }

    private dynamic ExpandSingleEmployeeItem(EmployeeEntity employeeItem, ApiVersion version)
    {
      var links = GetLinks(employeeItem.Id, version);

      var resourceToReturn = employeeItem.ToDynamic() as IDictionary<string, object>;
      resourceToReturn.Add("links", links);

      return resourceToReturn;
    }

    private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
    {
      var links = new List<LinkDto>();

      var getLink = _urlHelper.Link(nameof(GetSingleEmployee), new { version = version.ToString(), id = id });

      links.Add(
        new LinkDto(getLink, "self", "GET"));

      var deleteLink = _urlHelper.Link(nameof(RemoveEmployee), new { version = version.ToString(), id = id });

      links.Add(
        new LinkDto(deleteLink,
        "delete_employee",
        "DELETE"));

      var createLink = _urlHelper.Link(nameof(AddEmployee), new { version = version.ToString() });

      links.Add(
        new LinkDto(createLink,
        "create_employee",
        "POST"));


      return links;
    }
  }
}
