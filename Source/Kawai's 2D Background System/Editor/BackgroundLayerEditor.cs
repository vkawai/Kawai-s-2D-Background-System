using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BackgroundLayer))]
public class BackgroundLayerEditor : Editor {
    public override void OnInspectorGUI() {

        BackgroundLayer layer = (BackgroundLayer)target;

        EditorGUILayout.BeginVertical();
        
        // I will find if the current layer has any sprite.
        GUILayout.Label("Sprite:");
        if(layer.gameObject.GetComponent<SpriteRenderer>().sprite == null) {
            // If none, Show a warning!
            EditorGUILayout.HelpBox("Sprite not found!\nAssign an sprite to Sprite Renderer!", MessageType.Error);
        }
        else {
            // If there is an sprite, I will show this layer's sprite.
            Texture2D texture = layer.gameObject.GetComponent<SpriteRenderer>().sprite.texture;
            GUILayout.Label(texture, new GUILayoutOption[] { GUILayout.MaxHeight((EditorGUIUtility.currentViewWidth - 50) * 0.50f) });

            EditorGUILayout.Separator();
            
            EditorGUI.BeginChangeCheck();
            bool toggled = EditorGUILayout.ToggleLeft(new GUIContent("Static Layer", "Static layers won't loop or be affected to parallax effect.\nIt will remain attatched to the main camera."), layer.isStatic);
            if(EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(layer, "Backgroun Layer: Changed Static Propertie.");
                layer.isStatic = toggled;
            }

            // Static layer won't use any other attribute. It doesn't matter if ther are anything setted since it will be cleaned up on Start() routine.
            // So, if this layer is static, I will hide everything else.
            if(!layer.isStatic) {
                EditorGUILayout.Separator();

                EditorGUI.BeginChangeCheck();
                toggled = EditorGUILayout.ToggleLeft("Loop Horizontal", layer.loopHorizontal);
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(layer, "Backgroun Layer: Changed Horizontal Loop.");
                    layer.loopHorizontal = toggled;
                }

                EditorGUI.BeginChangeCheck();
                toggled = EditorGUILayout.ToggleLeft("Loop Vertical", layer.loopVertical);
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(layer, "Backgroun Layer: Changed Vertical Loop.");
                    layer.loopVertical = toggled;
                }

                EditorGUI.BeginChangeCheck();
                float value = EditorGUILayout.FloatField("Horizontal Parallax Factor", layer.horizontalParallaxFactor);
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(layer, "Backgroun Layer: Changed Horizontal Parallax Factor.");
                    layer.horizontalParallaxFactor = value;
                }


                EditorGUI.BeginChangeCheck();
                value = EditorGUILayout.FloatField("Vertical Parallax Factor", layer.verticalParallaxFactor);
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(layer, "Backgroun Layer: Changed Vertical Parallax Factor.");
                    layer.verticalParallaxFactor = value;
                }

            }
        }

        EditorGUILayout.EndVertical();
    }
    
}
