using System;
using Avalonia.Data.Converters;

namespace ERPMateus.Shared.Converters;

public sealed class NullToTrueConverter : IValueConverter
{
    public static readonly NullToTrueConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => value is null;

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotSupportedException();
}