#load "Company/CompanyFaker.csx"
#load "Employee/EmployeeFaker.csx"
#load "Division/DivisionFaker.csx"
#load "Project/ProjectFaker.csx"
#load "Department/DepartmentFaker.csx"

public Company GenerateCompany() => new CompanyFaker().Generate();
public Employee GenerateEmployee() => new EmployeeFaker().Generate();
public Division GenerateDivision() => new DivisionFaker().Generate();
public Project GenerateProject() => new ProjectFaker().Generate();
public Department GenerateDepartment() => new DepartmentFaker().Generate();

