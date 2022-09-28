using System.Collections;
using System.Reflection;
using System.Text;

namespace Faker.Core.Generators.Impl;

public class BoolGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return true;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(bool);
    }
}

public class ByteGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(byte)];
        do
        {
            context.Random.NextBytes(bytes);
        } while (bytes[0] == 0);

        return bytes[0];
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(byte);
    }
}

public class Int16Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(short)];
        context.Random.NextBytes(bytes);
        short value = 0;
        do
        {
            value = BitConverter.ToInt16(bytes);
        } while (value == 0);
        
        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(short);
    }
}

public class Int32Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(int)];
        context.Random.NextBytes(bytes);
        
        int value = 0;
        do
        {
            value = BitConverter.ToInt32(bytes);
        } while (value == 0);
        
        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(int);
    }
}

public class Int64Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(long)];
        context.Random.NextBytes(bytes);
        
        long value = 0;
        do
        {
            value = BitConverter.ToInt64(bytes);
        } while (value == 0);
        
        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(long);
    }
}

public class UInt16Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(ushort)];
        context.Random.NextBytes(bytes);
        
        ushort value = 0;
        do
        {
            value = BitConverter.ToUInt16(bytes);
        } while (value == 0);
        
        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(ushort);
    }
}

public class UInt32Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(uint)];
        context.Random.NextBytes(bytes);
        uint value = 0;
        do
        {
            value = BitConverter.ToUInt32(bytes);
        } while (value == 0);

        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(uint);
    }
}

public class UInt64Generator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(ulong)];
        context.Random.NextBytes(bytes);
        ulong value = 0;
        do
        {
            value = BitConverter.ToUInt64(bytes);
        } while (value == 0);
        
        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(ulong);
    }
}

public class FloatGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(float)];
        context.Random.NextBytes(bytes);
        float value = 0;
        do
        {
            value = BitConverter.ToSingle(bytes);
        } while (value == 0);

        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(float);
    }
}

public class DoubleGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var bytes = new byte[sizeof(double)];
        context.Random.NextBytes(bytes); 
        double value = 0;
        do
        {
            value = BitConverter.ToDouble(bytes);
        } while (value == 0);

        return value;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(double);
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
        return type == typeof(string);
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
        return type == typeof(char);
    }
}

public class DateTimeGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        DateTime dateTime;
        do
        {
            dateTime = new DateTime(context.Random.NextInt64(DateTime.Now.Ticks));
        } while (dateTime == DateTime.MinValue);

        return dateTime;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(DateTime);
    }
}

public class ObjectGenerator : IValueGenerator
{
    [ThreadStatic] private static readonly List<Type> t_prevTypes = new();

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        try
        {
            return GenerateUnsafe(typeToGenerate, context);
        }
        catch (Exception e)
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
            .Select(parameterInfo => context.Faker.Create(parameterInfo.ParameterType))
            .ToArray();

        var obj = Activator.CreateInstance(typeToGenerate, initializedParams);

        foreach (var propInfo in typeToGenerate.GetProperties())
        {
            var value = propInfo.GetValue(obj, null);

            if (value != null
                && !string.IsNullOrEmpty(value.ToString())
                && !value.ToString().Equals("0")) continue;

            typeToGenerate.InvokeMember(propInfo.Name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, obj, new[] { context.Faker.Create(propInfo.PropertyType) });
        }

        t_prevTypes.Remove(typeToGenerate);
        return obj;
    }

    public bool CanGenerate(Type type)
    {
        return true;
    }
}

public class ListGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var listType = typeof(List<>).MakeGenericType(typeToGenerate.GenericTypeArguments[0]);
        var list = (IList)Activator.CreateInstance(listType)!;

        int randomLength = context.Random.Next(2, 10);
        for (int i = 0; i < randomLength; i++)
        {
            var current = context.Faker.Create(typeToGenerate.GetGenericArguments()[0]);
            list?.Add(current);
        }

        return list;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType
               && type.GetGenericTypeDefinition() == typeof(List<>);
    }
}