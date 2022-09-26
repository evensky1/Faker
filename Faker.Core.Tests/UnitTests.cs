using System.Collections;

namespace Faker.Core.Tests;

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }

    public int Height { get; set; }

    public long Id { get; set; }

    public User(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public User()
    {
    }
}

public class Student
{
    public Card Card { get; set; }
    public string Name { get; set; }
    public School School { get; set; }
}

public class Card
{
    public int Num { get; set; }
    public School School { get; set; }
}

public class School
{
    public string Name { get; set; }
    public List<Student> Students { get; set; }
}

public class Tests
{
    private FakerImpl.Faker _faker;

    [SetUp]
    public void Setup()
    {
        _faker = new FakerImpl.Faker();
    }

    [Test]
    public void Circular_Dependency()
    {
        var temp = _faker.Create<School>();
        Assert.That(typeof(School), Is.EqualTo(temp.GetType()));
    }
    [Test]
    public void Type_Match_Default_Scenario()
    {
        Assert.Multiple(() =>
        {
            // ReSharper disable HeapView.BoxingAllocation
            Assert.That(typeof(bool), Is.EqualTo(_faker.Create<bool>().GetType()));
            Assert.That(typeof(byte), Is.EqualTo(_faker.Create<byte>().GetType()));
            Assert.That(typeof(short), Is.EqualTo(_faker.Create<short>().GetType()));
            Assert.That(typeof(int), Is.EqualTo(_faker.Create<int>().GetType()));
            Assert.That(typeof(long), Is.EqualTo(_faker.Create<long>().GetType()));
            Assert.That(typeof(ushort), Is.EqualTo(_faker.Create<ushort>().GetType()));
            Assert.That(typeof(uint), Is.EqualTo(_faker.Create<uint>().GetType()));
            Assert.That(typeof(ulong), Is.EqualTo(_faker.Create<ulong>().GetType()));
            Assert.That(typeof(float), Is.EqualTo(_faker.Create<float>().GetType()));
            Assert.That(typeof(double), Is.EqualTo(_faker.Create<double>().GetType()));
            Assert.That(typeof(string), Is.EqualTo(_faker.Create<string>().GetType()));
            Assert.That(typeof(char), Is.EqualTo(_faker.Create<char>().GetType()));
            Assert.That(typeof(DateTime), Is.EqualTo(_faker.Create<DateTime>().GetType()));
            Assert.That(typeof(User), Is.EqualTo(_faker.Create<User>().GetType()));
            Assert.That(typeof(List<List<List<User>>>), Is.EqualTo(_faker.Create<List<List<List<User>>>>().GetType()));
        });
    }

    [Test]
    public void Random_Generation_With_Integers()
    {
        var listOfLongs = new List<long>();
        var listOfInts = new List<int>();
        var listOfShorts = new List<short>();
        var listOfBytes = new List<byte>();

        for (int i = 0; i < 100; i++)
        {
            listOfLongs.Add(_faker.Create<long>());
            listOfInts.Add(_faker.Create<int>());
            listOfShorts.Add(_faker.Create<short>());
            listOfBytes.Add(_faker.Create<byte>());
        }

        Assert.Multiple(() =>
        {
            Assert.That
            (
                listOfLongs.FindAll(l => l.Equals(long.MinValue) || l.Equals(0L)),
                Has.Count.Not.EqualTo(listOfLongs.Count)
            );
            Assert.That
            (
                listOfInts.FindAll(i => i.Equals(int.MinValue) || i.Equals(0)),
                Has.Count.Not.EqualTo(listOfInts.Count)
            );
            Assert.That
            (
                listOfShorts.FindAll(s => s.Equals(short.MinValue) || s.Equals(0)),
                Has.Count.Not.EqualTo(listOfLongs.Count)
            );
            Assert.That
            (
                listOfLongs.FindAll(b => b.Equals(long.MinValue) || b.Equals(0)),
                Has.Count.Not.EqualTo(listOfLongs.Count)
            );
        });
    }

    [Test]
    public void Random_Generation_With_Unsigned_Integers()
    {
        var listOfLongs = new List<ulong>();
        var listOfInts = new List<uint>();
        var listOfShorts = new List<ushort>();

        for (int i = 0; i < 100; i++)
        {
            listOfLongs.Add(_faker.Create<ulong>());
            listOfInts.Add(_faker.Create<uint>());
            listOfShorts.Add(_faker.Create<ushort>());
        }

        Assert.Multiple(() =>
        {
            Assert.That
            (
                listOfLongs.FindAll(ul => ul.Equals(0L)),
                Has.Count.Not.EqualTo(listOfLongs.Count)
            );
            Assert.That
            (
                listOfInts.FindAll(ui => ui.Equals(0)),
                Has.Count.Not.EqualTo(listOfInts.Count)
            );
            Assert.That
            (
                listOfShorts.FindAll(us => us.Equals(0)),
                Has.Count.Not.EqualTo(listOfLongs.Count)
            );
        });
    }

    [Test]
    public void Random_Generation_With_Reals()
    {
        var listOfFloats = new List<float>();
        var listOfDoubles = new List<double>();

        for (int i = 0; i < 100; i++)
        {
            listOfFloats.Add(_faker.Create<float>());
            listOfDoubles.Add(_faker.Create<double>());
        }

        Assert.Multiple(() =>
        {
            Assert.That
            (
                listOfFloats.FindAll(f => f.Equals(float.MinValue) || f.Equals(0) || f.Equals(float.NaN)),
                Has.Count.Not.EqualTo(listOfFloats.Count)
            );
            Assert.That
            (
                listOfDoubles.FindAll(d => d.Equals(double.MinValue) || d.Equals(0) || d.Equals(double.NaN)),
                Has.Count.Not.EqualTo(listOfDoubles.Count)
            );
        });
    }

    [Test]
    public void String_Generation()
    {
        Assert.DoesNotThrow(() =>
        {
            string str = _faker.Create<string>();
            str = str.Trim();
            Console.WriteLine(str);
        });
    }

    [Test]
    public void String_Is_Not_Empty()
    {
        Assert.That(_faker.Create<string>(), Is.Not.Empty);
    }

    [Test]
    public void DateTime_Generation_By_Format()
    {
        Assert.DoesNotThrow(() =>
        {
            DateTime dateTime = _faker.Create<DateTime>();
            Console.WriteLine(dateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        });
    }

    [Test]
    public void DateTime_No_Default_Value()
    {
        var dates = new List<DateTime>();
        for (int i = 0; i < 100; i++)
        {
            dates.Add(_faker.Create<DateTime>());
        }

        Assert.IsNotNull(dates.Find(d => d.Equals(DateTime.MinValue)));
    }

    [Test]
    public void Default_Object_Generation()
    {
        var user = _faker.Create<User>();
        Assert.That(user.Age != 0 || user.Height != 0 || user.Id != 0 || user.Name.Length != 0, Is.True);
    }

    [Test]
    public void Default_List_Generation()
    {
        var list = _faker.Create<List<List<User>>>();
        foreach (var users in list)
        {
            Assert.That(users, Is.Not.Empty);
            foreach (var user in users)
            {
                Assert.That(user, Is.Not.Null);
            }
        }
    }
}