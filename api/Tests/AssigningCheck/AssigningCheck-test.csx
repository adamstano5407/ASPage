#load "../../.teapie/Definitions/ResponseClasses.csx"

using System.Text.Json;
using Xunit;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

tp.Test("Company entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateCompanyRequest"].StatusCode());
});

tp.Test("Second Company entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateCompanyTwoRequest"].StatusCode());
});

tp.Test("Employee entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateEmployeeRequest"].StatusCode());
});

tp.Test("Division entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateDivisionRequest"].StatusCode());
});

tp.Test("Project entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateProjectRequest"].StatusCode());
});

tp.Test("Department entity should be created successfully.", () =>
{
    Equal(201, tp.Responses["CreateDepartmentRequest"].StatusCode());
});
tp.Test("Employee should be assigned as manager on every hierarchy level.", () =>
{
    Equal(204, tp.Responses["AssignCompanyManagerRequest"].StatusCode());
    Equal(204, tp.Responses["AssignDivisionManagerRequest"].StatusCode());
    Equal(204, tp.Responses["AssignProjectManagerRequest"].StatusCode());
    Equal(204, tp.Responses["AssignDepartmentManagerRequest"].StatusCode());
});

tp.Test("Employee leadership positions should be unassigned.", async () =>
{
    Equal(
        204,
        tp.Responses["UnassignEmployeeLeadershipPositionsRequest"].StatusCode()
    );

    Equal(200, tp.Responses["GetCompanyAfterUnassignRequest"].StatusCode());
    Equal(200, tp.Responses["GetDivisionAfterUnassignRequest"].StatusCode());
    Equal(200, tp.Responses["GetProjectAfterUnassignRequest"].StatusCode());
    Equal(200, tp.Responses["GetDepartmentAfterUnassignRequest"].StatusCode());

    var company = JsonSerializer.Deserialize<CompanyResponse>(
        await tp.Responses["GetCompanyAfterUnassignRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var division = JsonSerializer.Deserialize<DivisionResponse>(
        await tp.Responses["GetDivisionAfterUnassignRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var project = JsonSerializer.Deserialize<ProjectResponse>(
        await tp.Responses["GetProjectAfterUnassignRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var department = JsonSerializer.Deserialize<DepartmentResponse>(
        await tp.Responses["GetDepartmentAfterUnassignRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    NotNull(company);
    NotNull(division);
    NotNull(project);
    NotNull(department);

    Null(company.ManagerId);
    Null(division.ManagerId);
    Null(project.ManagerId);
    Null(department.ManagerId);
});

tp.Test("Employee should be assigned again as manager on every hierarchy level.", () =>
{
    Equal(204, tp.Responses["AssignCompanyManagerAgainRequest"].StatusCode());
    Equal(204, tp.Responses["AssignDivisionManagerAgainRequest"].StatusCode());
    Equal(204, tp.Responses["AssignProjectManagerAgainRequest"].StatusCode());
    Equal(204, tp.Responses["AssignDepartmentManagerAgainRequest"].StatusCode());
});

tp.Test("Employee company should be changed.", async () =>
{
    Equal(204, tp.Responses["ChangeEmployeeCompanyRequest"].StatusCode());
    Equal(200, tp.Responses["GetEmployeeAfterCompanyChangeRequest"].StatusCode());

    var secondCompany = JsonSerializer.Deserialize<CompanyResponse>(
        await tp.Responses["CreateCompanyTwoRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var employee = JsonSerializer.Deserialize<EmployeeResponse>(
        await tp.Responses["GetEmployeeAfterCompanyChangeRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    NotNull(secondCompany);
    NotNull(employee);

    Equal(secondCompany.Id, employee.CompanyId);
});

tp.Test("Changing employee company should remove all leadership positions.", async () =>
{
    Equal(200, tp.Responses["GetCompanyAfterCompanyChangeRequest"].StatusCode());
    Equal(200, tp.Responses["GetDivisionAfterCompanyChangeRequest"].StatusCode());
    Equal(200, tp.Responses["GetProjectAfterCompanyChangeRequest"].StatusCode());
    Equal(200, tp.Responses["GetDepartmentAfterCompanyChangeRequest"].StatusCode());

    var company = JsonSerializer.Deserialize<CompanyResponse>(
        await tp.Responses["GetCompanyAfterCompanyChangeRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var division = JsonSerializer.Deserialize<DivisionResponse>(
        await tp.Responses["GetDivisionAfterCompanyChangeRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var project = JsonSerializer.Deserialize<ProjectResponse>(
        await tp.Responses["GetProjectAfterCompanyChangeRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    var department = JsonSerializer.Deserialize<DepartmentResponse>(
        await tp.Responses["GetDepartmentAfterCompanyChangeRequest"]
            .Content.ReadAsStringAsync(),
        jsonOptions
    );

    NotNull(company);
    NotNull(division);
    NotNull(project);
    NotNull(department);

    Null(company.ManagerId);
    Null(division.ManagerId);
    Null(project.ManagerId);
    Null(department.ManagerId);
});