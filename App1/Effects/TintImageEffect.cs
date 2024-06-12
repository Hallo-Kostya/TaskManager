using Xamarin.Forms;

namespace App1.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public TintImageEffect() : base("App1.TintImageEffect")
        {
        }

        public static readonly BindableProperty TintColorProperty = BindableProperty.CreateAttached(
            "TintColor",
            typeof(Color),
            typeof(TintImageEffect),
            Color.Default);

        public static Color GetTintColor(BindableObject view)
        {
            return (Color)view.GetValue(TintColorProperty);
        }

        public static void SetTintColor(BindableObject view, Color value)
        {
            view.SetValue(TintColorProperty, value);
        }
    }
}
