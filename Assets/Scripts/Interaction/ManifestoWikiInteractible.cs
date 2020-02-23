using System;
using System.Collections.Generic;
using System.Transactions;
using UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class ManifestoWikiInteractible : Interactible
    {
        public WikiScreenBehaviour ManifestoWikiScreen;
        
        public override void StartInteraction(Entity entity)
        {
            ManifestoWikiScreen.Show();       
        }

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Das Agile Manifest";
        public override string GetTooltip() => "Das Agile Manifest lesen";
    }
}