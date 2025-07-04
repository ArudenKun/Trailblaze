﻿// using Avalonia;
// using Avalonia.Markup.Xaml;
//
// namespace Trailblaze.Localization;
//
// public class LocalizeExtension(string key) : MarkupExtension, IObservable<string>, IDisposable
// {
//     public override object ProvideValue(IServiceProvider serviceProvider)
//     {
//         return this.ToBinding();
//     }
//
//     private IObserver<string>? _observer;
//
//     public IDisposable Subscribe(IObserver<string> observer)
//     {
//         _observer = observer;
//         _observer.OnNext(Localizer.Get(key));
//         Localizer.LanguageChanged += OnLanguageChanged;
//
//         return this;
//     }
//
//     private void OnLanguageChanged(object? sender, EventArgs e)
//     {
//         _observer?.OnNext(Localizer.Get(key));
//     }
//
//     public void Dispose()
//     {
//         Localizer.LanguageChanged -= OnLanguageChanged;
//         _observer = null;
//
//         GC.SuppressFinalize(this);
//     }
// }
