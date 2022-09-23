using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class IyilestirmeCalismasiNode : NodeModel
    {
        [DataMember]
        public string ClassName { get; set; } = "IyilestirmeCalismasiNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

    }
}
