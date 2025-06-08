using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Trailblaze.Models;

public partial class ValueDescription : ObservableObject, IEquatable<ValueDescription>
{
    [ObservableProperty]
    public partial object? Value { get; set; }

    [ObservableProperty]
    public partial string? Description { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public string ValueAsString
    {
        get => Value?.ToString() ?? string.Empty;
        set => Value = value;
    }

    public ValueDescription() { }

    public ValueDescription(object value, string? description = null)
    {
        Value = value;
        Description = description;
    }

    public ValueDescription(object value, object? description = null)
    {
        Value = value;
        Description = description?.ToString();
    }

    public bool Equals(ValueDescription? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(Value, other.Value) && Description == other.Description;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((ValueDescription)obj);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Description);
    }

    public override string? ToString()
    {
        return Description;
    }
}
