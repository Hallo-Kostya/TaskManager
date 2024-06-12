using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using App1.Droid.Effects;
using App1.Effects;
using System.ComponentModel;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectAndroid), "TintImageEffect")]
namespace App1.Droid.Effects
{
    public class TintImageEffectAndroid : PlatformEffect
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
            var control = Control as ImageView;
            var color = TintImageEffect.GetTintColor(Element);

            if (control != null && color != Color.Default)
            {
                control.SetColorFilter(new PorterDuffColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn));
            }
        }
    }
}
