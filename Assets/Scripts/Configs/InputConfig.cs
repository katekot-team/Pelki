using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(InputConfig), menuName = "Configs/" + nameof(InputConfig), order = 0)]
    public class InputConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Hard keys")]
        [SerializeField] private string horizontalAxisKey;
        [SerializeField] private string verticalAxisKey;

        [Header("Assigned id")]
        [Dropdown(nameof(GetAllButtonsIds))]
        [SerializeField] private string none;

        [Dropdown(nameof(GetAllButtonsIds))]
        [SerializeField] private string jumpId;
        
        [Dropdown(nameof(GetAllButtonsIds))]
        [SerializeField] private string rangedAttackId;

        [Header("Other")]
        [SerializeField] private List<ButtonInfo> buttonInfos;

        private Dictionary<string, string> buttonKeyByIdDictionary;

        public string HorizontalAxisKey => horizontalAxisKey;
        public string VerticalAxisKey => verticalAxisKey;
        public string JumpKey => GetKeyButtonById(jumpId);
        public string RangedAttackKey => GetKeyButtonById(rangedAttackId);

        public string GetKeyButtonById(string idButton)
        {
            string result = buttonKeyByIdDictionary[idButton];
            return result;
        }

        public IEnumerable<string> GetAllButtonsIds()
        {
            //sttrox: .Key because it dictionary and he hava data hoy Key,Value
            IEnumerable<string> result = buttonKeyByIdDictionary.Select(pair => pair.Key).ToList();
            return result;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            //sttrox: do nothing
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            buttonKeyByIdDictionary = buttonInfos.ToDictionary(info => info.ID, info => info.Key);
        }

        [Serializable]
        private class ButtonInfo
        {
            [SerializeField] private string id;
            [SerializeField] private string key;

            public string ID => id;
            public string Key => key;
        }
    }
}