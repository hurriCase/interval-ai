using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal class DateProgress : ProgressItem
    {
        [SerializeField] private GameObject _fireIcon;

        protected override void OnInit(bool isActive)
        {
            _fireIcon.SetActive(isActive);
        }
    }
}