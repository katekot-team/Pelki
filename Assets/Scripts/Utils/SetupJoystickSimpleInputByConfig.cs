using NaughtyAttributes;
using Pelki.Configs;
using UnityEngine;

namespace Pelki.Utils
{
    public class SetupJoystickSimpleInputByConfig : MonoBehaviour
    {
        [InfoBox("Horizontal Axis KEY get by config!", EInfoBoxType.Warning)]
        [SerializeField] private InputConfig config;
        [SerializeField] private SimpleInputNamespace.Joystick joystick;

        private void Awake()
        {
            joystick.xAxis.Key = config.HorizontalAxisKey;
            joystick.yAxis.Key = config.VerticalAxisKey;
        }
    }
}