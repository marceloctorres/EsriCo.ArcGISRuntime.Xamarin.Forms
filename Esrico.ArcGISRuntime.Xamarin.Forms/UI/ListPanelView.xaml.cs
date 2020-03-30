using System;
using System.Collections;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ListPanelView : PanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ListBindingContextProperty = BindableProperty.Create(
      nameof(ListBindingContext),
      typeof(object),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public object ListBindingContext
    {
      get => GetValue(ListBindingContextProperty);
      set => SetValue(ListBindingContextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
      nameof(ItemsSource),
      typeof(IEnumerable),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable ItemsSource
    {
      get => (IEnumerable)GetValue(ItemsSourceProperty);
      set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsGroupingEnabledProperty = BindableProperty.Create(
      nameof(IsGroupingEnabled),
      typeof(bool),
      typeof(ListPanelView),
      defaultValue: false);

    /// <summary>
    /// 
    /// </summary>
    public bool IsGroupingEnabled
    {
      get => (bool)GetValue(IsGroupingEnabledProperty);
      set => SetValue(IsGroupingEnabledProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
      nameof(ItemTemplate),
      typeof(DataTemplate),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate ItemTemplate
    {
      get => (DataTemplate)GetValue(ItemTemplateProperty);
      set => SetValue(ItemTemplateProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty HeaderTemplateProperty = BindableProperty.Create(
      nameof(HeaderTemplate),
      typeof(DataTemplate),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate)GetValue(HeaderTemplateProperty);
      set => SetValue(HeaderTemplateProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty FooterTemplateProperty = BindableProperty.Create(
      nameof(FooterTemplate),
      typeof(DataTemplate),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate FooterTemplate
    {
      get => (DataTemplate)GetValue(FooterTemplateProperty);
      set => SetValue(FooterTemplateProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty GroupHeaderTemplateProperty = BindableProperty.Create(
      nameof(GroupHeaderTemplate),
      typeof(DataTemplate),
      typeof(ListPanelView));

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate GroupHeaderTemplate
    {
      get => (DataTemplate)GetValue(GroupHeaderTemplateProperty);
      set => SetValue(GroupHeaderTemplateProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ListPanelView() => InitializeComponent();

    private void ListView_BindingContextChanged(object sender, EventArgs e) => Console.WriteLine(sender);
  }
}