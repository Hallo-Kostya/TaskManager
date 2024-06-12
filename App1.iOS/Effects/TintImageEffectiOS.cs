using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using App1.iOS.Effects;
using App1.Effects;
using UIKit;
using System.Linq;
using System;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectiOS), "TintImageEffect")]
namespace App1.iOS.Effects
{
    public class TintImageEffectiOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            UpdateTintColor();
        }

        protected override void OnDetached()
        {
            if (Control is UIImageView imageView)
            {
                imageView.TintColor = UIColor.Clear;
            }
        }

        private void UpdateTintColor()
        {
            if (Control is UIImageView imageView)
            {
                var color = TintImageEffect.GetTintColor(Element);
                imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                imageView.TintColor = color.ToUIColor();
            }
        }
    }

}
