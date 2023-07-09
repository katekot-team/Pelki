using UnityEngine;

namespace Pelki.Configs
{
    [CreateAssetMenu(fileName = nameof(InputConfig), menuName = "Configs/" + nameof(InputConfig), order = 0)]
    public class InputConfig : ScriptableObject
    {
        [SerializeField] private string horizontalAxisKey;
        [SerializeField] private string verticalAxisKey;

        public string HorizontalAxisKey => horizontalAxisKey;
        public string VerticalAxisKey => verticalAxisKey;
    }
}