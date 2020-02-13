using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class InteractibleManager : MonoBehaviour
    {
        private HashSet<Interactible> _interactibles = new HashSet<Interactible>();
        private int _maxBrokenCount = 3;

        public HashSet<Interactible> Interactibles
        {
            get => _interactibles;
        }

        public List<Interactible> NpcInteractibles;

        public TaskboardInteractible TaskboardInteractible;
        
        public void RegisterInteractible(Interactible interactible, bool isNpcInteractible = false)
        {
            Interactibles.Add(interactible);

            if (isNpcInteractible)
            {
                NpcInteractibles.Add(interactible);
            }
        }

        public void BreakRandomBreakable()
        {
            var fixables = Interactibles.OfType<Fixable>().ToList();
            if(fixables.Count(fixable => fixable.IsBroken) >= _maxBrokenCount) return;
            var rnd = Random.Range(0, fixables.Count);
            fixables[rnd].Break();
        }
    }
}