using System.Linq;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ModalPanelView : PanelView
  {
    private readonly string ColorKey = "DarkGrayTransparent";

    private Frame ModalFrame { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ModalPanelView()
    {
      InitializeComponent();
      CreateModalFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateModalFrame()
    {
      var resource = Resources.MergedDictionaries
        .Where(r => r.ContainsKey(ColorKey))
        .Select(r => r[ ColorKey ]).FirstOrDefault();
      Color backColor = resource != null ? (Color)resource : Color.Gray;
      ModalFrame = new Frame()
      {
        BackgroundColor = backColor,
        Padding = 0,
        HorizontalOptions = LayoutOptions.FillAndExpand,
        VerticalOptions = LayoutOptions.FillAndExpand
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(IsVisible))
      {
        if(IsVisible)
        {
          InsertModalFrame();
        }
        else
        {
          RemoveModalFrame();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void InsertModalFrame()
    {
      if(Parent is Layout<View> layout)
      {
        if(!layout.Children.Contains(ModalFrame))
        {
          var index = layout.Children.IndexOf(this);
          layout.Children.Insert(index - 1, ModalFrame);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveModalFrame()
    {
      if(Parent is Layout<View> layout)
      {
        if(layout.Children.Contains(ModalFrame))
        {
          layout.Children.Remove(ModalFrame);
        }
      }
    }
  }
}