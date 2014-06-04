/// The modified version of this software is Copyright (C) 2013 ZHing.
/// The original version's copyright as below.

using System.Collections.Generic;
using UnityEngine;

namespace HTMLEngine.NGUI {
  /// <summary>
  /// Provides gate between HTMLEngine and Unity3D. Implements abstract class.
  /// </summary>
  public class NGUIDevice : HtDevice {
    /// <summary>
    /// Fonts cache (to do not load every time from resouces)
    /// </summary>
    private readonly Dictionary<string, HtFont> fonts = new Dictionary<string, HtFont>();
    /// <summary>
    /// Image cache (same thing)
    /// </summary>
    private readonly Dictionary<string, HtImage> images = new Dictionary<string, HtImage>();

    /// <summary>
    /// White texture (for FillRect method)
    /// </summary>
    private static Texture2D whiteTex;

    /// <summary>
    /// Load font
    /// </summary>
    /// <param name="face">Font name</param>
    /// <param name="size">Font size</param>
    /// <param name="bold">Bold flag</param>
    /// <param name="italic">Italic flag</param>
    /// <param name="spacingX">Horizontal space</param>
    /// <param name="spacingY">Vertical space</param>
    /// <returns>Loaded font</returns>
    public override HtFont LoadFont(string face, int size, bool bold, bool italic, int spacingX, int spacingY) {
      // try get from cache
      string key = string.Format("{0}{1}{2}{3}sx{4}sy{5}", face, size, bold ? "b" : "", italic ? "i" : "", spacingX, spacingY);
      HtFont ret;
      if (fonts.TryGetValue(key, out ret)) return ret;
      // fail with cache, so create new and store into cache
      ret = new NGUIFont(face, size, bold, italic, spacingX, spacingY);
      fonts[key] = ret;
      return ret;
    }

    /// <summary>
    /// Load image
    /// </summary>
    /// <param name="src">src attribute from img tag</param>
    /// <param name="fps">fps attribute from img tag</param>
    /// <returns>Loaded image</returns>
    public override HtImage LoadImage(string src, int fps) {
      // try get from cache
      HtImage ret;
      if (images.TryGetValue(src, out ret)) return ret;
      // fail with cache, so create new and store into cache
      ret = new NGUIImage(src, fps);
      images[src] = ret;
      return ret;
    }

    /// <summary>
    /// FillRect implementation
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="color"></param>
    /// <param name="userData"></param>
    public override void FillRect(HtRect rect, HtColor color, object userData) {
      var root = userData as Transform;
      if (root != null) {
        var go = new GameObject("fill", typeof(UISprite));
        go.layer = root.gameObject.layer;
        go.transform.parent = root;
        go.transform.localPosition = new Vector3(rect.X + rect.Width / 2, -rect.Y - rect.Height / 2, 0f);
        go.transform.localScale = new Vector3(1f, 0.5f, 1f);//Vector3.one;
        var spr = go.GetComponent<UISprite>();
        spr.pivot = UIWidget.Pivot.Center;
        spr.atlas = Resources.Load("atlases/white", typeof(UIAtlas)) as UIAtlas;
        spr.spriteName = "white";
        spr.color = new Color32(color.R, color.G, color.B, color.A);
        spr.type = UISprite.Type.Sliced;
        spr.width = rect.Width != 0 ? rect.Width : 1;
        spr.height = rect.Height != 0 ? rect.Height : 1;
        spr.depth = NGUIHTML.currentDepth;
        //spr.MakePixelPerfect();
      } else {
        HtEngine.Log(HtLogLevel.Error, "Can't draw without root.");
     }
    }

    /// <summary>
    /// On device is released.
    /// </summary>
    public override void OnRelease() {
      foreach (var pair in fonts) {
        NGUIFont nguiFont = pair.Value as NGUIFont;
        if (nguiFont != null)
          nguiFont.OnRelease();
      }
    }
  }
}
