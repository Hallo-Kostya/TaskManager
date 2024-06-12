using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using App1.Droid.Effects;
using App1.Effects;
using System.Linq;
using System;

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
                    var effect = (TintImageEffect)Element.Effects.FirstOrDefault(e => e is TintImageEffect);
                    if (effect != null)
                    {
                        imageView.SetColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
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
            if (Control is ImageView imageView)
            {
                imageView.ClearColorFilter();
            }
        }
    }
}
