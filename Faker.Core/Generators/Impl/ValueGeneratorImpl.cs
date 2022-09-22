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

