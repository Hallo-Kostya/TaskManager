using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using App1.Droid.Effects;
using App1.Effects;


[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectAndroid), "TintImageEffect")]
namespace App1.Droid.Effects
{
    public class TintImageEffectAndroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            UpdateTintColor();
        }

        protected override void OnDetached()
        {
            if (Control is Android.Widget.ImageView imageView)
            {
                imageView.ClearColorFilter();
            }
        }

        private void UpdateTintColor()
        {
            if (Control is Android.Widget.ImageView imageView)
            {
                var color = TintImageEffect.GetTintColor(Element);
                imageView.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
        }
    }
}


