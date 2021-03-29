using System.Collections.Generic;

namespace KappaQueueCommon.Common.Language
{
    public class Language
    {
        public string Prefix;
        public string Name;
        public string LocalizedName;
    }

    public class Languages : List<Language>
    {
        public const string ENGLISH = "en";
        public const string RUSSIAN = "ru";
        public Languages()
            : base()
        {
            Add(new Language { Prefix = RUSSIAN, Name = "Russian", LocalizedName = "Русский" });
            Add(new Language { Prefix = ENGLISH, Name = "English", LocalizedName = "English" });
        }
    }
}
