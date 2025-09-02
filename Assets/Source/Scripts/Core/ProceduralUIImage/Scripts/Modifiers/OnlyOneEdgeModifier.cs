namespace UnityEngine.UI.ProceduralImage
{
    [ModifierID("Only One Edge")]
    public class OnlyOneEdgeModifier : ProceduralImageModifier
    {
        [SerializeField] private float radius;
        [SerializeField] private ProceduralImageEdge side;

        public enum ProceduralImageEdge
        {
            Top,
            Bottom,
            Left,
            Right
        }

        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Graphic.SetVerticesDirty();
            }
        }

        public ProceduralImageEdge Side
        {
            get => side;
            set => side = value;
        }

        public override Vector4 CalculateRadius(Rect imageRect) => side switch
        {
            ProceduralImageEdge.Top => new Vector4(radius, radius, 0, 0),
            ProceduralImageEdge.Right => new Vector4(0, radius, radius, 0),
            ProceduralImageEdge.Bottom => new Vector4(0, 0, radius, radius),
            ProceduralImageEdge.Left => new Vector4(radius, 0, 0, radius),
            _ => new Vector4(0, 0, 0, 0)
        };
    }
}