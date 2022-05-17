using UnityEngine;

namespace neeksdk.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Configs/Background config")]
    public class BackgroundConfig : ScriptableObject
    {
        public BackgroundPrefabData[] Backgrounds;
    }
}