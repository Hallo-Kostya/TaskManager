using Xamarin.Forms;

namespace App1.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public Color TintColor { get; set; }

        public TintImageEffect() : base("App1.TintImageEffect")
        {
        }
    }
}
