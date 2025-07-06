using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress
{
    internal sealed class UILineRenderer : Graphic
    {
        [SerializeField] private float _lineWidthPercent;
        [SerializeField] private float _pointRadiusPercent;
        [SerializeField] private bool _drawPoints;
        [SerializeField] private int _circleSegments;
        [SerializeField] private List<Vector2> _points = new();

        protected override void OnValidate()
        {
            base.OnValidate();

            SetVerticesDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            SetVerticesDirty();
        }

        internal void SetPoints(List<Vector2> newPoints)
        {
            _points.Clear();
            _points.AddRange(newPoints);

            SetVerticesDirty();
            SetMaterialDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (_points.Count < 1)
                return;

            var rect = GetPixelAdjustedRect();
            var scaledLineWidth = rect.height * _lineWidthPercent * 0.01f;
            var scaledPointRadius = rect.height * _pointRadiusPercent * 0.01f;

            if (_points.Count >= 2)
            {
                for (var i = 0; i < _points.Count - 1; i++)
                {
                    var start = ConvertToLocalPoint(_points[i], rect);
                    var end = ConvertToLocalPoint(_points[i + 1], rect);
                    DrawLine(vh, start, end, scaledLineWidth);
                }
            }

            if (_drawPoints is false)
                return;

            foreach (var point in _points)
            {
                var center = ConvertToLocalPoint(point, rect);
                DrawCircle(vh, center, scaledPointRadius);
            }
        }

        private Vector2 ConvertToLocalPoint(Vector2 normalizedPoint, Rect rect)
        {
            var x = normalizedPoint.x * rect.width - rect.width * 0.5f;
            var y = normalizedPoint.y * rect.height - rect.height * 0.5f;

            return new Vector2(x, y);
        }

        private void DrawLine(VertexHelper vh, Vector2 start, Vector2 end, float width)
        {
            var direction = (end - start).normalized;
            var perpendicular = new Vector2(-direction.y, direction.x) * width * 0.5f;

            var vertIndex = vh.currentVertCount;

            vh.AddVert(start + perpendicular, color, Vector2.zero);
            vh.AddVert(start - perpendicular, color, Vector2.zero);
            vh.AddVert(end - perpendicular, color, Vector2.zero);
            vh.AddVert(end + perpendicular, color, Vector2.zero);

            vh.AddTriangle(vertIndex, vertIndex + 1, vertIndex + 2);
            vh.AddTriangle(vertIndex + 2, vertIndex + 3, vertIndex);
        }

        private void DrawCircle(VertexHelper vh, Vector2 center, float radius)
        {
            var vertIndex = vh.currentVertCount;

            vh.AddVert(center, color, Vector2.zero);

            for (var i = 0; i <= _circleSegments; i++)
            {
                var angle = i / (float)_circleSegments * 2f * Mathf.PI;
                var pos = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
                vh.AddVert(pos, color, Vector2.zero);
            }

            for (var i = 0; i < _circleSegments; i++)
                vh.AddTriangle(vertIndex, vertIndex + i + 1, vertIndex + i + 2);
        }
    }
}