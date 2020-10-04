using UnityEngine;

namespace Code
{
    [CreateAssetMenu(menuName = "BP/Weapon")]
    public class WeaponBlueprint : ScriptableObject
    {
        public string displayName;
        public GameObject prefab;
        public GameObject projectilePrefab;
    }
}