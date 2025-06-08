using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using JetBrains.Annotations;

namespace Trailblaze.Controls.Acrylic;

/// <summary>
///
/// </summary>
[PublicAPI]
public class Acrylic : ContentControl
{
    private static readonly ImmutableExperimentalAcrylicMaterial DefaultAcrylicMaterial =
        (ImmutableExperimentalAcrylicMaterial)
            new ExperimentalAcrylicMaterial
            {
                MaterialOpacity = 0.1,
                TintColor = new Color(255, 5, 5, 5),
                TintOpacity = 1,
                PlatformTransparencyCompensationLevel = 0,
            }.ToImmutable();

    /// <summary>
    ///
    /// </summary>
    public static readonly StyledProperty<ExperimentalAcrylicMaterial?> MaterialProperty =
        AvaloniaProperty.Register<Acrylic, ExperimentalAcrylicMaterial?>(nameof(Material));

    /// <summary>
    ///
    /// </summary>
    public ExperimentalAcrylicMaterial? Material
    {
        get => GetValue(MaterialProperty);
        set => SetValue(MaterialProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly StyledProperty<int> BlurProperty = AvaloniaProperty.Register<
        Acrylic,
        int
    >(nameof(Blur));

    /// <summary>
    ///
    /// </summary>
    public int Blur
    {
        get => GetValue(BlurProperty);
        set => SetValue(BlurProperty, value);
    }

    static Acrylic()
    {
        AffectsRender<Acrylic>(MaterialProperty);
        AffectsRender<Acrylic>(BlurProperty);
    }

    /// <inheritdoc />
    public override void Render(DrawingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var material =
            Material != null
                ? (ImmutableExperimentalAcrylicMaterial)Material.ToImmutable()
                : DefaultAcrylicMaterial;
        context.Custom(
#pragma warning disable CA2000
            new AcrylicRenderOperation(this, material, Blur, new Rect(default, Bounds.Size))
#pragma warning restore CA2000
        );
    }
}
