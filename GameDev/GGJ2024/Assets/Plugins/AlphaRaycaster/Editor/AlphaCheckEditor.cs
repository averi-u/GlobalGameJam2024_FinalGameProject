// Copyright 2014-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

namespace AlphaRaycaster
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;
    
    [CustomEditor(typeof(AlphaCheck)), CanEditMultipleObjects]
    public class AlphaCheckEditor : Editor
    {
        private SerializedProperty alphaThreshold;
        private SerializedProperty includeMaterialAlpha;
    
        private void OnEnable ()
        {
            alphaThreshold = serializedObject.FindProperty("AlphaThreshold");
            includeMaterialAlpha = serializedObject.FindProperty("IncludeMaterialAlpha");
        }
    
        public override void OnInspectorGUI ()
        {
            var activeGo = Selection.activeGameObject;
            if (activeGo)
            {
                if (activeGo.GetComponent<Image>())
                {
                    var image = activeGo.GetComponent<Image>();
                    var path = AssetDatabase.GetAssetPath(image.mainTexture);
                    if (path != string.Empty && !image.sprite.packed)
                    {
                        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (!textureImporter)
                        {
                            EditorGUILayout.HelpBox("Assign a custom source image to the Image component to configure alpha checking.\nBuilt-in Unity images are not supported.", MessageType.Warning);
                            return;
                        }
    
                        if (!textureImporter.isReadable)
                        {
                            EditorGUILayout.HelpBox("The texture is not readable. Alpha check won't have effect.", MessageType.Warning);
                            if (GUILayout.Button("FIX"))
                            {
                                textureImporter.isReadable = true;
                                AssetDatabase.ImportAsset(path);
                            }
                            return;
                        }
                    }
                    else if (!image.sprite)
                    {
                        EditorGUILayout.HelpBox("Assign a source image to the Image component to configure alpha checking.", MessageType.Warning);
                        return;
                    }
    
                    BlockingChildsGUI(activeGo);
                }
                else if (activeGo.GetComponent<Text>())
                {
                    var text = activeGo.GetComponent<Text>();
                    var path = AssetDatabase.GetAssetPath(text.mainTexture);
                    if (path != string.Empty)
                    {
                        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (!textureImporter || !textureImporter.isReadable)
                        {
                            EditorGUILayout.HelpBox("The font texture is not readable. Alpha check won't have effect.\nConsult the documentation on how to prepare fonts to use with Alpha Raycaster.", MessageType.Warning);
                            return;
                        }
                    }
                }
                else if (activeGo.GetComponent<RawImage>())
                {
                    var rawImage = activeGo.GetComponent<RawImage>();
                    var path = AssetDatabase.GetAssetPath(rawImage.mainTexture);
                    if (path != string.Empty)
                    {
                        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (!textureImporter)
                        {
                            EditorGUILayout.HelpBox("Assign a custom source image to the RawImage component to configure alpha checking.\nBuilt-in Unity images are not supported.", MessageType.Warning);
                            return;
                        }
    
                        if (!textureImporter.isReadable)
                        {
                            EditorGUILayout.HelpBox("The texture is not readable. Alpha check won't have effect.", MessageType.Warning);
                            if (GUILayout.Button("FIX"))
                            {
                                textureImporter.isReadable = true;
                                AssetDatabase.ImportAsset(path);
                            }
                            return;
                        }
                    }
                    else if (!rawImage.mainTexture)
                    {
                        EditorGUILayout.HelpBox("Assign a source texture to the RawImage component to configure alpha checking.", MessageType.Warning);
                        return;
                    }
    
                    BlockingChildsGUI(activeGo);
                }
                else
                {
                    EditorGUILayout.HelpBox("Can't find Image, RawImage or Text components. Alpha check is only possible for UI objects with an Image or Text components attached.", MessageType.Error);
                    return;
                }
            }
            else return;
    
            serializedObject.Update();
            EditorGUILayout.PropertyField(alphaThreshold);
            EditorGUILayout.PropertyField(includeMaterialAlpha);
            serializedObject.ApplyModifiedProperties();
        }
    
        private void BlockingChildsGUI (GameObject activeGo)
        {
            var blockingChilds = activeGo.GetComponentsInChildren<CanvasRenderer>(false)
                .Where(child => child.gameObject != activeGo && (!child.GetComponent<CanvasGroup>() || child.GetComponent<CanvasGroup>().blocksRaycasts)).ToList();
            if (blockingChilds.Count > 0)
            {
                EditorGUILayout.HelpBox("Some of the child objects may be blocking the raycast.", MessageType.Warning);
                if (GUILayout.Button("FIX"))
                {
                    foreach (var blockingChild in blockingChilds)
                    {
                        var canvasGroup = blockingChild.GetComponent<CanvasGroup>() ? blockingChild.GetComponent<CanvasGroup>() : blockingChild.gameObject.AddComponent<CanvasGroup>();
                        canvasGroup.blocksRaycasts = false;
                    }
                }
            }
        }
    }
    
}
