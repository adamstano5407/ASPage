#load "../../.teapie/Definitions/Company/Company.csx"
#load "../../.teapie/Definitions/Division/Division.csx"
#load "../../.teapie/Definitions/Project/Project.csx"
#load "../../.teapie/Definitions/Department/Department.csx"
#load "../../.teapie/Definitions/Employee/Employee.csx"
#load "../../.teapie/Definitions/Generator.csx"
#load "../../.teapie/Definitions/SetObjectVariable.csx"

const string Tag = "hierarchy-001";

var company = GenerateCompany();
var companyTwo = GenerateCompany();
var employee = GenerateEmployee();
var division = GenerateDivision();
var project = GenerateProject();
var department = GenerateDepartment();

tp.SetVariable("Company", company.ToJsonString(), Tag);
tp.SetVariable("CompanyTwo", companyTwo.ToJsonString(), Tag);
SetObjectVariables("Employee", employee, Tag);
SetObjectVariables("Division", division, Tag);
SetObjectVariables("Project", project, Tag);
SetObjectVariables("Department", department, Tag);