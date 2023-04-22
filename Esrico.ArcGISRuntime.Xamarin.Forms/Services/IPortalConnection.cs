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

    /// <summary>
    /// 
    /// </summary>
    AuthenticationType AuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string BaseUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    TokenCredential Credential { get; }

    /// <summary>
    /// 
    /// </summary>
    Func<CredentialRequestInfo, Task<Credential>> ChallengeHandlerAsync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string Domain { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    string OrganizationName { get; }

    /// <summary>
    /// 
    /// </summary>
    string OrganizationSubDomain { get; }

    /// <summary>
    /// 
    /// </summary>
    string Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    ArcGISPortal Portal { get; }

    /// <summary>
    /// 
    /// </summary>
    PortalInfo PortalInfo { get; }

    /// <summary>
    /// 
    /// </summary>
    PortalUser PortalUser { get; }

    /// <summary>
    /// 
    /// </summary>
    string ServerRegisterUrl { get; }

    /// <summary>
    /// 
    /// </summary>
    bool SignedIn { get; }

    /// <summary>
    /// 
    /// </summary>
    TokenAuthenticationType TokenAuthenticationType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    DateTimeOffset TokenExpirationDateTime { get; }

    /// <summary>
    /// 
    /// </summary>
    double TokenValidDays { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    ImageSource UserImage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string UserImageString { get; }

    /// <summary>
    /// 
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 
    /// </summary>
    string WebMapId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    Task<Credential> CreateCredentialAsync(CredentialRequestInfo info);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupTitle"></param>
    /// <returns></returns>
    PortalGroup GetGroup(string groupTitle);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<string> GetLicenseInfoJsonAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Map> GetMapAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webMapId"></param>
    /// <returns></returns>
    Task<Map> GetMapAsync(string webMapId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="itemTitle"></param>
    /// <returns></returns>
    Task<PortalItem> GetWebMapItemByGroupAndTitleAsync(PortalGroup group, string itemTitle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<List<PortalItem>> GetWebMapItemsByGroupAsync(PortalGroup group);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool IsCredentialNull();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SignInAsync();

    /// <summary>
    /// 
    /// </summary>
    void SignOut();
  }
}