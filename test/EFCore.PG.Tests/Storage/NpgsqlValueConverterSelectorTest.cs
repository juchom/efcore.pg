using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.ValueConversion;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Storage;

public class NpgsqlValueConverterSelectorTest
{
    private readonly IValueConverterSelector _selector
        = new NpgsqlValueConverterSelector(new ValueConverterSelectorDependencies());

    [ConditionalFact]
    public void Can_get_converters_for_int_enum_arrays()
        => AssertConverters(
            _selector.Select(typeof(Queen[])).ToList(),
            (typeof(NpgsqlArrayConverter<Queen[], int[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], long[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], decimal[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], string[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], byte[][]>), default), // composite
            //(typeof(CompositeValueConverter<Queen[], int[], byte[]>), new ConverterMappingHints(size: 4)),
            (typeof(NpgsqlArrayConverter<Queen[], short[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], byte[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], ulong[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], uint[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], ushort[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], sbyte[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], double[]>), default),
            (typeof(NpgsqlArrayConverter<Queen[], float[]>), default));

    private static void AssertConverters(
        IList<ValueConverterInfo> converterInfos,
        params (Type InfoType, ConverterMappingHints Hints)[] converterTypes)
    {
        Assert.Equal(converterTypes.Length, converterInfos.Count);

        for (var i = 0; i < converterTypes.Length; i++)
        {
            var converter = converterInfos[i].Create();
            Assert.Equal(converterTypes[i].InfoType, converter.GetType());
        }
    }

    private enum Queen
    {
        Freddie,
        Brian,
        Rodger,
        John
    }
}