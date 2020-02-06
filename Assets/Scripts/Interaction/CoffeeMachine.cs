using System;
using System.Transactions;

namespace Core
{
    public class CoffeeMachine : Fixable
    {
        public override string GetName() => "Coffee Machine";

        public override void OnFixed()
        {
            
        }
    }
}