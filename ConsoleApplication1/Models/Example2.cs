using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Models
{
    [DataContract]
    public class Example2
    {
        public Example2(int num, Dictionary<string, string> dict, Dictionary<int, string> dictInt)
        {
            Num = num;
            Dict = dict;
            DictInt = dictInt;

        }
        [DataMember(Name = "num")]
        public int Num { get; set; }
        [DataMember(Name = "dict")]
        public Dictionary<string, string> Dict { get; set; }
        [DataMember(Name = "dictint")]
        public Dictionary<int, string> DictInt { get; set; }

    }
}
