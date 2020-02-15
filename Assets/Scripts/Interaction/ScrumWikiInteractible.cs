using System;
using System.Collections.Generic;
using System.Transactions;
using UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class ScrumWikiInteractible : Interactible
    {
        public WikiScreenBehaviour ScrumWikiScreen;
        
        public override void StartInteraction(Entity entity)
        {
            ScrumWikiScreen.Show();       
        }

        public override void FinishInteraction(Entity entity)
        {
            
        }

        public override string GetName() => "Scrum-Wiki";
        public override string GetTooltip() => "Scrum-Wiki lesen";
    }
}