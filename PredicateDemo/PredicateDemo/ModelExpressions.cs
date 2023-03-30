using Calabonga.DemoClasses;
//using Calabonga.PredicatesBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace PredicateDemo
{
    public static class EmployeeModelExpression
    {
        public static Expression<Func<Employee, bool>> EmplIsInDepartment(int dptId)
             => em => em.DepartmentId == dptId;

        public static Expression<Func<Employee, List<Employee>>>? GetCooworkers(string emplName)
        {
            var filter = PredicateBuilder.New<Employee>();
            var emplInDept = People.GetEmployees().Where(x => x.Name == emplName).FirstOrDefault().DepartmentId;
            filter = filter.And(EmplIsInDepartment(emplInDept));

            return null;// filter;//People.GetEmployees().Where(
        }
    }
}
