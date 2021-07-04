using System;
using System.Collections.Generic;
using System.Linq;

namespace NewLinqFeatures
{
    class Program
    {
        static List<Person> people = new List<Person>
        {
            new Person { Id = 1, Name = "Amit", Age = 38},
            new Person { Id = 2, Name = "Ravi", Age = 36},
            new Person { Id = 3, Name = "Manish", Age = 34},
            new Person { Id = 4, Name = "Satish", Age = 29},
        };
        static void Main(string[] args)
        {
            Console.WriteLine("Hello .NET 6!"); 
            Console.WriteLine("----MaxBy and MinBy----");
            Person oldestPerson = people.MaxBy(person => person.Age);
            Person youngestPerson = people.MinBy(person => person.Age);
            Console.WriteLine($"Oldest Person using MaxBy: {oldestPerson.Name}");
            Console.WriteLine($"Youngest Person using MaxBy: {youngestPerson.Name}");

            //Without using MaxBy and MinBy
            oldestPerson = people.OrderByDescending(person => person.Age).First();
            youngestPerson = people.OrderBy(person => person.Age).First();
            Console.WriteLine($"Oldest Person without using MaxBy: {oldestPerson.Name}");
            Console.WriteLine($"Youngest Person without using MaxBy: {youngestPerson.Name}");

            Console.WriteLine("----Chunk----");

            IEnumerable<Person[]> cluster = people.Chunk(2);
            foreach(var people in cluster)
            {
                Console.WriteLine($"Cluster of {string.Join(",", people.Select(person => person.Name))}");
            }

            Console.WriteLine("----DistinctBy, UnionBy, IntersectBy, ExceptBy----");

            IEnumerable<Person> evenAgedPeople = people.Where(person => person.Age % 2 == 0);
            //Amit,Ravi,Manish
            IEnumerable<Person> personAbove35 = people.Where(person => person.Age > 35);
            //Amit,Ravi
            
            IEnumerable<Person> union = evenAgedPeople.UnionBy(personAbove35, x => x.Age);
            //Amit,Ravi,Manish
            Console.WriteLine($"Union: {string.Join(",", union.Select(person => person.Name))}");

            IEnumerable<Person> intersection = evenAgedPeople.IntersectBy(personAbove35.Select(p => p.Age), x => x.Age);
            //Amit,Ravi
            Console.WriteLine($"Intersection: {string.Join(",", intersection.Select(person => person.Name))}");

            Console.WriteLine("----Index and Range----");
            
            Person secondLastPerson = people.ElementAt(^2);
            Console.WriteLine(secondLastPerson.Name);
            //Manish

            IEnumerable<Person> take3People = people.Take(..3);
            Console.WriteLine(string.Join(",", take3People.Select(person => person.Name)));
            //Amit,Ravi,Manish

            IEnumerable<Person> skip1Person =  people.Take(1..);
            Console.WriteLine(string.Join(",", skip1Person.Select(person => person.Name)));
            //Ravi,Manish,Satish

            IEnumerable<Person> take3Skip1People = people.Take(1..3);
            Console.WriteLine(string.Join(",", take3Skip1People.Select(person => person.Name)));  
            //Ravi,Manish

            IEnumerable<Person> takeLast2People = people.Take(^2..);
            Console.WriteLine(string.Join(",", takeLast2People.Select(person => person.Name)));
            //Manish,Satish

            IEnumerable<Person> skipLast3People = people.Take(..^3);
            Console.WriteLine(string.Join(",", skipLast3People.Select(person => person.Name)));
            //Amit

            IEnumerable<Person> takeLast3SkipLast2 = people.Take(^3..^2);
            Console.WriteLine(string.Join(",", takeLast3SkipLast2.Select(person => person.Name)));
            //Ravi

            Console.WriteLine("----TryGetNonEnumeratedCount----");
            List<Person> anotherList = people.TryGetNonEnumeratedCount(out int count) ? new List<Person>(count): new List<Person>();
anotherList.AddRange(people);
            Console.WriteLine(string.Join(",", anotherList.Select(person => person.Name)));

            Console.WriteLine("----FirstOrDefault/LastOrDefault/SingleOrDefault overloads with default----");
            List<int> emptyList = new List<int>();
            int value = emptyList.FirstOrDefault(-1);
            Console.WriteLine(value);

            Console.WriteLine("----Zip with 3 parameters----");

            IEnumerable<int> ids = Enumerable.Range(1, 4);
            IEnumerable<Person> allPeople = people;
            IEnumerable<int> allAges = people.Select(person => person.Age);

            IEnumerable<(int Id, Person Person, int Age)> zipped = ids.Zip(allPeople, allAges);
            Console.WriteLine(string.Join(",", zipped.Select(x => x.Id + x.Person.Name + x.Age)));
        }
    }
}
