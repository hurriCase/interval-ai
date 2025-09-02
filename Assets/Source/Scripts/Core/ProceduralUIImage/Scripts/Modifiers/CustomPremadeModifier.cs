/* Uncomment this to work from it as a base for your own modifier
 *
 *
namespace UnityEngine.UI.ProceduralImage
{
    [ModifierID("Your Modifier Identity here")]
    public class CustomPremadeModifier : ProceduralImageModifier
    {
        public override Vector4 CalculateRadius(Rect imageRect)
        {
            var r = Mathf.Min(imageRect.width, imageRect.height) * 0.5f;
            return new Vector4(r, r, r, 0);
        }
    }
}
*/
