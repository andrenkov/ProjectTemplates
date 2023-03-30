using Calabonga.DemoClasses;
//using Calabonga.PredicatesBuilder;
using System.Linq;
using System.Linq.Expressions;
using static PredicateDemo.EmployeeModelExpression;
using LinqKit;

#region Old
Console.WriteLine("Loading list of People:");


var peoples = People.GetPeople();

//var countries = peoples
//    .Select(p => p.Country)
//    .Distinct();

//foreach (Country country in countries)
//{
//    Console.WriteLine($"{country}");
//}


//var folks = peoples
//    .Where(p => p.Country == Country.Russia);// "p => p.Country == Country.Russia" is the Predicate

//foreach (Person people in folks)
//{
//    Console.WriteLine($"{people.Name} from {people.Country} age {people.Age}");
//}

//var predicate = PredicateBuilder
//    .True<Person>()
//    .And(p => p.Country == Country.Germany);

//the same as above
var predicate = PredicateBuilder.True<Person>();

var predicateCountry = PredicateBuilder.True<Person>();
predicateCountry = predicateCountry.And(p => p.Country == Country.Germany);
predicateCountry = predicateCountry.Or(p => p.Country == Country.China);
predicate = predicate.And(predicateCountry);

var predicateAge = PredicateBuilder.True<Person>();
predicateAge = predicateAge.And(p => p.Age > 20 && p.Age < 35);

predicate = predicate.And(p => p.Gender == "M");


//var filterWhere = predicate.Compile();
//var peopleList = peoples.Where(filterWhere);
//var peopleList = peoples.Where(predicate.Compile());
var peopleList = peoples.Where((Func<Person, bool>)predicate.Compile());//Func<Person, bool> is a cast

foreach (Person man in peopleList)
{
    //Console.WriteLine($"name : {man.Name} country : {man.Country} age : {man.Age} gender : {man.Gender}");
}

//###################################################
#endregion Old

#region Salary
Console.WriteLine("--------------------------");
Console.WriteLine("Loading list of Employee:");


var emloyees = People.GetEmployees();
var departments = People.GetDepartments();

var predicateEmployee = PredicateBuilder.True<Employee>();
var emplFilterPoor = IsPoor();
var emplFilterRich = IsRich();
predicateEmployee = predicateEmployee.And(emplFilterPoor);
predicateEmployee = predicateEmployee.Or(emplFilterRich);


static Expression<Func<Employee, bool>> IsPoor()
    => em => em.Salary < 100;

static Expression<Func<Employee, bool>> IsRich()
    => em => em.Salary > 5000;

var emplist = emloyees.Where((Func<Employee, bool>)predicateEmployee.Compile());

foreach (Employee empl in emplist)
{
    //Console.WriteLine($"name : {empl.Name} salary : {empl.Salary}");
}
//###################################################
#endregion
Console.WriteLine("--------------------------");
Console.WriteLine("Loading department of the employee:");

int dpt = 1; //Global
string name = "Bob";

var predicateEmployeeDpt = PredicateBuilder.New<Employee>();
predicateEmployeeDpt = predicateEmployeeDpt.And(EmplIsInDepartment(dpt));

//var dptlist = emloyees.Where(GetCooworkers(name));
//foreach (Employee empl in dptlist)
//{
//    Console.WriteLine($"name : {empl.Name} dpt : {empl.DepartmentId}");
//}









