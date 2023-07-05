using UnityEditor;

namespace Assets.SimpleLocalization.Editor
{
    public static class LocalizationEditorUtil
    {
        [MenuItem("Pelki/LocalizationSyncResources")]
        private static void LocalizationSyncResources()
        {
            LocalizationManager.Read();
        }
    }
}