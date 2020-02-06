using System;
using System.Transactions;

namespace Core
{
    public class Phone : Fixable
    {
        public override string GetName() => "Phone";

        public override void OnFixed()
        {
            
        }
    }
}