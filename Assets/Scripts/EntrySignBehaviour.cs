using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntrySignBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject[] DaySigns;

        public void EnableSignByDay(int day)
        {
            DisableAllSigns();
            if (day > DaySigns.Length) return;
            
            DaySigns[day-1].SetActive(true);
        }

        private void DisableAllSigns()
        {
            foreach (var daySign in DaySigns)
            {
                daySign.SetActive(false);
            }
        }
    }
}