namespace UnityEngine.UI.ProceduralImage
{
    [ExecuteInEditMode]
    [AddComponentMenu("UI/Procedural Image")]
    public class ProceduralImage : Image
    {
        [SerializeField] private float borderRatio;
        [SerializeField] private float falloffDistance = 1;

        private static Material DefaultProceduralImageMaterial
        {
            get
            {
                if (_materialInstance == null)
                    _materialInstance = new Material(Shader.Find("UI/Procedural UI Image"));

                return _materialInstance;
            }
            set => _materialInstance = value;
        }

        private static Material _materialInstance;

        public float BorderRatio
        {
            get => borderRatio;
            set
            {
                borderRatio = value;
                SetVerticesDirty();
            }
        }

        public float FalloffDistance
        {
            get => falloffDistance;
            set
            {
                falloffDistance = value;
                SetVerticesDirty();
            }
        }

        protected ProceduralImageModifier Modifier
        {
            get
            {
                if (_modifier == null)
                {
                    _modifier = GetComponent<ProceduralImageModifier>();

                    if (_modifier == null)
                        ModifierType = typeof(FreeModifier);
                }

                return _modifier;
            }
            set => _modifier = value;
        }

        /// <summary>
        /// Gets or sets the type of the modifier. Adds a modifier of that type.
        /// </summary>
        /// <value>The type of the modifier.</value>
        public System.Type ModifierType
        {
            get => Modifier.GetType();
            set
            {
                if (_modifier != null && _modifier.GetType() != value)
                {
                    if (GetComponent<ProceduralImageModifier>() != null)
                        DestroyImmediate(GetComponent<ProceduralImageModifier>());

                    gameObject.AddComponent(value);
                    Modifier = GetComponent<ProceduralImageModifier>();
                    SetAllDirty();
                }
                else if (_modifier == null)
                {
                    gameObject.AddComponent(value);
                    Modifier = GetComponent<ProceduralImageModifier>();
                    SetAllDirty();
                }
            }
        }

        private ProceduralImageModifier _modifier;

        protected override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_OnDirtyVertsCallback -= OnVerticesDirty;
        }

        private void Init()
        {
            FixTexCoordsInCanvas();
            m_OnDirtyVertsCallback += OnVerticesDirty;
            preserveAspect = false;
            material = null;

            if (sprite == null)
                sprite = EmptySprite.GetSprite();
        }

        protected void OnVerticesDirty()
        {
            if (sprite == null)
                sprite = EmptySprite.GetSprite();
        }

        protected void FixTexCoordsInCanvas()
        {
            var c = GetComponentInParent<Canvas>();
            if (c != null)
                FixTexCoordsInCanvas(c);
        }

        protected void FixTexCoordsInCanvas(Canvas c)
        {
            c.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 |
                                          AdditionalCanvasShaderChannels.TexCoord2 |
                                          AdditionalCanvasShaderChannels.TexCoord3;
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (Application.isPlaying is false)
                UpdateGeometry();
        }
#endif

        /// <summary>
        /// Prevents radius to get bigger than rect size
        /// </summary>
        /// <returns>The fixed radius.</returns>
        /// <param name="vec">border-radius as Vector4 (starting upper-left, clockwise)</param>
        private Vector4 FixRadius(Vector4 vec)
        {
            var r = rectTransform.rect;
            vec = new Vector4(Mathf.Max(vec.x, 0), Mathf.Max(vec.y, 0), Mathf.Max(vec.z, 0), Mathf.Max(vec.w, 0));

            //Allocates mem
            //float scaleFactor = Mathf.Min(r.width / (vec.x + vec.y), r.width / (vec.z + vec.w), r.height / (vec.x + vec.w), r.height / (vec.z + vec.y), 1);

            //Allocation free:
            var scaleFactor =
                Mathf.Min
                (
                    Mathf.Min
                    (
                        Mathf.Min
                        (
                            Mathf.Min
                            (
                                r.width / (vec.x + vec.y),
                                r.width / (vec.z + vec.w)
                            ),
                            r.height / (vec.x + vec.w)
                        ),
                        r.height / (vec.z + vec.y)),
                    1f
                );
            return vec * scaleFactor;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            EncodeAllInfoIntoVertices(toFill, CalculateInfo());
        }

        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();
            FixTexCoordsInCanvas();
        }

        private ProceduralImageInfo CalculateInfo()
        {
            var r = GetPixelAdjustedRect();
            var pixelSize = 1f / Mathf.Max(0, falloffDistance);

            var radius = FixRadius(Modifier.CalculateRadius(r));

            var minSide = Mathf.Min(r.width, r.height);

            var info = new ProceduralImageInfo(r.width + falloffDistance, r.height + falloffDistance,
                falloffDistance, pixelSize, radius / minSide, borderRatio);

            return info;
        }

        private void EncodeAllInfoIntoVertices(VertexHelper vh, ProceduralImageInfo info)
        {
            var vert = new UIVertex();

            var uv1 = new Vector2(info.Width, info.Height);
            var uv2 = new Vector2(EncodeFloats_0_1_16_16(info.Radius.x, info.Radius.y),
                EncodeFloats_0_1_16_16(info.Radius.z, info.Radius.w));
            var uv3 = new Vector2(info.BorderWidth == 0 ? 1 : Mathf.Clamp01(info.BorderWidth), info.PixelSize);

            for (var i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vert, i);

                vert.position += ((Vector3)vert.uv0 - new Vector3(0.5f, 0.5f)) * info.FallOffDistance;

                vert.uv1 = uv1;
                vert.uv2 = uv2;
                vert.uv3 = uv3;

                vh.SetUIVertex(vert, i);
            }
        }

        /// <summary>
        /// Encode two values between [0,1] into a single float. Each using 16 bits.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private float EncodeFloats_0_1_16_16(float a, float b)
        {
            var kDecodeDot = new Vector2(1.0f, 1f / 65535.0f);
            return Vector2.Dot(
                new Vector2(Mathf.Floor(a * 65534) / 65535f,
                    Mathf.Floor(b * 65534) / 65535f),
                kDecodeDot);
        }

        public override Material material
        {
            get => m_Material == null ? DefaultProceduralImageMaterial : base.material;
            set => base.material = value;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            OnEnable();
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();

            //Don't allow negative numbers for fall off distance
            falloffDistance = Mathf.Max(0, falloffDistance);

            //Don't allow negative numbers for fall off distance
            borderRatio = Mathf.Max(0, borderRatio);
        }
#endif
    }

    /// <summary>
    /// Contains all parameters of a procedural image
    /// </summary>
    public struct ProceduralImageInfo
    {
        public float Width { get; }
        public float Height { get; }
        public float FallOffDistance { get; }
        public Vector4 Radius { get; }
        public float BorderWidth { get; }
        public float PixelSize { get; }

        public ProceduralImageInfo(
            float width,
            float height,
            float fallOffDistance,
            float pixelSize,
            Vector4 radius,
            float borderWidth)
        {
            Width = Mathf.Abs(width);
            Height = Mathf.Abs(height);
            FallOffDistance = Mathf.Max(0, fallOffDistance);
            Radius = radius;
            BorderWidth = Mathf.Max(borderWidth, 0);
            PixelSize = Mathf.Max(0, pixelSize);
        }
    }
}