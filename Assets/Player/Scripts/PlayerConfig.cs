using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Config", fileName = "Player config")]
    public class PlayerConfig : ScriptableObject{
        public int BaseHealth = 100;
        public int MaxHealth = 100;
        public float Speed = 5;
        public float JumpForce = 5;
    }
}