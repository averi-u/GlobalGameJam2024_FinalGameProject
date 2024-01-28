// Copyright 2014-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

namespace AlphaRaycaster
{
    using UnityEngine;
    using UnityEngine.UI;
    
    [AddComponentMenu("Event/Alpha Check"), ExecuteInEditMode]
    public class AlphaCheck : MonoBehaviour, ICanvasRaycastFilter
    {
        [Range(0, 1), Tooltip("Texture regions with opacity (alpha) lower than alpha threshold won't react to input events.")]
        public float AlphaThreshold = .9f;
        [Tooltip("Whether material tint color should affect alpha threshold.")]
        public bool IncludeMaterialAlpha;
    
        private GameObject gameObj;
        private Image checkedImage;
        private RawImage checkedRawImage;
        private Text checkedText;
        private bool isSetupValid;
    
        private void Awake ()
        {
            gameObj = gameObject;
            checkedImage = GetComponent<Image>();
            checkedRawImage = GetComponent<RawImage>();
            checkedText = GetComponent<Text>();
            isSetupValid = checkedImage || checkedRawImage || checkedText;
        }
    
        public bool IsRaycastLocationValid (Vector2 screenPosition, Camera eventCamera)
        {
            if (!isSetupValid) return true;
    
            if (checkedImage)
                return !AlphaRaycaster.AlphaCheckImage(gameObj, checkedImage, screenPosition, eventCamera, AlphaThreshold, IncludeMaterialAlpha);
            if (checkedRawImage)
                return !AlphaRaycaster.AlphaCheckRawImage(gameObj, checkedRawImage, screenPosition, eventCamera, AlphaThreshold, IncludeMaterialAlpha);
            if (checkedText)
                return !AlphaRaycaster.AlphaCheckText(gameObj, checkedText, screenPosition, eventCamera, AlphaThreshold, IncludeMaterialAlpha);
    
            return true;
        }
    }
    
}
