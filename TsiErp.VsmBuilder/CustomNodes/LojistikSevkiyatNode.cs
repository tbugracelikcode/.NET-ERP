using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class LojistikSevkiyatNode : NodeModel
    {
        [DataMember]
        public string ClassName { get; set; } = "LojistikSevkiyatNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        [DataMember]
        public string nodeID { get; set; }

    }
}
