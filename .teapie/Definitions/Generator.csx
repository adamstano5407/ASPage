#load "CompanyFaker.csx"
#load "EmployeeFaker.csx"
#load "DivisionFaker.csx"
#load "ProjectFaker.csx"
#load "DepartmentFaker.csx"

public Company GenerateCompany() => new CompanyFaker().Generate();
public Employee GenerateEmployee() => new EmployeeFaker().Generate();
public Division GenerateDivision() => new DivisionFaker().Generate();
public Project GenerateProject() => new ProjectFaker().Generate();
public Department GenerateDepartment() => new DepartmentFaker().Generate();

