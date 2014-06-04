/// The modified version of this software is Copyright (C) 2013 ZHing.
/// The original version's copyright as below.

using UnityEngine;

namespace HTMLEngine.NGUI {
  /// <summary>
  /// Provides font for use with HTMLEngine. Implements abstract class.
  /// </summary>
  public class NGUIFont : HtFont {
    /// <summary>
    /// style to draw
    /// </summary>
    public readonly GUIStyle style = new GUIStyle();
    /// <summary>
    /// content to draw
    /// </summary>
    public readonly GUIContent content = new GUIContent();
    /// <summary>
    /// Width of whitespace
    /// </summary>
    private readonly int whiteSize;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="face">Font name</param>
    /// <param name="size">Font size</param>
    /// <param name="bold">Bold flag</param>
    /// <param name="italic">Italic flag</param>
    /// <param name="spacingX">Horizontal space</param>
    /// <param name="spacingY">Vertical space</param>
    public NGUIFont(string face, int size, bool bold, bool italic, int spacingX, int spacingY)
      : base(face, size, bold, italic, spacingX, spacingY)
    {
      this.style.font = Resources.Load("fonts/" + face, typeof(Font)) as Font;
      this.style.fontSize = size;
      if (bold && italic)
        this.style.fontStyle = FontStyle.BoldAndItalic;
      else if (bold)
        this.style.fontStyle = FontStyle.Bold;
      else if (italic)
        this.style.fontStyle = FontStyle.Italic;
      else
        this.style.fontStyle = FontStyle.Normal;
      
      // showing error if font not found
      if (this.style.font == null)
        Debug.LogError("Could not load font: " + face);

      // some tuning
      this.style.wordWrap = false;
      
      // calculating white size
      NGUIText.fontSize = style.fontSize;
      NGUIText.pixelDensity = NGUIHTML.currentRoot != null ? 1f / NGUIHTML.currentRoot.pixelSizeAdjustment : 1f;
      NGUIText.fontStyle = style.fontStyle;
      NGUIText.encoding = false;
      NGUIText.spacingX = SpacingX;
      NGUIText.spacingY = SpacingY;
      NGUIText.dynamicFont = style.font;
      NGUIText.Update();
      var s1 = NGUIText.CalculatePrintedSize(" .");
      var s2 = NGUIText.CalculatePrintedSize(".");
      this.whiteSize = (int)(s1.x - s2.x);
    }
    
    /// <summary>
    /// Space between text lines in pixels
    /// </summary>
    public override int LineSpacing { get { return (int)this.style.lineHeight + SpacingY; } }
    
    /// <summary>
    /// Space between words
    /// </summary>
    public override int WhiteSize { get { return this.whiteSize; } }

    /// <summary>
    /// Measuring text width and height
    /// </summary>
    /// <param name="text">text to measure</param>
    /// <returns>width and height of measured text</returns>
    public override HtSize Measure(string text) {
      NGUIText.fontSize = style.fontSize;
      NGUIText.pixelDensity = NGUIHTML.currentRoot != null ? 1f / NGUIHTML.currentRoot.pixelSizeAdjustment : 1f;
      NGUIText.fontStyle = style.fontStyle;
      NGUIText.rectWidth = Screen.width;
      NGUIText.rectHeight = Screen.height;
      NGUIText.gradient = false;
      NGUIText.gradientTop = Color.white;
      NGUIText.gradientBottom = Color.white;
      NGUIText.encoding = false;
      NGUIText.premultiply = true;
      NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
      NGUIText.spacingX = SpacingX;
      NGUIText.spacingY = SpacingY;
      NGUIText.maxLines = 0;
      NGUIText.dynamicFont = style.font;
      NGUIText.bitmapFont = null;
      NGUIText.Update();
      var s = NGUIText.CalculatePrintedSize(text);
      return new HtSize((int)s.x, (int)s.y);
    }
    
    /// <summary>
    /// Draw method.
    /// </summary>
    /// <param name="rect">Where to draw</param>
    /// <param name="color">Text color</param>
    /// <param name="text">Text</param>
    /// <param name="isEffect">Is effect</param>
		/// <param name="effect">Effect</param>
    /// <param name="effectColor">Effect color</param>
		/// <param name="effectAmount">Effect amount</param>
		/// <param name="linkText">Link text</param>
    /// <param name="userData">User data</param>
    public override void Draw(string id, HtRect rect, HtColor color, string text, bool isEffect, Core.DrawTextEffect effect, HtColor effectColor, int effectAmount, string linkText, object userData) {
      // NGUI do not need to draw effect.
      if (isEffect) return;

      var root = userData as Transform;
      if (root != null) {
        var go = new GameObject(string.IsNullOrEmpty(id) ? "label" : id, typeof(UILabel));
        go.layer = root.gameObject.layer;
        go.transform.parent = root;
        go.transform.localPosition = new Vector3(rect.X + rect.Width / 2, -rect.Y - rect.Height / 2, 0f);
        go.transform.localScale = Vector3.one;
        var lab = go.GetComponent<UILabel>();
        lab.enabled = false;
        lab.pivot = UIWidget.Pivot.Center;
        lab.supportEncoding = false;
        lab.fontSize = style.fontSize;
        lab.fontStyle = style.fontStyle;
        lab.color = new Color32(color.R, color.G, color.B, color.A);
        lab.spacingX = SpacingX;
        lab.spacingY = SpacingY;
        switch (effect) {
        case Core.DrawTextEffect.Outline:
          lab.effectStyle = UILabel.Effect.Outline;
          break;
        case Core.DrawTextEffect.Shadow:
          lab.effectStyle = UILabel.Effect.Shadow;
          break;
        }
        lab.effectColor = new Color32(effectColor.R, effectColor.G, effectColor.B, effectColor.A);
        lab.effectDistance = new Vector2(effectAmount, effectAmount);
        if (NGUIHTML.currentLabelOverflow == UILabel.Overflow.ShrinkContent && NGUIHTML.currentLabelCrispness != UILabel.Crispness.Never) {
          lab.width = rect.Width + Mathf.Max(1, (int)(rect.Width * 0.05f));
          lab.height = rect.Height + Mathf.Max(1, (int)(rect.Height * 0.05f));
        } else {
          lab.width = 0;//rect.Width;
          lab.height = 0;//rect.Height;
        }
        lab.depth = NGUIHTML.currentDepth;
        lab.overflowMethod = NGUIHTML.currentLabelOverflow;
        lab.keepCrispWhenShrunk = NGUIHTML.currentLabelCrispness;
        lab.trueTypeFont = style.font;
        lab.enabled = true;
        lab.text = text;
        lab.MakePixelPerfect();

        // build link.
        if (!string.IsNullOrEmpty(linkText)) {
          var collider = go.AddComponent<BoxCollider>();
          collider.isTrigger = true;
          collider.center = new Vector3(0f, 0f, -0.25f);
          collider.size = new Vector3(lab.width, lab.height, 1f);

          var nguiLinkText = go.AddComponent<NGUILinkText>();
          nguiLinkText.linkText = linkText;

          var uiButtonColor = go.AddComponent<UIButtonColor>();
          uiButtonColor.tweenTarget = go;
          uiButtonColor.hover = new Color32(
            HtEngine.LinkHoverColor.R,
            HtEngine.LinkHoverColor.G,
            HtEngine.LinkHoverColor.B,
            HtEngine.LinkHoverColor.A);
          uiButtonColor.pressed = new Color(
            lab.color.r * HtEngine.LinkPressedFactor,
            lab.color.g * HtEngine.LinkPressedFactor,
            lab.color.b * HtEngine.LinkPressedFactor, lab.color.a);
          uiButtonColor.duration = 0f;

          var uiButtonMessage = go.AddComponent<UIButtonMessage>();
          uiButtonMessage.target = root.gameObject;
          uiButtonMessage.functionName = HtEngine.LinkFunctionName;
        }
      } else {
        HtEngine.Log(HtLogLevel.Error, "Can't draw without root.");
      }
    }
    
    /// <summary>
    /// on the font be released.
    /// </summary>
    public void OnRelease() {
    }
  }
}