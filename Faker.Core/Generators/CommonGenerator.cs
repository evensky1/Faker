using System.Reflection;
using Faker.Core.Generators.Impl;

namespace Faker.Core.Generators;

public class CommonGenerator
{
    private readonly IReadOnlyList<IValueGenerator> _generators;

    public CommonGenerator()
    {
        var types = Assembly.GetAssembly(typeof(ValueGeneratorDummie))?.GetTypes();
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
        var gen = _generators.First(g => g.CanGenerate(typeToGenerate));
        return gen.Generate(typeToGenerate, new());
    }
}