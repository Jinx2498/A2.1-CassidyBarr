using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Weebles
{
    [AddComponentMenu("Scripts/GameBrains/Weebles/Weeble")]
    public class Weeble : ExtendedMonoBehaviour
    {
        public string shortName;
        public Color color;
    }
}