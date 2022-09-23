using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class EmniyetStoguNode : NodeModel
    {
        [DataMember]
        public string ClassName { get; set; } = "EmniyetStoguNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

    }
}
