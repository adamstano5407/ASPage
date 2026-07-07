using APIKros.DTOs;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests.Employee;
using AutoMapper;
using FluentValidation;

namespace APIKros.Services;


public interface IEmployeeService : IService<EmployeeDto, CreateEmployeeRequest,  UpdateEmployeeRequest, int>
{
    public Task ChangeCompany(int employeeId, ChangeCompanyRequest request);
    public Task UnassignEmployeeFromLeadershipPositions(int employeeId);
    public Task<CompanyDto> GetCompany(int employeeId);
}


public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    
    private readonly IValidator<CreateEmployeeRequest> _createEmployeeVal;
    private readonly IValidator<UpdateEmployeeRequest> _updateEmployeeVal;
    private readonly IValidator<ChangeCompanyRequest> _changeCompanyVal;
    
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IValidator<CreateEmployeeRequest> createEmployeeVal,  IValidator<UpdateEmployeeRequest> updateEmployeeVal, IValidator<ChangeCompanyRequest> changeCompanyVal, ICompanyRepository companyRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _createEmployeeVal = createEmployeeVal;
        _updateEmployeeVal = updateEmployeeVal;
        _changeCompanyVal = changeCompanyVal;
        _companyRepository = companyRepository;
        _mapper = mapper;
    }
    
    public async Task<EmployeeDto?> GetAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee == null ? throw new NotFoundException() : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeRequest request)
    {
        await _createEmployeeVal.ValidateAndThrowAsync(request);

        var employee = new Employee
        {
            Title = request.Title,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            CompanyId = request.CompanyId,
            EmployeeNumber = request.EmployeeNumber
        };

        await _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        request.Id = id;

        var employee = await _employeeRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException();

        await _updateEmployeeVal.ValidateAndThrowAsync(request);

        employee.Title = request.Title ?? employee.Title;
        employee.FirstName = request.FirstName ?? employee.FirstName;
        employee.LastName = request.LastName ?? employee.LastName;
        employee.Email = request.Email ?? employee.Email;
        employee.Phone = request.Phone ?? employee.Phone;
        employee.EmployeeNumber = request.EmployeeNumber ?? employee.EmployeeNumber;

        if (request.CompanyId.HasValue && request.CompanyId.Value != employee.CompanyId)
        {
            await _employeeRepository.UnassignEmployeeFromLeadershipPositionsAsync(employee.Id);
            employee.CompanyId = request.CompanyId.Value;
        }

        await _employeeRepository.UpdateAsync(employee);
        await _employeeRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) throw new NotFoundException();
        await UnassignEmployeeFromLeadershipPositions(id);
        await _employeeRepository.DeleteAsync(id);
        await _employeeRepository.SaveChangesAsync();
    }

    public async Task ChangeCompany(int employeeId ,ChangeCompanyRequest request)
    {
        request.EmployeeId = employeeId;
        await _changeCompanyVal.ValidateAndThrowAsync(request);
    }

    public async Task UnassignEmployeeFromLeadershipPositions(int employeeId)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            throw new NotFoundException();
        }
        await _employeeRepository.UnassignEmployeeFromLeadershipPositionsAsync(employeeId);
    }

    public async Task<CompanyDto> GetCompany(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new NotFoundException();
        }
        var company = await _companyRepository.GetByIdAsync(employee.CompanyId);
        return _mapper.Map<CompanyDto>(company);
    }
}