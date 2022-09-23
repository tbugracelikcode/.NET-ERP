using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class DisKaynaklarNode : NodeModel
    {
        [DataMember]
        public string ClassName { get; set; } = "DisKaynaklarNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

    }
}
