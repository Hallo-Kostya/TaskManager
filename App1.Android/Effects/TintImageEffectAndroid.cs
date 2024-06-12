using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using App1.Droid.Effects;
using App1.Effects;
using System.Linq;
using System;
using Color = Xamarin.Forms.Color;

[assembly: ResolutionGroupName("App1")]
[assembly: ExportEffect(typeof(TintImageEffectAndroid), "TintImageEffect")]
namespace App1.Droid.Effects
{
    public class TintImageEffectAndroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control is ImageView imageView)
                {
                    var effect = (App1.Effects.TintImageEffect)Element.Effects.FirstOrDefault(e => e is App1.Effects.TintImageEffect);

                    if (effect != null && effect.TintColor != Color.Default)
                    {
                        var colorFilter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                        imageView.SetColorFilter(colorFilter);
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
            // Очистка
        }
    }
}

