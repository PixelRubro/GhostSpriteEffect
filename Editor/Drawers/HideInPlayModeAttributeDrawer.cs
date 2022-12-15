using UnityEditor;
using UnityEngine;

namespace SoftBoiledGames.GhostSpriteEffect.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(HideInPlayModeAttribute))]
    public class HideInPlayModeAttributeDrawer : BasePropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
                return 0f;

            return base.GetPropertyHeight(property, label);
        }
    }
}