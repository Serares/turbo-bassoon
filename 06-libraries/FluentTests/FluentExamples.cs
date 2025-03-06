using FluentAssertions;
using FluentAssertions.Extensions; // to use February, March extension methods;
namespace FluentTests;

public class FluentExamples
{
    [Fact]
    public void TestString()
    {
        string city = "Bucharest";
        string expectedCity = "Bucharest";

        city.Should().StartWith("B")
        .And.EndWith("st")
        .And.Contain("uch")
        .And.HaveLength(9);

        city.Should().NotBeNull()
        .And.Be("Bucharest")
        .And.BeSameAs(expectedCity)
        .And.BeOfType<string>();

        city.Length.Should().Be(9);
    }
    [Fact]
    public void TestCollections()
    {
        string[] names = { "Ion", "Maria", "Mircea" };
        names.Should().HaveCountLessThan(4, "because the maximum items should be 3 or fewer");
        names.Should().OnlyContain(name => name.Length <= 6);
    }
    [Fact]
    public void TestDateTimes()
    {
        DateTime when = new(
          hour: 9, minute: 30, second: 0,
          day: 25, month: 3, year: 2024);

        when.Should().Be(25.March(2024).At(9, 30));

        when.Should().BeOnOrAfter(23.March(2024));

        when.Should().NotBeSameDateAs(12.February(2024));

        when.Should().HaveYear(2024);

        DateTime due = new(
          hour: 13, minute: 0, second: 0,
          day: 25, month: 3, year: 2024);

        when.Should().BeAtLeast(2.Hours()).Before(due);
    }
}
