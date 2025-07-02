using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemData), true)]
public class MyClassPropertyDrawer : PropertyDrawer
{
    // 한 줄 높이
    private float lineHeight = EditorGUIUtility.singleLineHeight;
    private float verticalSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 각 필드를 차례로 그리기 위해 Rect 쪼개기
        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // Id 필드 (레이블 너비 30, 필드 너비 50)
        float labelWidthBackup = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 30f;
        SerializedProperty idProp = property.FindPropertyRelative("Id");
        EditorGUI.PropertyField(currentRect, idProp, new GUIContent("ID"));
        EditorGUIUtility.labelWidth = labelWidthBackup;

        // 다음 줄
        currentRect.y += lineHeight + verticalSpacing;

        // Name 필드 (레이블 너비 50, 필드 너비 position.width - 60)
        EditorGUIUtility.labelWidth = 50f;
        SerializedProperty nameProp = property.FindPropertyRelative("Name");
        EditorGUI.PropertyField(currentRect, nameProp, new GUIContent("Name"));
        EditorGUIUtility.labelWidth = labelWidthBackup;

        // 다음 줄
        currentRect.y += lineHeight + verticalSpacing;

        // Description 필드 (레이블 너비 80)
        EditorGUIUtility.labelWidth = 80f;
        SerializedProperty descProp = property.FindPropertyRelative("Description");
        EditorGUI.PropertyField(currentRect, descProp, new GUIContent("Description"));
        EditorGUIUtility.labelWidth = labelWidthBackup;

        // 다음 줄
        currentRect.y += lineHeight + verticalSpacing;

        // IconImage 필드 (레이블 너비 60)
        EditorGUIUtility.labelWidth = 60f;
        SerializedProperty iconProp = property.FindPropertyRelative("IconImage");
        EditorGUI.PropertyField(currentRect, iconProp, new GUIContent("Icon"));
        EditorGUIUtility.labelWidth = labelWidthBackup;

        // 다음 줄
        currentRect.y += lineHeight + verticalSpacing;

        // Type 필드
        SerializedProperty typeProp = property.FindPropertyRelative("Type");
        EditorGUI.PropertyField(currentRect, typeProp);

        currentRect.y += lineHeight + verticalSpacing;

        // Value 필드
        SerializedProperty valueProp = property.FindPropertyRelative("Value");
        EditorGUI.PropertyField(currentRect, valueProp);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 필드 7개 * 줄 높이 + 간격 포함
        return (EditorGUIUtility.singleLineHeight + 2f) * 7;
    }
}
