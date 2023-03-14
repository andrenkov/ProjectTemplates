using Calabonga.DemoClasses;
using Calabonga.PredicatesBuilder;

Console.WriteLine("Loading list of People");


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
predicate = predicate.And(p => p.Country == Country.Germany);

var qryWhere = predicate.Compile();
var countriesQry = peoples.Where(qryWhere);
foreach (Person man in countriesQry)
{
    Console.WriteLine($"{man.Name}");
}

