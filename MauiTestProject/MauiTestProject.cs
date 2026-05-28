namespace MauiTestProject;
using Xunit;

public class Person
{
	public int Id { get; set; }
	public string UserId { get; set; }
	public string Name { get; set; }
	public int BirthYear { get; set; }
	public int BirthMonth { get; set; }
	public int BirthDayOfMonth { get; set; }
	public string? Remarks { get; set; }
	public string? PictureUrl { get; set; }
	public int Age { get; set; }
}
public class MauiTestProject
{
	[Fact]
	public void SortByNameAlphabeticalOrder()
	{
		// Arrange
		var people = new List<Person>
		{
			new Person { Name = "Charlie", BirthMonth = 3, BirthDayOfMonth = 1, BirthYear = 1990 },
			new Person { Name = "Alice",   BirthMonth = 6, BirthDayOfMonth = 15, BirthYear = 1985 },
			new Person { Name = "Bob",     BirthMonth = 1, BirthDayOfMonth = 22, BirthYear = 2000 }
		};

		// Act
		var sorted = people.OrderBy(p => p.Name).ToList();

		// Assert
		Assert.Equal("Alice", sorted[0].Name);
		Assert.Equal("Bob", sorted[1].Name);
		Assert.Equal("Charlie", sorted[2].Name);
	}

	[Theory]
	[InlineData("alice", true)]   // lowercase should still match
	[InlineData("Ali", true)]     // partial match should work
	[InlineData("xyz", false)]    // no match
	[InlineData("", true)]        // empty query returns everyone
	public void FilterByName(string query, bool expectResult)
	{
		// Arrange
		var people = new List<Person>
		{
			new Person { Name = "Alice", BirthMonth = 6, BirthDayOfMonth = 15, BirthYear = 1985 }
		};

		// Act
		var result = string.IsNullOrEmpty(query)
			? people
			: people.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();

		// Assert
		Assert.Equal(expectResult, result.Any());
	}
}
