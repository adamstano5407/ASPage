#load "../../.teapie/Definitions/ResponseClasses.csx"

using System.Text.Json;
using Xunit;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

void AssertStatus(string requestName, int expectedStatus)
{
    Assert.True(
        tp.Responses.ContainsKey(requestName),
        $"Response '{requestName}' was not found."
    );

    Equal(expectedStatus, tp.Responses[requestName].StatusCode());
}

tp.Test("Base hierarchy should be created", () =>
{
    AssertStatus("CreateCompanyRequest", 201);
    AssertStatus("CreateEmployeeRequest", 201);
    AssertStatus("CreateDivisionRequest", 201);
    AssertStatus("CreateProjectRequest", 201);
    AssertStatus("CreateDepartmentRequest", 201);
});

tp.Test("Second records used for UPDATE tests should be created", () =>
{
    AssertStatus("CreateEmployee2Request", 201);
    AssertStatus("CreateDivision2Request", 201);
    AssertStatus("CreateProject2Request", 201);
    AssertStatus("CreateDepartment2Request", 201);
});

tp.Test("CREATE duplicate company should return Bad Request 400", () =>
{
    AssertStatus("CreateCompanyCloneRequest", 400);
    AssertStatus("CreateCompanyCodeCloneRequest", 400);
});

tp.Test("CREATE duplicate employee should return Bad Request 400", () =>
{
    AssertStatus("CreateEmployeeCloneRequest", 400);
    AssertStatus("CreateEmployeeEmailCloneRequest", 400);
    AssertStatus("CreateEmployeeNumberCloneRequest", 400);
});

tp.Test("CREATE duplicate division should return Bad Request 400", () =>
{
    AssertStatus("CreateDivisionCloneRequest", 400);
    AssertStatus("CreateDivisionCodeCloneRequest", 400);
});

tp.Test("CREATE duplicate project should return Bad Request 400", () =>
{
    AssertStatus("CreateProjectCloneRequest", 400);
    AssertStatus("CreateProjectCodeCloneRequest", 400);
});

tp.Test("CREATE duplicate department should return Bad Request 400", () =>
{
    AssertStatus("CreateDepartmentCloneRequest", 400);
    AssertStatus("CreateDepartmentCodeCloneRequest", 400);
});


tp.Test("UPDATE employee to duplicate email should return Bad Request 400", () =>
{
    AssertStatus("UpdateEmployeeEmailCloneRequest", 400);
});

tp.Test("UPDATE employee to duplicate employee number should return Bad Request 400", () =>
{
    AssertStatus("UpdateEmployeeNumberCloneRequest", 400);
});

tp.Test("UPDATE division to duplicate code in same company should return Bad Request 400", () =>
{
    AssertStatus("UpdateDivisionCodeCloneRequest", 400);
});

tp.Test("UPDATE project to duplicate code in same division should return Bad Request 400", () =>
{
    AssertStatus("UpdateProjectCodeCloneRequest", 400);
});

tp.Test("UPDATE department to duplicate code in same project should return Bad Request 400", () =>
{
    AssertStatus("UpdateDepartmentCodeCloneRequest", 400);
});


tp.Test("Second test records should be deleted", () =>
{
    AssertStatus("DeleteDepartment2Request", 204);
    AssertStatus("DeleteProject2Request", 204);
    AssertStatus("DeleteDivision2Request", 204);
    AssertStatus("DeleteEmployee2Request", 204);
});

tp.Test("Base hierarchy should be deleted", () =>
{
    AssertStatus("DeleteDepartmentRequest", 204);
    AssertStatus("DeleteProjectRequest", 204);
    AssertStatus("DeleteDivisionRequest", 204);
    AssertStatus("DeleteEmployeeRequest", 204);
    AssertStatus("DeleteCompanyRequest", 204);
});