#load "../../.teapie/Definitions/ResponseClasses.csx"
using System.Text.Json;
using Xunit;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

tp.Test("Create requests should succeed.", () =>
{
    Equal(201, tp.Responses["CreateCompanyRequest"].StatusCode());
    Equal(201, tp.Responses["CreateEmployeeRequest"].StatusCode());
    Equal(201, tp.Responses["CreateDivisionRequest"].StatusCode());
    Equal(201, tp.Responses["CreateProjectRequest"].StatusCode());
    Equal(201, tp.Responses["CreateDepartmentRequest"].StatusCode());
});

tp.Test("Created company should be valid.", async () =>
{
    var json = await tp.Responses["CreateCompanyRequest"]
        .Content.ReadAsStringAsync();

    var company = JsonSerializer.Deserialize<CompanyResponse>(json, jsonOptions);

    NotNull(company);
    True(company.Id > 0);
});


tp.Test("Created employee should be valid.", async () =>
{
    var company = JsonSerializer.Deserialize<CompanyResponse>(
        await tp.Responses["CreateCompanyRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        await tp.Responses["CreateEmployeeRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    NotNull(company);
    NotNull(employee);

    True(employee.Id > 0);
    Equal(company.Id, employee.CompanyId);
});


tp.Test("Created division should be valid.", async () =>
{
    var company = JsonSerializer.Deserialize<CompanyResponse>(
        await tp.Responses["CreateCompanyRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        await tp.Responses["CreateEmployeeRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var division = JsonSerializer.Deserialize<DivisionResponse>(
        await tp.Responses["CreateDivisionRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    NotNull(division);

    True(division.Id > 0);
    Equal(company.Id, division.CompanyId);
    Equal(employee.Id, division.ManagerId);
});


tp.Test("Created project should be valid.", async () =>
{
    var division = JsonSerializer.Deserialize<DivisionResponse>(
        await tp.Responses["CreateDivisionRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        await tp.Responses["CreateEmployeeRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var project = JsonSerializer.Deserialize<ProjectResponse>(
        await tp.Responses["CreateProjectRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    NotNull(project);

    True(project.Id > 0);
    Equal(division.Id, project.DivisionId);
    Equal(employee.Id, project.ManagerId);
});

tp.Test("Created department should be valid.", async () =>
{
    var project = JsonSerializer.Deserialize<ProjectResponse>(
        await tp.Responses["CreateProjectRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        await tp.Responses["CreateEmployeeRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    var department = JsonSerializer.Deserialize<DepartmentResponse>(
        await tp.Responses["CreateDepartmentRequest"].Content.ReadAsStringAsync(),
        jsonOptions);

    NotNull(department);

    True(department.Id > 0);
    Equal(project.Id, department.ProjectId);
    Equal(employee.Id, department.ManagerId);
});

tp.Test("Created hierarchy should be retrievable.", () =>
{
    Equal(200, tp.Responses["GetCompanyRequest"].StatusCode());
    Equal(200, tp.Responses["GetEmployeeRequest"].StatusCode());
    Equal(200, tp.Responses["GetDivisionRequest"].StatusCode());
    Equal(200, tp.Responses["GetProjectRequest"].StatusCode());
    Equal(200, tp.Responses["GetDepartmentRequest"].StatusCode());
});


tp.Test("Deleted company should not be retrievable.", () =>
{
    Equal(404, tp.Responses["GetDeletedCompanyRequest"].StatusCode());
});

tp.Test("Deleted employee should not be retrievable.", () =>
{
    Equal(404, tp.Responses["GetDeletedEmployeeRequest"].StatusCode());
});

tp.Test("Deleted division should not be retrievable.", () =>
{
    Equal(404, tp.Responses["GetDeletedDivisionRequest"].StatusCode());
});

tp.Test("Deleted project should not be retrievable.", () =>
{
    Equal(404, tp.Responses["GetDeletedProjectRequest"].StatusCode());
});

tp.Test("Deleted department should not be retrievable.", () =>
{
    Equal(404, tp.Responses["GetDeletedDepartmentRequest"].StatusCode());
});
