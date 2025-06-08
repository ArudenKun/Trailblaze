﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Trailblaze.Common;

public abstract class JsonFile
{
    private readonly string _filePath;

    private readonly JsonTypeInfo _rootTypeInfo;
    private readonly ReadOnlyDictionary<JsonPropertyInfo, object?> _rootPropertyDefaults;

    /// <summary>
    /// Initializes an instance of <see cref="JsonFile" />.
    /// </summary>
    /// <remarks>
    /// If you are relying on compile-time serialization, the <paramref name="jsonOptions" /> instance
    /// must have a valid <see cref="JsonSerializerOptions.TypeInfoResolver"/> set.
    /// </remarks>
    protected JsonFile(string filePath, JsonSerializerOptions jsonOptions)
    {
        _filePath = filePath;

        _rootTypeInfo = jsonOptions.GetTypeInfo(GetType());
        _rootPropertyDefaults = _rootTypeInfo
            .Properties.ToDictionary(p => p, p => p.Get?.Invoke(this))
            .AsReadOnly();
    }

    /// <summary>
    /// Initializes an instance of <see cref="JsonFile" />.
    /// </summary>
    protected JsonFile(string filePath, IJsonTypeInfoResolver jsonTypeInfoResolver)
        : this(
            filePath,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = jsonTypeInfoResolver,
            }
        ) { }

    /// <summary>
    /// Initializes an instance of <see cref="JsonFile" />.
    /// </summary>
    [RequiresUnreferencedCode(
        "This constructor initializes the settings manager with reflection-based serialization, which is incompatible with assembly trimming."
    )]
    [RequiresDynamicCode(
        "This constructor initializes the settings manager with reflection-based serialization, which is incompatible with ahead-of-time compilation."
    )]
    protected JsonFile(string filePath)
        : this(filePath, new DefaultJsonTypeInfoResolver()) { }

    /// <summary>
    /// Resets the settings to their default values.
    /// </summary>
    public virtual void Reset()
    {
        foreach (var property in _rootTypeInfo.Properties)
            property.Set?.Invoke(this, _rootPropertyDefaults[property]);
    }

    /// <summary>
    /// Saves the settings to file.
    /// </summary>
    public virtual void Save()
    {
        // Write to memory first, so that we don't end up producing a corrupted file in case of an error
        var data = JsonSerializer.SerializeToUtf8Bytes(this, _rootTypeInfo);

        var dirPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dirPath))
            Directory.CreateDirectory(dirPath);

        File.WriteAllBytes(_filePath, data);
    }

    /// <summary>
    /// Loads the settings from file.
    /// Returns true if the file was loaded, false if it didn't exist.
    /// </summary>
    public virtual bool Load()
    {
        try
        {
            using var stream = File.OpenRead(_filePath);
            using var document = JsonDocument.Parse(
                stream,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip,
                }
            );

            // This mess is required because System.Text.Json cannot populate an existing object.
            // We also can't deserialize into a new object and then copy the properties over,
            // because the target type may not have a parameterless or otherwise accessible constructor.
            // https://github.com/dotnet/runtime/issues/92877
            foreach (var jsonProperty in document.RootElement.EnumerateObject())
            {
                var property = _rootTypeInfo.Properties.FirstOrDefault(p =>
                    string.Equals(p.Name, jsonProperty.Name, StringComparison.Ordinal)
                );

                if (property is null)
                    continue;

                // HACK: Use custom converter specified on the property.
                // This will also apply the converter to any other nested properties of the same type,
                // but unfortunately there's no way to avoid that for now.
                var propertyOptions = new JsonSerializerOptions(property.Options);
                if (property.CustomConverter is not null)
                    propertyOptions.Converters.Add(property.CustomConverter);

                property.Set?.Invoke(
                    this,
                    jsonProperty.Value.Deserialize(
                        propertyOptions.GetTypeInfo(property.PropertyType)
                    )
                );
            }

            return true;
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes the settings file.
    /// Returns true if the file was deleted, false if it didn't exist.
    /// </summary>
    public virtual bool Delete()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                // This doesn't throw if the file doesn't exist, but
                // does throw if the directory doesn't exist.
                File.Delete(_filePath);
            }
            else
            {
                return false;
            }

            return true;
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return false;
        }
    }
}
