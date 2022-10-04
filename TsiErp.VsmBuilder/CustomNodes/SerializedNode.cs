using Blazor.Diagrams.Core.Models;
using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    public class SerializedNode : NodeModel
    {
        public string Name { get; set; }
        public string Adet { get; set; }
        public string Gun { get; set; }
        public string Sayi { get; set; }
        public string Departman { get; set; }
        public string Istasyon { get; set; }
        public string IslemeZamani { get; set; }
        public string CevrimZamani { get; set; }
        public string VardiyaSayisi { get; set; }
        public string FireOrani { get; set; }
        public string ClassName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string ParentID { get; set; }
        public string nodeID { get; set; }
        public string StokIsaretiNodeID { get; set; }
        public string OperatorNodeID { get; set; }
        public string ZamanCizgisiNodeID { get; set; }
    }
}
