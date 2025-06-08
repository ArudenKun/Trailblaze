using Avalonia;
using Avalonia.Controls;

namespace Trailblaze.Controls;

public partial class IfPresenter : UserControl
{
    public static readonly StyledProperty<bool> ConditionProperty = AvaloniaProperty.Register<
        IfPresenter,
        bool
    >(nameof(Condition));

    public static readonly StyledProperty<Control> TrueProperty = AvaloniaProperty.Register<
        IfPresenter,
        Control
    >(nameof(True));

    public static readonly StyledProperty<Control> FalseProperty = AvaloniaProperty.Register<
        IfPresenter,
        Control
    >(nameof(False));

    public IfPresenter()
    {
        InitializeComponent();
    }

    public bool Condition
    {
        get => GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    public Control True
    {
        get => GetValue(TrueProperty);
        set => SetValue(TrueProperty, value);
    }

    public Control False
    {
        get => GetValue(FalseProperty);
        set => SetValue(FalseProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        switch (change.Property.Name)
        {
            case nameof(Condition):
            case nameof(True):
            case nameof(False):
                UpdateContent();
                break;
        }

        base.OnPropertyChanged(change);
    }

    private void UpdateContent()
    {
        Content = Condition ? True : False;
    }
}
