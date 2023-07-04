using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
    /// <summary>
    /// Localize text component.
    /// </summary>
    public class LocalizedText : MonoBehaviour
    {
        [FormerlySerializedAs("LocalizationKey")]
        [Dropdown(nameof(EditorGetDropDownLocalizationKeys))]
        [SerializeField] private string localizationKey;

        [HideIf(nameof(EditorTextMeshProFill))]
        [SerializeField] private Text textTarget;

        [HideIf(nameof(EditorHasSampleTextFill))]
        [SerializeField] private TMP_Text targetTextMeshPro;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Reset()
        {
            if (TryGetComponent(out Text foundTextComponent))
            {
                textTarget = foundTextComponent;
                targetTextMeshPro = null;
                return;
            }

            if (TryGetComponent(out TMP_Text foundTextMeshProComponent))
            {
                targetTextMeshPro = foundTextMeshProComponent;
                textTarget = null;
                return;
            }
        }

        private void Localize()
        {
            if (targetTextMeshPro != null)
            {
                targetTextMeshPro.text = LocalizationManager.Localize(localizationKey);
            }
            else if (textTarget != null)
            {
                textTarget.text = LocalizationManager.Localize(localizationKey);
            }
            else
            {
                Debug.LogError($"In '{this.name}' not set target, try set text or TextMeshPro", this);
            }
        }

        private IReadOnlyList<string> EditorGetDropDownLocalizationKeys()
        {
            return LocalizationManager.GetKeys();
        }

        private bool EditorTextMeshProFill()
        {
            return targetTextMeshPro != null;
        }

        private bool EditorHasSampleTextFill()
        {
            return textTarget != null;
        }

        [Button()]
        private void EditorTrySetTarget()
        {
            Reset();
        }
    }
}