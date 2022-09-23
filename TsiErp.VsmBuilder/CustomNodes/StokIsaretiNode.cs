using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class StokIsaretiNode : NodeModel
    {

        [DataMember]
        public string Adet { get; set; }
        [DataMember]
        public string Gun { get; set; }
        [DataMember]
        public string ClassName { get; set; } = "StokIsaretiNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

    }
}
