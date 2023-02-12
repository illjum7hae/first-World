using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TerrainGeneration))]
public class SeedOneLine : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        TerrainGeneration example = (TerrainGeneration)property.objectReferenceValue;
        string displayString = string.Format("Seed value: {0}, Displayed Seed value: {1}", example.seed, example.saveSeed);
        EditorGUI.LabelField(position, displayString);
    }
}

