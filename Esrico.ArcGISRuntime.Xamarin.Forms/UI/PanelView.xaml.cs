using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PanelView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CloseButtonImageProperty = BindableProperty.Create(
      nameof(CloseButtonImage),
      typeof(ImageSource),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public ImageSource CloseButtonImage
    {
      get => (ImageSource)GetValue(CloseButtonImageProperty);
      set => SetValue(CloseButtonImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CloseButtonLabelProperty = BindableProperty.Create(
      nameof(CloseButtonLabel),
      typeof(string),
      typeof(PanelView),
      defaultValue: "Close");

    /// <summary>
    /// 
    /// </summary>
    public string CloseButtonLabel
    {
      get => (string)GetValue(CloseButtonLabelProperty);
      set => SetValue(CloseButtonLabelProperty, value);
    }


    /// <summary>
    /// 
    /// </summary>
    public new static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
      nameof(IsVisible),
      typeof(bool),
      typeof(PanelView),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanged: OnIsVisibleChanged,
      defaultValue: false);

    /// <summary>
    /// 
    /// </summary>
    public new bool IsVisible
    {
      get => (bool)GetValue(IsVisibleProperty);
      set => SetValue(IsVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var panelView = bindable as PanelView;
      bool newVisible = (bool)newValue;
      bool oldVisible = (bool)oldValue;
      if (newVisible != oldVisible)
      {
        panelView.SetVisible(newVisible);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visible"></param>
    private void SetVisible(bool visible)
    {
      base.IsVisible = visible;
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(
      nameof(HeaderContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnHeaderContentChanged);

    /// <summary>
    /// 
    /// </summary>
    public View HeaderContent
    {
      get => (View)GetValue(HeaderContentProperty);
      set => SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnHeaderContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if (!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.HeaderContentView.Content = newView;
      }
      else
      {
        Console.WriteLine(bindable);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty FooterContentProperty = BindableProperty.Create(
      nameof(FooterContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnFooterContentChanged);

    /// <summary>
    /// 
    /// </summary>
    public View FooterContent
    {
      get => (View)GetValue(FooterContentProperty);
      set => SetValue(FooterContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnFooterContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if (!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.FooterContentView.Content = newView;
      }
      else
      {
        Console.WriteLine(bindable);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyContentProperty = BindableProperty.Create(
      nameof(BodyContent),
      typeof(View),
      typeof(PanelView),
      propertyChanged: OnBodyContentPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public View BodyContent
    {
      get => (View)GetValue(BodyContentProperty);
      set => SetValue(BodyContentProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnBodyContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      if (!ReferenceEquals(newValue, bindable))
      {
        var panelView = (PanelView)bindable;
        var newView = (View)newValue;

        panelView.BodyContentView.Content = newView;
      }
      else
      {
        Console.WriteLine(bindable);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.Black);

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
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.White);

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
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.Black);

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
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
      nameof(TitleText),
      typeof(string),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public string TitleText
    {
      get => (string)GetValue(TitleTextProperty);
      set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(
      nameof(IsTitleVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsTitleVisible
    {
      get => (bool)GetValue(IsTitleVisibleProperty);
      set => SetValue(IsTitleVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyBorderColorProperty = BindableProperty.Create(
      nameof(BodyBorderColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public Color BodyBorderColor
    {
      get => (Color)GetValue(BodyBorderColorProperty);
      set => SetValue(BodyBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty BodyBackgroundColorProperty = BindableProperty.Create(
      nameof(BodyBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    public Color BodyBackgroundColor
    {
      get => (Color)GetValue(BodyBackgroundColorProperty);
      set => SetValue(BodyBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusBackgroundColorProperty = BindableProperty.Create(
      nameof(StatusBackgroundColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    public Color StatusBackgroundColor
    {
      get => (Color)GetValue(StatusBackgroundColorProperty);
      set => SetValue(StatusBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusBorderColorProperty = BindableProperty.Create(
      nameof(StatusBorderColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public Color StatusBorderColor
    {
      get => (Color)GetValue(StatusBorderColorProperty);
      set => SetValue(StatusBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusTextColorProperty = BindableProperty.Create(
      nameof(StatusTextColor),
      typeof(Color),
      typeof(PanelView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public Color StatusTextColor
    {
      get => (Color)GetValue(StatusTextColorProperty);
      set => SetValue(StatusTextColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty StatusTextProperty = BindableProperty.Create(
      nameof(StatusText),
      typeof(string),
      typeof(PanelView));

    /// <summary>
    /// 
    /// </summary>
    public string StatusText
    {
      get => (string)GetValue(StatusTextProperty);
      set => SetValue(StatusTextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsStatusVisibleProperty = BindableProperty.Create(
      nameof(IsStatusVisible),
      typeof(bool),
      typeof(PanelView),
      defaultValue: true);

    /// <summary>
    /// 
    /// </summary>
    public bool IsStatusVisible
    {
      get => (bool)GetValue(IsStatusVisibleProperty);
      set => SetValue(IsStatusVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public PanelView()
    {
      InitializeComponent();
      base.IsVisible = false;
      CloseButtonImage = ImageSource.FromStream(() => this.GetType().Assembly.GetStreamEmbeddedResource(@"ic_close"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
      IsVisible = false;
    }
  }
}