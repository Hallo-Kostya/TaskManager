using Xamarin.Forms;

namespace App1.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public TintImageEffect() : base("App1.TintImageEffect")
        {

        }

        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintImageEffect), Color.Default);

        public Color TintColor { get; set; }

    }
}
