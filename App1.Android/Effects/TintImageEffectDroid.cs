using Android.Graphics;
using Android.Widget;
using App1.Droid.Effects;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectDroid), "TintImageEffect")]
namespace App1.Droid.Effects
{
    public class TintImageEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                UpdateColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            if (Control is ImageView imageView)
            {
                imageView.ClearColorFilter();
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == App1.Effects.TintImageEffect.TintColorProperty.PropertyName)
            {
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            if (Control is ImageView imageView)
            {
                var effect = (App1.Effects.TintImageEffect)Element.Effects.FirstOrDefault(e => e is App1.Effects.TintImageEffect);

                if (effect != null && effect.TintColor != Xamarin.Forms.Color.Default)
                {
                    var colorFilter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                    imageView.SetColorFilter(colorFilter);
                }
            }
        }
    }
}
