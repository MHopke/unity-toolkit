namespace gametheory.Localization
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class LocalizationKey : System.Attribute
    {
        public string Key;

        public LocalizationKey(string key)
        {
            Key = key;
        }
    }
}
