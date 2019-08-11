
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class IdentifyView : ContentView
  {

    public string SvgPrevious = "left-arrow-in-circular-button.svg";
    public string SvgNext = "right-arrow-in-circular-button.svg";
    public string SvgClose = "cross-circular-button-outline.svg";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(IdentifyView));

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(IdentifyView));

    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(IdentifyView));

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
      nameof(Title),
      typeof(string),
      typeof(IdentifyView));

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBorderColor
    {
      get => (Color)GetValue(TitleBorderColorProperty);
      set => SetValue(TitleBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBackgroundColor
    {
      get => (Color)GetValue(TitleBackgroundColorProperty);
      set => SetValue(TitleBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public string Title
    {
      get => (string)GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyView()
    {
      InitializeComponent();
    }
  }
}