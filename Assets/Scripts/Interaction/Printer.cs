using System;
using System.Transactions;

namespace Core
{
    public class Printer : Fixable
    {
        public override string GetName() => "Printer";

        public override void OnFixed()
        {
            
        }
    }
}