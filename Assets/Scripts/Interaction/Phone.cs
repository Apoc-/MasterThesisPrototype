using System;
using System.Collections.Generic;
using System.Transactions;
using Random = UnityEngine.Random;

namespace Core
{
    public class Phone : Fixable
    {
        private void Start()
        {
            _brokenTooltips = new List<string> {
                "Telefonat annehmen"
            };
        }

        public override void StartInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Telefon";
        public override void OnFixed()
        {
            
        }

        public override string GetBrokenMessage()
        {
            return "Das Telefon klingelt Sturm, muss was ganz wichtiges sein!";
        }
    }
}