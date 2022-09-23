using System.Runtime.Serialization;

namespace TsiErp.VsmBuilder.CustomNodes
{
    public class SerializedNode
    {
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
    }
}
