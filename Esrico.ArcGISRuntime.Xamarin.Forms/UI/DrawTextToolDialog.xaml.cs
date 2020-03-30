using System;
using System.Windows.Input;

using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawTextToolDialog : ModalPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public DrawTextToolDialog()
    {
      InitializeComponent();
      IsVisible = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public ICommand AcceptCommand { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Text { get; set; }

    private void OKButton_Clicked(object sender, EventArgs e)
    {
      IsVisible = false;
      if(AcceptCommand != null)
      {
        if(AcceptCommand.CanExecute(null))
        {
          AcceptCommand.Execute(Text);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseButton_Clicked(object sender, EventArgs e) => IsVisible = false;
  }
}