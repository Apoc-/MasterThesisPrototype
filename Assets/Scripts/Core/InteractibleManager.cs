using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public Phone Phone;
        
        public void AddToNpcInteractibles(Interactible interactible)
        {
            NpcInteractibles.Add(interactible);
        }
        
        public void RegisterInteractible(Interactible interactible)
        {
            Interactibles.Add(interactible);
        }
    }
}