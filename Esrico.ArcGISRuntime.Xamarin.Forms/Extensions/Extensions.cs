﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions
{
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
      var sSing = string.Empty;
      var sign = Math.Sign(value);
      value = Math.Abs(value);

      if (!string.IsNullOrEmpty(axis))
      {
        sSing = axis.ToUpper() == "X" ? sign > 0 ? "E" : "W" : sign > 0 ? "N" : "S";
        sign = 1;
      }

      var degree = Math.Floor(value);
      var minutes = Math.Floor((value - degree) * 60);
      var seconds = ((value - degree) * 60 - minutes) * 60;

      return string.Format("{0:0}°{1:00}'{2:00.000}\" {3}", sign * degree, minutes, seconds, sSing);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapPoint"></param>
    /// <returns></returns>
    public static string ToDms(this MapPoint mapPoint)
    {
      return $"Y:{mapPoint.Y.ToDms("Y")}, X:{mapPoint.X.ToDms("X")}";
    }

    public static bool AreEquals(this Viewpoint vp, Viewpoint otro)
    {

      var json = vp.ToJson();
      var jsonOtro = otro != null ? otro.ToJson() : string.Empty;
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
    public static bool AreEquals(this Viewpoint vp, Viewpoint bounds, Viewpoint center)
    {
      return (vp.TargetGeometry.IsEqual(bounds.TargetGeometry) ||
        (vp.TargetGeometry.IsEqual(center.TargetGeometry) &&
          vp.TargetScale.Equals(center.TargetScale)));
    }


  }
}
