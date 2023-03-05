using UnityEditor;
using UnityEngine;

namespace PixelSpark.GhostSprite.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : ConditionalAttributeDrawer
    {
        protected override PropertyDrawing GetPropertyDrawing()
        {
            return PropertyDrawing.Show;
        }
    }
}