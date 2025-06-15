using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Collections;
using Trailblaze.Converters;
using ZLinq;

namespace Trailblaze.Controls.Switch;

public class SwitchCases : AvaloniaList<SwitchCase>
{
    [RequiresUnreferencedCode(
        "Conversion methods are required for type conversion, including op_Implicit, op_Explicit, Parse and TypeConverter."
    )]
    internal SwitchCase? EvaluateCases(object? value, Type targetType)
    {
        if (Count == 0)
            // If we have no cases, then we can't match anything.
            return null;

        return this.AsValueEnumerable()
                .FirstOrDefault(@case =>
                    RelayConverter.CompareValues(value, @case.Value, targetType)
                )
            // ?? this.AsValueEnumerable().FirstOrDefault(x => x.IsDefault)
            ?? this.AsValueEnumerable().FirstOrDefault(x => x.Value == AvaloniaProperty.UnsetValue);
    }
}
