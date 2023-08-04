using System.Collections.Generic;
using NaughtyAttributes;
using Pelki.Configs;
using UnityEngine;

namespace Pelki.Utils
{
    public class SetupButtonInputUIByConfig : MonoBehaviour
    {
        [SerializeField] private SimpleInputNamespace.ButtonInputUI button;

        [Dropdown(nameof(EditorGetAllButtonsIds))]
        [SerializeField] private string idButton;

        [SerializeField] private InputConfig inputConfig;

        private void Awake()
        {
            string keyButton = inputConfig.GetKeyButtonById(idButton);
            button.button.Key = keyButton;
        }

        private IEnumerable<string> EditorGetAllButtonsIds()
        {
            IEnumerable<string> result = inputConfig.GetAllButtonsIds();
            return result;
        }
    }
}