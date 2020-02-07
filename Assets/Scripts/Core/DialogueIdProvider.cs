namespace Core
{
    public static class DialogueIdProvider
    {
        public static string GetDialogueIdByDay(int day)
        {
            //return "dummy_text";
            
            switch (day)
            {
                case 1:
                    return "advisor_01";
                case 2:
                    return "advisor_02";
                default:
                    return "dummy_text";
            }
        }
    }
}