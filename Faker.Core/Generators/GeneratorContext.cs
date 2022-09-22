using Faker.Core.FakerImpl;

namespace Faker.Core.Generators;

public class GeneratorContext
{
    public Random Random { get; }
    public IFaker Faker { get; }

    public GeneratorContext()
    {
        Random = new Random(Guid.NewGuid().GetHashCode());
        Faker = new FakerImpl.Faker();
    }
    public GeneratorContext(Random random, IFaker faker)
    {
        Random = random;
        Faker = faker;
    }
}