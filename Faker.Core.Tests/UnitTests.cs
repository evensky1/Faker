using NotImplementedException = System.NotImplementedException;

namespace Faker.Core.Tests;

public class Tests
{
    private FakerImpl.Faker _faker;
    
    [SetUp]
    public void Setup()
    {
        _faker = new FakerImpl.Faker();
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
        });
    }

    [Test]
    public void Not_Supported_Type()
    {
        Assert.Throws<NotImplementedException>(() => _faker.Create<Tests>());
    }

    [Test]
    public void Arithmetic_Operations_With_Integers()
    {
        long a = _faker.Create<long>();
        int b = _faker.Create<int>();
        short c = _faker.Create<short>();
        byte d = _faker.Create<byte>();
        Assert.DoesNotThrow(() =>
        {
            var l = a + b + c + d;
        });
    }
    
    [Test]
    public void Arithmetic_Operations_With_Unsigned_Integers()
    {
        ulong a = _faker.Create<ulong>();
        uint b = _faker.Create<uint>();
        ushort c = _faker.Create<ushort>();
        Assert.DoesNotThrow(() =>
        {
            var l = a + b + c;
        });
    }

    [Test]
    public void Arithmetic_Operations_With_Reals()
    {
        double a = _faker.Create<double>();
        float b = _faker.Create<float>();
        
        Assert.DoesNotThrow(() =>
        {
            var d = a + b;
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
    public void DateTime_Generation_By_Format()
    {
        Assert.DoesNotThrow(() =>
        {
            DateTime dateTime = _faker.Create<DateTime>();
            Console.WriteLine(dateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        });
    }
}