namespace Faker.Core.Generators;

public class GeneratorContext
{
    public Random Random { get; }
    public FakerImpl.Faker Faker { get; }

    public GeneratorContext()
    {
        Random = new Random(Guid.NewGuid().GetHashCode());
        Faker = new FakerImpl.Faker();
    }
    public GeneratorContext(Random random,  FakerImpl.Faker faker)
    {
        Random = random;
        Faker = faker;
    }
}