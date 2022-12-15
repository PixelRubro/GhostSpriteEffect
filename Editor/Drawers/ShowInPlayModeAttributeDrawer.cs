using UnityEngine;
using UnityEditor;

namespace SoftBoiledGames.GhostSpriteEffect.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(ShowInPlayModeAttribute))]
    public class ShowInPlayModeAttributeDrawer : BasePropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!Application.isPlaying)
            {
                return 0f;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}
