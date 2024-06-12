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
            try
            {
                if (Control is UIImageView imageView)
                {
                    var effect = (TintImageEffect)Element.Effects.FirstOrDefault(e => e is TintImageEffect);
                    if (effect != null)
                    {
                        imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                        imageView.TintColor = effect.TintColor.ToUIColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            if (Control is UIImageView imageView)
            {
                imageView.TintColor = UIColor.Clear;
            }
        }
    }
}
