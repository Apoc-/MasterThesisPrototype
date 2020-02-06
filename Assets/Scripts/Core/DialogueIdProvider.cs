namespace Core
{
    public static class DialogueIdProvider
    {
        public static string GetDialogueIdByDay(int day)
        {
            switch (day)
            {
                case 1:
                    return "advisor_01";
                default:
                    return "dummy_text";
            }
        }
    }
}