using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class BotAnswerNode : NodeModel
    {

        [DataMember]
        public string Fikri { get; set; }
        [DataMember]
        public string Turan { get; set; }
        [DataMember]
        public string HO { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

    }
}
