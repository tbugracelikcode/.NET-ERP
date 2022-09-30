using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class OperatorNode : NodeModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Sayi { get; set; }
        [DataMember]
        public string ClassName { get; set; } = "OperatorNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        [DataMember]
        public string ParentID { get; set; }

    }
}
