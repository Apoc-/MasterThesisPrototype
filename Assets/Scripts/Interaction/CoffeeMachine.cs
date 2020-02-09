using System;
using System.Collections.Generic;
using System.Transactions;
using Random = UnityEngine.Random;

namespace Core
{
    public class CoffeeMachine : Fixable
    {
        private void Start()
        {
            _brokenTooltips = new List<string> {
                "Kaffee nachfüllen", "Wasser nachfüllen", "Neue Milch aufmachen"
            };
        }

        public override void StartInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Kaffee Maschine";

        public override void OnFixed()
        {
            
        }
    }
}