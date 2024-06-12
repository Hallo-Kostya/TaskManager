using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using App1.iOS.Effects;
using App1.Effects;
using System.ComponentModel;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectIOS), "TintImageEffect")]
namespace App1.iOS.Effects
{
    public class TintImageEffectIOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            UpdateColor();
        }

        protected override void OnDetached()
        {
            // Optional: Cleanup code if needed
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == TintImageEffect.TintColorProperty.PropertyName)
            {
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            var control = Control as UIImageView;
            var color = TintImageEffect.GetTintColor(Element);

            if (control != null && color != Color.Default)
            {
                control.Image = control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                control.TintColor = color.ToUIColor();
            }
        }
    }
}
