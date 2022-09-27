using System.Reflection;
using Faker.Core.Generators.Impl;

namespace Faker.Core.Generators;

public class CommonGenerator
{
    private readonly IReadOnlyList<IValueGenerator> _generators;

    public CommonGenerator()
    {
        var types = Assembly.GetAssembly(typeof(ValueGeneratorImplDummie))?.GetTypes();
        _generators = new List<IValueGenerator>()
        {
            new BoolGenerator(),
            new ByteGenerator(), new Int16Generator(), new Int32Generator(), new Int64Generator(),
            new UInt16Generator(), new UInt32Generator(), new UInt64Generator(),
            new FloatGenerator(), new DoubleGenerator(),
            new ListGenerator(),
            new StringGenerator(),
            new CharGenerator(),
            new DateTimeGenerator(),
            new ObjectGenerator()
        };
    }

    public object Generate(Type typeToGenerate)
    {
        try
        {
            var gen = _generators
                .First(g => g.CanGenerate(typeToGenerate));
            
            return gen.Generate(typeToGenerate, new());
        }
        catch (InvalidOperationException e)
        {
            Console.Error.WriteLine(e.Message);
            throw new NotImplementedException();
        }
    }
}