using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Faker.Core.Generators.Impl;

public class ValueGeneratorImplDummie
{
    public ValueGeneratorImplDummie()
    {
    }
}

public class BoolGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(bool)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToBoolean(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Boolean");
    }
}

public class ByteGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(byte)];
        context.Random.NextBytes(bytes);
        return bytes[0];
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Byte");
    }
}

public class Int16Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(short)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToInt16(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Int16");
    }
}

public class Int32Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(int)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToInt32(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Int32");
    }
}

public class Int64Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(long)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToInt64(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Int64");
    }
}

public class UInt16Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(ushort)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToUInt16(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("UInt16");
    }
}

public class UInt32Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(uint)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToUInt32(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("UInt32");
    }
}

public class UInt64Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(ulong)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("UInt64");
    }
}

public class FloatGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(float)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToSingle(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Single");
    }
}

public class DoubleGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(double)];
        context.Random.NextBytes(bytes);
        return BitConverter.ToDouble(bytes);
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Double");
    }
}

public class StringGenerator : IValueGenerator
{
    private const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var res = new StringBuilder();
        var topBorder = context.Random.Next(2, 64);
        for (int i = 0; i < topBorder; i++)
        {
            res.Append(chars[context.Random.Next(chars.Length)]);
        }

        return res.ToString();
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("String");
    }
}

public class CharGenerator : IValueGenerator
{
    private const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return chars[context.Random.Next(chars.Length)];
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("Char");
    }
}

public class DateTimeGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return new DateTime(context.Random.NextInt64(DateTime.Now.Ticks));
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("DateTime");
    }
}

public class ObjectGenerator : IValueGenerator
{
    [ThreadStatic]
    private static readonly List<Type> t_prevTypes = new ();
    
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        try
        {
            return GenerateUnsafe(typeToGenerate, context);
        }
        catch (StackOverflowException e)
        {
            Console.WriteLine(e);
            return Activator.CreateInstance(typeToGenerate, null);
        }
    }
    
    private object GenerateUnsafe(Type typeToGenerate, GeneratorContext context)
    {
        if (t_prevTypes.Contains(typeToGenerate)) 
        {
            throw new StackOverflowException();
        }
        
        t_prevTypes.Add(typeToGenerate);
        
        var largestCtor =
            typeToGenerate
                .GetConstructors()
                .MaxBy(c => c.GetParameters().Length);

        var initializedParams = largestCtor?.GetParameters()
            .Select(parameterInfo => ReflectionMethodCall(parameterInfo.ParameterType, context))
            .Select(createMethod => createMethod?.Invoke(context.Faker, null)).ToArray();

        var obj = Activator.CreateInstance(typeToGenerate, initializedParams);

        foreach (var propInfo in typeToGenerate.GetProperties())
        {
            var value = propInfo.GetValue(obj, null);
            if (value != null
                && !string.IsNullOrEmpty(value.ToString())
                && !value.ToString().Equals("0")) continue;

            var createMethod = ReflectionMethodCall(propInfo.PropertyType, context);

            typeToGenerate.InvokeMember(propInfo.Name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, obj, new[] { createMethod?.Invoke(context.Faker, null) });
        }

        t_prevTypes.Remove(typeToGenerate);
        return obj;
    }

    public bool CanGenerate(Type type)
    {
        return false;
    }

    private MethodInfo? ReflectionMethodCall(Type type, GeneratorContext context)
    {
        return context.Faker
            .GetType()
            .GetMethod("Create")
            ?.MakeGenericMethod(type);
    }
}

public class ListGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var createMethod = context.Faker
            .GetType()
            .GetMethod("Create")
            ?.MakeGenericMethod(typeToGenerate.GenericTypeArguments[0]);

        var listType = typeof(List<>).MakeGenericType(typeToGenerate.GenericTypeArguments[0]);
        var list = (IList)Activator.CreateInstance(listType);

        int randomLength = context.Random.Next(2, 10);
        for (int i = 0; i < randomLength; i++)
        {
            var current = createMethod?.Invoke(context.Faker, null);
            if (current != null) list.Add(current);
        }

        return list;
    }

    public bool CanGenerate(Type type)
    {
        return type.Name.Equals("List`1");
    }
}