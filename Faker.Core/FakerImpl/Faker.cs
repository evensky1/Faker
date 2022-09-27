using Faker.Core.Generators;

namespace Faker.Core.FakerImpl;

public class Faker : IFaker
{
    public T Create<T>()
    {
        return (T) Create(typeof(T));
    }

    public object Create(Type t)
    {
        var gen = new CommonGenerator();
        return gen.Generate(t);
    }
    
}