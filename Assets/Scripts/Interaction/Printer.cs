using System;
using System.Collections.Generic;
using System.Transactions;
using Random = UnityEngine.Random;

namespace Core
{
    public class Printer : Fixable
    {
        private void Start()
        {
            _brokenTooltips = new List<String>
            {
                "Druckerpapier nachfüllen",
                "Kartusche wechseln",
                "Papierstau auflösen"
            };
        }

        public override string GetName() => "Printer";
        public override void OnFixed()
        {
            
        }
    }
}