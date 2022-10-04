using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    [DataContract]
    public class ZamanCizgisiNode : NodeModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string GunSayisi { get; set; }
        [DataMember]
        public string ClassName { get; set; } = "ZamanCizgisiNode";
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        [DataMember]
        public string nodeID { get; set; }
        [DataMember]
        public string ParentID { get; set; }
        [DataMember]
        public string StokIsaretiNodeID { get; set; }
        [DataMember]
        public string OperatorNodeID { get; set; }
        [DataMember]
        public string ZamanCizgisiNodeID { get; set; }

    }
}
