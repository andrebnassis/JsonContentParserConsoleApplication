using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Models
{
    [DataContract]
    public class Example1
    {
        public Example1(int id, double num, List<string> stringList, List<int> intList, List<double> doubleList, Dictionary<string, string> dict, List<Example2> example2)
        {
            Id = id;
            Num = num;
            StringList = stringList;
            IntList = intList;
            DoubleList = doubleList;
            Dict = dict;
            Example2 = example2;

        }

        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "num")]
        public double Num { get; set; }
        [DataMember(Name = "stringlist")]
        public List<string> StringList { get; set; }
        [DataMember(Name = "intlist")]
        public List<int> IntList { get; set; }
        [DataMember(Name = "doublelist")]
        public List<double> DoubleList { get; set; }
        [DataMember(Name = "dict")]
        public Dictionary<string, string> Dict { get; set; }
        [DataMember(Name = "example2")]
        public List<Example2> Example2 { get; set; }
        //public Example2 Example2 { get; set; }

    }
}
