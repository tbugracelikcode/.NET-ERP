using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{    
    [DataContract]
    public class BilgiKutusuNode : NodeModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Departman { get; set; }
        [DataMember]
        public string Istasyon { get; set; }
        [DataMember]
        public string IslemeZamani { get; set; }
        [DataMember]
        public string CevrimZamani { get; set; }
        [DataMember]
        public string VardiyaSayisi { get; set; }
        [DataMember]
        public string FireOrani { get; set; }
        [DataMember]
        public string ClassName { get; set; } = "BilgiKutusuNode";
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
