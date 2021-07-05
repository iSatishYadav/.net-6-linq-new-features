## What is `.NET 6` and why should I care?

`.NET 6` is the upcoming major overhaul for `.NET`. It unifies entire `.NET` experience. No more `.NET Core`, `.NET Full Framework`, `Xamarin`, `Mono`, etc. Just a single `.NET`.

## New `LINQ` Features
At the time of writing this post, 5 previews of `.NET 6` have been released and last couple of releases have been blessing for `LINQ`.

Here are few top new `LINQ` features:

* New methods `MaxBy` and `MinBy`
* New methods `Chunk`
* New methods `DistinctBy`, `UnionBy`, `IntersectBy`, and `ExceptBy`
* `Index` and `Range` parameters
* New method `TryGetNonEnumeratedCount`
* Default parameters for `FirstOrDefault`, `LastOrDefault`, and `SingleOrDefault`
* `Zip` supports 3 `IEnumerables`

## New methods `MaxBy` and `MinBy`
Finding out maximum of a minimum has been easier than even with these new `MaxBy` or `MinBy` methods.

Suppose a `List` of `Person`s as:
````csharp
static List<Person> people = new List<Person>
{
    new Person { Id = 1, Name = "Amit", Age = 38},
    new Person { Id = 2, Name = "Ravi", Age = 36},
    new Person { Id = 3, Name = "Manish", Age = 34},
    new Person { Id = 4, Name = "Satish", Age = 29},
};
````
If we need to get maximum and minimum, currently here's how to do it:

````csharp
//Without using MaxBy and MinBy
Person oldestPerson = people.OrderByDescending(person => person.Age).First();
Person youngestPerson = people.OrderBy(person => person.Age).First();
Console.WriteLine($"Oldest Person without using MaxBy: {oldestPerson.Name}");
Console.WriteLine($"Youngest Person without using MaxBy: {youngestPerson.Name}");
````

But with `MaxBy` and `MinBy`, it's just a single method call:

````csharp
Person oldestPerson = people.MaxBy(person => person.Age);
Person youngestPerson = people.MinBy(person => person.Age);
Console.WriteLine($"Oldest Person using MaxBy: {oldestPerson.Name}");
Console.WriteLine($"Youngest Person using MaxBy: {youngestPerson.Name}");
````

Neat, right? Let me know in the comments, if you have alternate methods of getting maximum and minimum using `LINQ`.

## New method `Chunk`
A new method `Chunk` slices the `IEnumerable` into provided sizes. e.g. If you have a collection of 4 elements and you need to cluster them into fixed size of 2, here's how to do:

````csharp
IEnumerable<Person[]> cluster = people.Chunk(2);
// Print each cluster.
foreach(var people in cluster)
{
    Console.WriteLine($"Cluster of {string.Join(",", people.Select(person => person.Name))}");
}
//Prints
// Cluster of Amit,Ravi
// Cluster of Manish,Satish
````

## New methods `DistinctBy`, `UnionBy`, `IntersectBy`, and `ExceptBy`

Exisiting set methods `Distinct`, `Union`, `Intersect`, and `Except` have been powered up by these new methods which can take a selector function to operate.
e.g.
````csharp
IEnumerable<Person> evenAgedPeople = people.Where(person => person.Age % 2 == 0);
//Amit,Ravi,Manish

IEnumerable<Person> personAbove35 = people.Where(person => person.Age > 35);
//Amit,Ravi

IEnumerable<Person> union = evenAgedPeople.UnionBy(personAbove35, x => x.Age);
//Amit,Ravi,Manish

IEnumerable<Person> intersection = evenAgedPeople.IntersectBy(personAbove35.Select(p => p.Age), x => x.Age);
//Amit,Ravi
````
Let me know in the comments, the usage of `DistinctBy` and `ExceptBy`.

## `Index` and `Range` parameters

`Range`: `..`, and `Index`: `^` already exist in `C#`8, `.NET 6` brings these two to `LINQ`.

* The `ElementAt` operator now takes indices from the end.

````csharp
Person secondLastPerson = people.ElementAt(^2);
//Manish
````

* `Skip` and `Take` now take `Range` as well:
````csharp
IEnumerable<Person> take3People = people.Take(..3);
//Amit,Ravi,Manish

IEnumerable<Person> skip1Person =  people.Take(1..);
//Ravi,Manish,Satish

IEnumerable<Person> take3Skip1People = people.Take(1..3);
//Ravi,Manish

IEnumerable<Person> takeLast2People = people.Take(^2..);
//Manish,Satish

IEnumerable<Person> skipLast3People = people.Take(..^3);
//Amit

IEnumerable<Person> takeLast3SkipLast2 = people.Take(^3..^2);
//Ravi

````

## New method `TryGetNonEnumeratedCount`
Sometimes you need to get a count without the enumeration. `TryGetNonEnumeratedCount` will try to get count without foring an enumeration. It internally checks for the implementation of `ICollection` i.e. `IEnumerable` that already has a mechanism to get count without forcing an enumeration. Otherwise it'll try to take advantange of new improvements in `LINQ`.
Useful in scenarios where you want to get a count but not at the cost of enumerating the `IEnumerable`.
e.g.

````csharp
List<Person> anotherList = people.TryGetNonEnumeratedCount(out int count) ? new List<Person>(count): new List<Person>();
anotherList.AddRange(people);
````
In our case right now, we know that people is just an in-memory list, but what if it was from database of some `Stream`, then it would've made sense to find out the count to optimize the instantiation of `anotherList` object by specifying `capacity` parameter.


## Default parameters for `FirstOrDefault`, `LastOrDefault`, and `SingleOrDefault`
Current `FirstOrDefault`, `LastOrDefault`, and `SingleOrDefault` methods return `default(T)` if the source `IEnumerable` is empty. The new overloads accept a parameter which will be returned if source is empty.

e.g.

````csharp
List<int> emptyList = new List<int>();
int value = emptyList.FirstOrDefault(-1);
//-1 instead of 0
````
This reminds me of `ISNULL` function of `SQL Server`.

## `Zip` supports 3 `IEnumerables`
Before `.NET 6`, [`Zip`](https://docs.microsoft.com/en-gb/dotnet/api/system.linq.enumerable.zip?view=net-5.0) used to take only 2 parameters. Now it takes 3 parameters:

````csharp
IEnumerable<int> ids = Enumerable.Range(1, 4);
IEnumerable<Person> allPeople = people;
IEnumerable<int> allAges = people.Select(person => person.Age);

IEnumerable<(int Id, Person Person, int Age)> zipped = ids.Zip(allPeople, allAges);
````

## Conclusion
`.NET 6` previews are bring awesome set of new features with every new release. Let me know in comments which all features you're excited for.

## Next Steps
* Download [`.NET 6`](https://dotnet.microsoft.com/download/dotnet/6.0) today and try out.
* Source code is available at: [https://github.com/iSatishYadav/.net-6-linq-new-features](https://shawt.io/r/sYR)

> Originally posted at: [https://blog.satishyadav.com/.net-6-linq-new-features](https://blog.satishyadav.com/.net-6-linq-new-features?utm_source=gh&utm_medium=rm)
