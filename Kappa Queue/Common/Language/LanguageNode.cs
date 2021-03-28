using System.Collections.Generic;

namespace Kappa_Queue.Common.Language
{
    public class LanguageNode
    {
        public string Language;
        public string Value;
    }

    public class Collocation
    {
        public string Code;
        public List<LanguageNode> Nodes = new List<LanguageNode>();
    }
}
