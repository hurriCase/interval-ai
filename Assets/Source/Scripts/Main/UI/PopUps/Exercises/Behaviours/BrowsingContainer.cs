using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours
{
    internal sealed class BrowsingContainer : MonoBehaviour
    {
        [SerializeField] private ExerciseSearch _exerciseSearch;

        internal void Init()
        {
            _exerciseSearch.Init();
        }
    }
}