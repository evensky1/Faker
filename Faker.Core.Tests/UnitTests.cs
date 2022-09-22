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
        });
    }
}