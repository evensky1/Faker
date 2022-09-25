using System.Reflection;
using Faker.Core.Generators.Impl;

namespace Faker.Core.Generators;

public class CommonGenerator
{
    private readonly IReadOnlyList<IValueGenerator> _generators;

    public CommonGenerator()
    {
        var types = Assembly.GetAssembly(typeof(ValueGeneratorImplDummie))?.GetTypes();
        _generators =
            (
                from type in types
                where type.GetInterface("IValueGenerator") != null
                select (IValueGenerator)Activator.CreateInstance(type)! ?? throw new InvalidOperationException()
            )
            .ToList();
    }

    public object Generate(Type typeToGenerate)
    {
        try
        {
            var gen = _generators
                .Where(g => g.CanGenerate(typeToGenerate))
                .DefaultIfEmpty(new ObjectGenerator())
                .First();
            return gen.Generate(typeToGenerate, new());
        }
        catch (InvalidOperationException e)
        {
            Console.Error.WriteLine(e.Message);
            throw new NotImplementedException();
        }
    }
}