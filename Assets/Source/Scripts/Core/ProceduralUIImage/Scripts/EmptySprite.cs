namespace UnityEngine.UI.ProceduralImage
{
    public static class EmptySprite
    {
        private static Sprite _sprite;

        public static Sprite GetSprite()
        {
            if (_sprite == null)
                _sprite = Resources.Load<Sprite>("procedural_ui_image_default_sprite");

            return _sprite;
        }

        public static bool IsEmptySprite(Sprite sprite) => GetSprite() == sprite;
    }
}