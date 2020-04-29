using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private bool _active;

    /// <summary>
    /// 
    /// </summary>
    public bool Active
    {
      get => _active;
      set => SetProperty(ref _active, value);
    }

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

    private string _organizationName; 

    /// <summary>
    /// 
    /// </summary>
    public string OrganizationName
    {
      get => _organizationName;
      set => SetProperty(ref _organizationName, value);
    }

    private string _organizationSubDomain;

    /// <summary>
    /// 
    /// </summary>
    public string OrganizationSubDomain 
    { 
      get => _organizationSubDomain; 
      set => SetProperty(ref _organizationSubDomain, value);
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
          if(!string.IsNullOrEmpty(_baseUrl))
          {
            ServerRegisterUrl = ResetSharingUrl(_baseUrl);
            ServerRegister();
          }
        }
      }
    }

    private double _tokenValidDays;

    /// <summary>
    /// 
    /// </summary>
    public double TokenValidDays
    {
      get => _tokenValidDays;
      set => SetProperty(ref _tokenValidDays, value);
    }

    private DateTimeOffset _tokenExpirationDateTime;

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset TokenExpirationDateTime { 
      get => _tokenExpirationDateTime; 
      set => SetProperty(ref _tokenExpirationDateTime, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public string WebMapId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private string ServerRegisterUrl { get; set; }

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
    private PortalInfo PortalInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private TokenAuthenticationType TokenAuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalConnection()
    {
      TokenAuthenticationType = TokenAuthenticationType.ArcGISToken;
      TokenValidDays = 30;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete()]
    public async Task SingIn() => await SingInAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    private string ResetSharingUrl(string baseUrl)
    {
      var uriBuilder = new UriBuilder(baseUrl);
      uriBuilder.Path = !baseUrl.Contains("/sharing/rest") ?
        string.Concat(uriBuilder.Path, uriBuilder.Path.EndsWith("/") ? 
          "sharing/rest" : 
          "/sharing/rest") :
        uriBuilder.Path;
      return uriBuilder.Uri.AbsoluteUri;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string SubDomainUrl()
    {
      var uriBuilder = new UriBuilder(BaseUrl);
      uriBuilder.Host = !string.IsNullOrEmpty(PortalInfo.OrganizationSubdomain) ?
        string.Concat(PortalInfo.OrganizationSubdomain, ".", PortalInfo.CustomBaseDomain) :
        uriBuilder.Host;

      return uriBuilder.Uri.AbsoluteUri;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task SingInAsync()
    {
      try
      {
        await AddCredentialAsync();
        Portal = await ArcGISPortal.CreateAsync(new Uri(BaseUrl));
        var licenseInfo = await Portal.GetLicenseInfoAsync();

        ArcGISRuntimeEnvironment.SetLicense(licenseInfo);
        PortalInfo = Portal.PortalInfo;
        PortalUser = Portal.User;

        UserImage = PortalUser.ThumbnailUri != null ?
          ImageSource.FromUri(PortalUser.ThumbnailUri) :
          ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource("ic_user"));
        GetUserImageString();
        UserName = PortalUser.FullName;
        OrganizationName = PortalInfo.OrganizationName;
        OrganizationSubDomain = SubDomainUrl();
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex);
      }
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
        ServerUri = new Uri(ServerRegisterUrl),
        TokenAuthenticationType = TokenAuthenticationType,
      };
      AuthenticationManager.Current.RegisterServer(serverInfo);
    }

    /// <summary>
    /// 
    /// </summary>
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
        var uri = new Uri($"{ServerRegisterUrl}");
        var now = DateTime.Now;

        Credential = await AuthenticationManager.Current.GenerateCredentialAsync(
          uri,
          User,
          Password,
          new GenerateTokenOptions
          {
            TokenAuthenticationType = TokenAuthenticationType,
            TokenValidity = (int)(TokenValidDays * 24 * 60),
          });
        AuthenticationManager.Current.AddCredential(Credential);
        TokenExpirationDateTime = Credential.ExpirationDate.Value;
        TokenValidDays = (TokenExpirationDateTime - now).TotalDays;
      }
      catch
      {
        throw;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupTitle"></param>
    /// <returns></returns>
    public PortalGroup GetGroupAsync(string groupTitle)
    {
      if(PortalUser != null)
      {
        var groups = PortalUser.Groups;
        return groups.Where(g => g.Title == groupTitle).FirstOrDefault();
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public async Task<List<PortalItem>> GetWebMapItemsByGroupAsync(PortalGroup group)
    {
      var portalQueryParams = new PortalQueryParameters($"type:\"web map\" group:{group.GroupId}")
      {
        SortField = "Title",
        SortOrder = PortalQuerySortOrder.Ascending,
      };
      var results = await Portal.FindItemsAsync(portalQueryParams);
      return results.Results.ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="itemTitle"></param>
    /// <returns></returns>
    public async Task<PortalItem> GetWebMapItemByGroupAndTitleAsync(PortalGroup group, string itemTitle)
    {
      var webMapItems = await GetWebMapItemsByGroupAsync(group);
      if(webMapItems != null)
      {
        return webMapItems.Where(i => i.Title == itemTitle).FirstOrDefault();
      }
      return null;
    }
  }
}
