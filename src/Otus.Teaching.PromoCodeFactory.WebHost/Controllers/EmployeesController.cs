

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Сотрудники
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController
    : ControllerBase
{
    private readonly IRepository<Employee> _employeeRepository;

    public EmployeesController(IRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Получить данные всех сотрудников
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        var employeesModelList = employees.Select(x =>
            new EmployeeShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FullName,
            }).ToList();

        return employeesModelList;
    }

    /// <summary>
    /// Получить данные сотрудника по Id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        var employeeModel = new EmployeeResponse()
        {
            Id = employee.Id,
            Email = employee.Email,
            Roles = employee.Roles.Select(x => new RoleItemResponse()
            {
                Name = x.Name,
                Description = x.Description
            }).ToList(),
            FullName = employee.FullName,
            AppliedPromocodesCount = employee.AppliedPromocodesCount
        };

        return employeeModel;
    }
    /// <summary>
    /// Удалить данные сотрудника
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var res = _employeeRepository.Delete(id);
        if (!res) return NotFound();
        return Ok();
    }
    /// <summary>
    /// Добавить новоую запись сотрудника
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Add(EmployeeShortAdd employee)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
        if (existingEmployee != null)
            return BadRequest();

        var res = _employeeRepository.CreateAsync(new Employee
        {
            Id = employee.Id,
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName
        });
        return Ok();
    }
    /// <summary>
    /// Изменить данные сотрудника
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newId"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Email"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, Guid? newId, string? FirstName, string? LastName, string? Email)
    {
        if (newId == null & FirstName == null & LastName == null & Email == null)
        {
            return BadRequest("All Employee object fields cannot be null.");
        }
        Employee? existingEmployee = null;
        existingEmployee = await _employeeRepository.GetByIdAsync(id);
        if (existingEmployee == null)
        {
            return BadRequest("Employee with the specified ID doesn't exist.");
        }
        if (newId != null)
        {
            var updatedEmployee = await _employeeRepository.GetByIdAsync(newId ?? default);
            if (updatedEmployee != null)
            {
                return BadRequest("Employee with the specified ID already exist.");
            }
        }

        Employee employee = new()
        {
            Email = Email ?? existingEmployee!.Email,
            FirstName = FirstName ?? existingEmployee.FirstName,
            LastName = LastName ?? existingEmployee.LastName,
            Id = newId ?? existingEmployee.Id
        };
        await _employeeRepository.UpdateAsync(employee);
        return NoContent();
    }
}