using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Prism.Mvvm;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{


  /// <summary>
  /// 
  /// </summary>
  public class PortalConnection : BindableBase
  {
    private string _user;

    /// <summary>
    /// 
    /// </summary>
    public string User
    {
      get => _user;
      set => SetProperty(ref _user, value);
    }

    private string _password;

    /// <summary>
    /// 
    /// </summary>
    public string Password
    {
      get => _password;
      set => SetProperty(ref _password, value);
    }

    private string _userName;

    /// <summary>
    /// 
    /// </summary>
    public string UserName
    {
      get => _userName;
      set => SetProperty(ref _userName, value);
    }

    private ImageSource _imageSource;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource UserImage
    {
      get => _imageSource;
      set => SetProperty(ref _imageSource, value);
    }

    private string _imageUserString;

    /// <summary>
    /// 
    /// </summary>
    public string UserImageString
    {
      get => _imageUserString;
      set => SetProperty(ref _imageUserString, value);
    }

    private string _baseUrl;
    /// <summary>
    /// 
    /// </summary>
    public string BaseUrl
    {
      get => _baseUrl;
      set
      {
        if(_baseUrl != value)
        {
          _baseUrl = value;
        }
        if(!string.IsNullOrEmpty(_baseUrl))
        {
          ServerRegister();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string WebMapId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private TokenCredential Credential { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private ArcGISPortal Portal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private PortalUser PortalUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private TokenAuthenticationType TokenAuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalConnection() => TokenAuthenticationType = TokenAuthenticationType.ArcGISToken;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete()]
    public async Task SingIn() => await SingInAsync();

    /// <summary>
    /// 
    /// </summary>
    public async Task SingInAsync()
    {
      await AddCredentialAsync();

      Portal = await ArcGISPortal.CreateAsync(new Uri(BaseUrl));
      var licenseInfo = await Portal.GetLicenseInfoAsync();

      ArcGISRuntimeEnvironment.SetLicense(licenseInfo);

      PortalUser = Portal.User;
      UserImage = PortalUser.ThumbnailUri != null ?
        ImageSource.FromUri(PortalUser.ThumbnailUri) :
        ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource("ic_user"));
      GetUserImageString();
      UserName = PortalUser.FullName;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SingOut()
    {
      AuthenticationManager.Current.RemoveCredential(Credential);
      Credential = null;
      UserName = string.Empty;
      UserImage = null;
      UserImageString = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete()]
    public async Task<Map> GetMap() => await GetMapAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Map> GetMapAsync()
    {
      var item = await PortalItem.CreateAsync(Portal, WebMapId);
      return new Map(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webMapId"></param>
    /// <returns></returns>
    [Obsolete()]
    public async Task<Map> GetMap(string webMapId) => await GetMapAsync(webMapId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webMapId"></param>
    /// <returns></returns>
    public async Task<Map> GetMapAsync(string webMapId)
    {
      var item = await PortalItem.CreateAsync(Portal, webMapId);
      return new Map(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsCredentialNull() => Credential == null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete()]
    public string LicenseInfoJson() => Portal.PortalInfo.LicenseInfo.ToJson();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetLicenseInfoJsonAsync()
    {
      var licenseInfo = await Portal.GetLicenseInfoAsync();
      return licenseInfo.ToJson();
    }

    /// <summary>
    /// 
    /// </summary>
    private void ServerRegister()
    {
      var serverInfo = new ServerInfo
      {
        ServerUri = new Uri(BaseUrl),
        TokenAuthenticationType = TokenAuthenticationType,
      };
      AuthenticationManager.Current.RegisterServer(serverInfo);
    }

    private void GetUserImageString()
    {
      var token = CancellationToken.None;
      Task<Stream> task = PortalUser.GetThumbnailDataAsync(token);
      Stream s = task.Result ?? GetType().Assembly.GetStreamEmbeddedResource("ic_user");

      UserImageString = s.ToBase64String();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task AddCredentialAsync()
    {
      try
      {
        var url = $"{BaseUrl}";
        Credential = await AuthenticationManager.Current.GenerateCredentialAsync(
          new Uri(url),
          User,
          Password,
          new GenerateTokenOptions
          {
            TokenAuthenticationType = TokenAuthenticationType,
            TokenValidity = 720,
          });
        AuthenticationManager.Current.AddCredential(Credential);
      }
      catch
      {
        throw;
      }
    }

  }
}
