using System.Linq;
using Xamarin.Forms;

namespace App1.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.CreateAttached(
                "TintColor",
                typeof(Color),
                typeof(TintImageEffect),
                Color.Default,
                propertyChanged: OnTintColorChanged);

        public static Color GetTintColor(BindableObject view) => (Color)view.GetValue(TintColorProperty);
        public static void SetTintColor(BindableObject view, Color value) => view.SetValue(TintColorProperty, value);

        private static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Image image)
            {
                var effect = image.Effects.OfType<TintImageEffect>().FirstOrDefault();
                if (effect != null)
                {
                    image.Effects.Remove(effect);
                }
                image.Effects.Add(new TintImageEffect { TintColor = (Color)newValue });
            }
        }

        public Color TintColor { get; set; }

        public TintImageEffect() : base("App1.TintImageEffect")
        {
        }
    }
}
