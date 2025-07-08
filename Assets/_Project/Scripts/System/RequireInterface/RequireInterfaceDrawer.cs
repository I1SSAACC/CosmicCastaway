#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
public class RequireInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RequireInterfaceAttribute attr = (RequireInterfaceAttribute)attribute;

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.ObjectField(
            position,
            property,
            attr.RequiredType,
            label
        );
        EditorGUI.EndProperty();
    }
}
#endif