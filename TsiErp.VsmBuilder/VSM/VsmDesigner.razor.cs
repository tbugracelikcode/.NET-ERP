using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.VsmBuilder.CustomNodes;
using Newtonsoft.Json;
using System.Linq;

namespace TsiErp.VsmBuilder.VSM
{
    public partial class VsmDesigner
    {
        private readonly Diagram _diagram = new Diagram();
        private int? _draggedType;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _diagram.RegisterModelComponent<BotAnswerNode, BotAnswerWidget>();
            _diagram.RegisterModelComponent<BilgiKutusuNode, BilgiKutusuWidget>();
        }

        private void OnDragStart(int key)
        {
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null)
                return;

            var position = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var node = _draggedType == 0 ? new NodeModel(position) : new BilgiKutusuNode() { Position = position, X = position.X, Y = position.Y };
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            _diagram.Nodes.Add(node);
            _draggedType = null;
        }

        void Tikla()
        {
            var list = _diagram.Nodes.ToList();

            string nodeJsonString = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
