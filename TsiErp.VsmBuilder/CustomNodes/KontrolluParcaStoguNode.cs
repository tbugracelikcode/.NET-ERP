using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class KontrolluParcaStoguNode : NodeModel
    {
        [DataMember]
        public string ClassName { get; set; } = "KontrolluParcaStoguNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        [DataMember]
        public string nodeID { get; set; }
    }
}
