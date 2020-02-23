using System;
using System.Collections.Generic;

namespace Core
{
    public class WaterDispenserInteractible : Fixable
    {
        private void Start()
        {
            _brokenTooltips = new List<string> {
                "Becher bereitstellen", "Wassertank austauschen"
            };
        }

        public override string GetName() => "Wasserspender";

        public override void OnFixed()
        {
            
        }
    }
}