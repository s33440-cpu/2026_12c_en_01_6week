using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Config", fileName = "Player config")]
    public class PlayerConfig : ScriptableObject{
        public int BaseHealth = 2;
        public int MaxHealth = 5;
    }
}