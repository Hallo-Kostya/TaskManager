using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using App1.iOS.Effects;
using App1.Effects;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectiOS), "TintImageEffect")]
namespace App1.iOS.Effects
{
    public class TintImageEffectiOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control is UIImageView imageView)
                {
                    var effect = (TintImageEffect)Element.Effects.FirstOrDefault(e => e is TintImageEffect);
                    if (effect != null)
                    {
                        var color = effect.TintColor.ToUIColor();
                        imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                        imageView.TintColor = color;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cannot set property on attached control. Error: {ex.Message}");
            }
        }

        protected override void OnDetached()
        {
            // Cleanup code if necessary
        }
    }
}
