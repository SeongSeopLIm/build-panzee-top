using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace WAK
{

    public class UIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [System.Serializable]
        private class ScaleEffectData
        {
            public float FocusScale = 1.1f;
            public float Duration = 0.15f;
            public Ease Ease = Ease.OutQuad;
        }

        [System.Serializable]
        private class SoundEffectData
        {
            public string HoverSFXKey;
            public string InteractSFXKey;
        }

        [SerializeField] private bool scaleEffect = true;
        // TODO : scaleEffect에 다른 인스펙터 노출 토글 적용
        [SerializeField] private ScaleEffectData scaleEffectData;


        [SerializeField] private bool soundEffect = true;
        [SerializeField] private SoundEffectData soundEffectData;

        private Selectable selectable;

        private void OnValidate()
        {
            if(!gameObject.TryGetComponent<Selectable>(out selectable))
            {
                Debug.LogWarning("Selectable 찾을 수 없음.");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(scaleEffect)
            {
                transform.DOScale(scaleEffectData.FocusScale, scaleEffectData.Duration)
                    .SetId(GetInstanceID());
            }

            if(soundEffect)
            {
                // TODO : 오디오재생 추가
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (scaleEffect)
            {
                DOTween.Kill(GetInstanceID());
                transform.DOScale(1, scaleEffectData.Duration)
                    .SetId(GetInstanceID());
            }
            if (soundEffect)
            {
                // TODO : 오디오재생 추가
            }
        }
    }

}
