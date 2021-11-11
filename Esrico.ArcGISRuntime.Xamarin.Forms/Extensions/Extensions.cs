using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using Xamarin.Forms;

/// <summary>
/// 
/// </summary>
namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions
{
  /// <summary>
  /// 
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static string ToDms(this double value, string axis)
    {
      string sSing = string.Empty;
      int sign = Math.Sign(value);
      value = Math.Abs(value);

      if (!string.IsNullOrEmpty(axis))
      {
        sSing = axis.ToUpper() == "X" ? sign > 0 ? "E" : "W" : sign > 0 ? "N" : "S";
        sign = 1;
      }

      double degree = Math.Floor(value);
      double minutes = Math.Floor((value - degree) * 60);
      double seconds = ((value - degree) * 60 - minutes) * 60;

      return string.Format("{0:0}°{1:00}'{2:00.000}\" {3}", sign * degree, minutes, seconds, sSing);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapPoint"></param>
    /// <returns></returns>
    public static string ToDms(this MapPoint mapPoint) => $"Y:{mapPoint.Y.ToDms("Y")}, X:{mapPoint.X.ToDms("X")}";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="otro"></param>
    /// <returns></returns>
    public static bool AreEquals(this Viewpoint vp, Viewpoint otro)
    {

      string json = vp.ToJson();
      string jsonOtro = otro != null ? otro.ToJson() : string.Empty;
      Debug.WriteLine($"Viewpoint 1:{json}");
      Debug.WriteLine($"Viewpoint 2:{jsonOtro}");

      return json == jsonOtro;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vp"></param>
    /// <param name="bounds"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public static bool AreEquals(this Viewpoint vp, Viewpoint bounds, Viewpoint center) => (vp.TargetGeometry.IsEqual(bounds.TargetGeometry) ||
        (vp.TargetGeometry.IsEqual(center.TargetGeometry) &&
          vp.TargetScale.Equals(center.TargetScale)));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageString"></param>
    /// <returns></returns>
    public static ImageSource ToImageSource(this string imageString)
    {
      byte[] bytes = Convert.FromBase64String(imageString);
      return ImageSource.FromStream(() => new MemoryStream(bytes));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToBase64String(this Stream s)
    {
      byte[] bytes = null;
      using (MemoryStream ms = new MemoryStream())
      {
        s.CopyTo(ms);
        bytes = ms.ToArray();
      }
      return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static Stream GetStreamEmbeddedResource(this Assembly assembly, string resource)
    {
      string fullName = assembly.GetManifestResourceNames().Where(s => s.Contains(resource)).FirstOrDefault();
      if (!string.IsNullOrEmpty(fullName))
      {
        return assembly.GetManifestResourceStream(fullName);
      }
      return null;
    }

  }
}
