using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services {
  /// <summary>
  /// 
  /// </summary>
  public interface IPortalConnection {
    /// <summary>
    /// 
    /// </summary>

    bool Active { get; set; }
    string BaseUrl { get; set; }
    TokenCredential Credential { get; }
    string Domain { get; set; }
    string Name { get; set; }
    string OrganizationName { get; set; }
    string OrganizationSubDomain { get; set; }
    string Password { get; set; }
    ArcGISPortal Portal { get; }
    PortalInfo PortalInfo { get; }
    PortalUser PortalUser { get; }
    string ServerRegisterUrl { get; }
    bool SignedIn { get; }
    TokenAuthenticationType TokenAuthenticationType { get; set; }
    AuthenticationType AuthenticationType { get; set; }
    DateTimeOffset TokenExpirationDateTime { get; set; }
    double TokenValidDays { get; set; }
    string User { get; set; }
    ImageSource UserImage { get; set; }
    string UserImageString { get; set; }
    string UserName { get; set; }
    string WebMapId { get; set; }
    Task<Credential> CreateCredentialAsync(CredentialRequestInfo info);
    PortalGroup GetGroup(string groupTitle);
    Task<string> GetLicenseInfoJsonAsync();
    Task<Map> GetMapAsync();
    Task<Map> GetMapAsync(string webMapId);
    Task<PortalItem> GetWebMapItemByGroupAndTitleAsync(PortalGroup group, string itemTitle);
    Task<List<PortalItem>> GetWebMapItemsByGroupAsync(PortalGroup group);
    bool IsCredentialNull();
    Task SignInAsync();
    void SignOut();
  }
}