using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.VsmBuilder.CustomNodes;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Core.Behaviors;

namespace TsiErp.VsmBuilder.VSM
{
    public partial class VsmDesigner
    {
        private int? _draggedType;
        private readonly Diagram _diagram = new Diagram();


        protected override void OnInitialized()
        {
            base.OnInitialized();

            #region RegisterModelComponent
            _diagram.RegisterModelComponent<BilgiKutusuNode, BilgiKutusuWidget>();
            _diagram.RegisterModelComponent<StokIsaretiNode, StokIsaretiWidget>();
            _diagram.RegisterModelComponent<BitmisUrunNode, BitmisUrunWidget>();
            _diagram.RegisterModelComponent<CekmeKanbaniNode, CekmeKanbaniWidget>();
            _diagram.RegisterModelComponent<DisKaynaklarNode, DisKaynaklarWidget>();
            _diagram.RegisterModelComponent<EmniyetStoguNode, EmniyetStoguWidget>();
            _diagram.RegisterModelComponent<FifoTransferiNode, FifoTransferiWidget>();
            _diagram.RegisterModelComponent<ItmeHareketiNode, ItmeHareketiWidget>();
            _diagram.RegisterModelComponent<IyilestirmeCalismasiNode, IyilestirmeCalismasiWidget>();
            _diagram.RegisterModelComponent<KanbanKutusuNode, KanbanKutusuWidget>();
            _diagram.RegisterModelComponent<KontrolluParcaStoguNode, KontrolluParcaStoguWidget>();
            _diagram.RegisterModelComponent<KonveyorNode, KonveyorWidget>();
            _diagram.RegisterModelComponent<LojistikSevkiyatNode, LojistikSevkiyatWidget>();
            _diagram.RegisterModelComponent<OperatorNode, OperatorWidget>();
            _diagram.RegisterModelComponent<SinyalKanbaniNode, SinyalKanbaniWidget>();
            _diagram.RegisterModelComponent<UretimKanbaniNode, UretimKanbaniWidget>();
            _diagram.RegisterModelComponent<YukSeviyelendirmeNode, YukSeviyelendirmeWidget>();
            #endregion
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
            NodeModel node = new NodeModel(position);
            switch (_draggedType)
            {
                case 1:
                    node = new BilgiKutusuNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 2:
                    node = new StokIsaretiNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 3:
                    node = new OperatorNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 4:
                    node = new ItmeHareketiNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 5:
                    node = new EmniyetStoguNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 6:
                    node = new KontrolluParcaStoguNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 7:
                    node = new LojistikSevkiyatNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 8:
                    node = new FifoTransferiNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 9:
                    node = new DisKaynaklarNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 10:
                    node = new BitmisUrunNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 11:
                    node = new CekmeKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 12:
                    node = new IyilestirmeCalismasiNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 13:
                    node = new KanbanKutusuNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 14:
                    node = new KonveyorNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 15:
                    node = new SinyalKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 16:
                    node = new UretimKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                case 17:
                    node = new YukSeviyelendirmeNode() { Position = position, X = position.X, Y = position.Y };
                    break;
                default:
                    break;
            }

            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            _diagram.Nodes.Add(node);
            _draggedType = null;


        }

        private void OnDragEnd(DragEventArgs e)
        {

            _diagram.Nodes.ToList()[0].Position = new Blazor.Diagrams.Core.Geometry.Point(e.ClientX, e.ClientY);
        }

        void Tikla()
        {

            string serializedString = JsonConvert.SerializeObject(_diagram.Nodes.ToList(), Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var serializedNodeList = JsonConvert.DeserializeObject<List<SerializedNode>>(serializedString, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Auto });

            _diagram.Nodes.Clear();

            foreach (var item in serializedNodeList)
            {
                Blazor.Diagrams.Core.Geometry.Point position = new Blazor.Diagrams.Core.Geometry.Point(item.X, item.Y);
                switch (item.ClassName)
                {
                    case "BilgiKutusuNode":
                        BilgiKutusuNode bilgiKutusuNode = new BilgiKutusuNode() { Position = position, X = position.X, Y = position.Y };
                        bilgiKutusuNode.ClassName = item.ClassName;
                        bilgiKutusuNode.CevrimZamani = item.CevrimZamani;
                        bilgiKutusuNode.Departman = item.Departman;
                        bilgiKutusuNode.FireOrani = item.FireOrani;
                        bilgiKutusuNode.IslemeZamani = item.IslemeZamani;
                        bilgiKutusuNode.Istasyon = item.Istasyon;
                        bilgiKutusuNode.VardiyaSayisi = item.VardiyaSayisi;
                        bilgiKutusuNode.X = item.X;
                        bilgiKutusuNode.Y = item.Y;
                        _diagram.Nodes.Add(bilgiKutusuNode);
                        break;
                    case "BitmisUrunNode":
                        BitmisUrunNode bitmisUrunNode = new BitmisUrunNode() { Position = position, X = position.X, Y = position.Y };
                        bitmisUrunNode.ClassName = item.ClassName;
                        bitmisUrunNode.X = item.X;
                        bitmisUrunNode.Y = item.Y;
                        _diagram.Nodes.Add(bitmisUrunNode);
                        break;
                    case "CekmeKanbaniNode":
                        CekmeKanbaniNode cekmeKanbaniNode = new CekmeKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                        cekmeKanbaniNode.ClassName = item.ClassName;
                        cekmeKanbaniNode.X = item.X;
                        cekmeKanbaniNode.Y = item.Y;
                        _diagram.Nodes.Add(cekmeKanbaniNode);
                        break;
                    case "DisKaynaklarNode":
                        DisKaynaklarNode disKaynaklarNode = new DisKaynaklarNode() { Position = position, X = position.X, Y = position.Y };
                        disKaynaklarNode.ClassName = item.ClassName;
                        disKaynaklarNode.X = item.X;
                        disKaynaklarNode.Y = item.Y;
                        _diagram.Nodes.Add(disKaynaklarNode);
                        break;
                    case "EmniyetStoguNode":
                        EmniyetStoguNode emniyetStoguNode = new EmniyetStoguNode() { Position = position, X = position.X, Y = position.Y };
                        emniyetStoguNode.ClassName = item.ClassName;
                        emniyetStoguNode.X = item.X;
                        emniyetStoguNode.Y = item.Y;
                        _diagram.Nodes.Add(emniyetStoguNode);
                        break;
                    case "FifoTransferiNode":
                        FifoTransferiNode fifoTransferiNode = new FifoTransferiNode() { Position = position, X = position.X, Y = position.Y };
                        fifoTransferiNode.ClassName = item.ClassName;
                        fifoTransferiNode.X = item.X;
                        fifoTransferiNode.Y = item.Y;
                        _diagram.Nodes.Add(fifoTransferiNode);
                        break;
                    case "ItmeHareketiNode":
                        ItmeHareketiNode itmeHareketiNode = new ItmeHareketiNode() { Position = position, X = position.X, Y = position.Y };
                        itmeHareketiNode.ClassName = item.ClassName;
                        itmeHareketiNode.X = item.X;
                        itmeHareketiNode.Y = item.Y;
                        _diagram.Nodes.Add(itmeHareketiNode);
                        break;
                    case "IyilestirmeCalismasiNode":
                        IyilestirmeCalismasiNode iyilestirmeCalismasiNode = new IyilestirmeCalismasiNode() { Position = position, X = position.X, Y = position.Y };
                        iyilestirmeCalismasiNode.ClassName = item.ClassName;
                        iyilestirmeCalismasiNode.X = item.X;
                        iyilestirmeCalismasiNode.Y = item.Y;
                        _diagram.Nodes.Add(iyilestirmeCalismasiNode);
                        break;
                    case "KanbanKutusuNode":
                        KanbanKutusuNode kanbanKutusuNode = new KanbanKutusuNode() { Position = position, X = position.X, Y = position.Y };
                        kanbanKutusuNode.ClassName = item.ClassName;
                        kanbanKutusuNode.X = item.X;
                        kanbanKutusuNode.Y = item.Y;
                        _diagram.Nodes.Add(kanbanKutusuNode);
                        break;
                    case "KontrolluParcaStoguNode":
                        KontrolluParcaStoguNode kontrolluParcaStoguNode = new KontrolluParcaStoguNode() { Position = position, X = position.X, Y = position.Y };
                        kontrolluParcaStoguNode.ClassName = item.ClassName;
                        kontrolluParcaStoguNode.X = item.X;
                        kontrolluParcaStoguNode.Y = item.Y;
                        _diagram.Nodes.Add(kontrolluParcaStoguNode);
                        break;
                    case "KonveyorNode":
                        KonveyorNode konveyorNode = new KonveyorNode() { Position = position, X = position.X, Y = position.Y };
                        konveyorNode.ClassName = item.ClassName;
                        konveyorNode.X = item.X;
                        konveyorNode.Y = item.Y;
                        _diagram.Nodes.Add(konveyorNode);
                        break;
                    case "LojistikSevkiyatNode":
                        LojistikSevkiyatNode lojistikSevkiyatNode = new LojistikSevkiyatNode() { Position = position, X = position.X, Y = position.Y };
                        lojistikSevkiyatNode.ClassName = item.ClassName;
                        lojistikSevkiyatNode.X = item.X;
                        lojistikSevkiyatNode.Y = item.Y;
                        _diagram.Nodes.Add(lojistikSevkiyatNode);
                        break;
                    case "OperatorNode":
                        OperatorNode operatorNode = new OperatorNode() { Position = position, X = position.X, Y = position.Y };
                        operatorNode.ClassName = item.ClassName;
                        operatorNode.Sayi = item.Sayi;
                        operatorNode.X = item.X;
                        operatorNode.Y = item.Y;
                        _diagram.Nodes.Add(operatorNode);
                        break;
                    case "SinyalKanbaniNode":
                        SinyalKanbaniNode sinyalKanbaniNode = new SinyalKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                        sinyalKanbaniNode.ClassName = item.ClassName;
                        sinyalKanbaniNode.X = item.X;
                        sinyalKanbaniNode.Y = item.Y;
                        _diagram.Nodes.Add(sinyalKanbaniNode);
                        break;
                    case "StokIsaretiNode":
                        StokIsaretiNode stokIsaretiNode = new StokIsaretiNode() { Position = position, X = position.X, Y = position.Y };
                        stokIsaretiNode.ClassName = item.ClassName;
                        stokIsaretiNode.Adet = item.Adet;
                        stokIsaretiNode.Gun = item.Gun;
                        stokIsaretiNode.X = item.X;
                        stokIsaretiNode.Y = item.Y;
                        _diagram.Nodes.Add(stokIsaretiNode);
                        break;
                    case "UretimKanbaniNode":
                        UretimKanbaniNode uretimKanbaniNode = new UretimKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                        uretimKanbaniNode.ClassName = item.ClassName;
                        uretimKanbaniNode.X = item.X;
                        uretimKanbaniNode.Y = item.Y;
                        _diagram.Nodes.Add(uretimKanbaniNode);
                        break;
                    case "YukSeviyelendirmeNode":
                        YukSeviyelendirmeNode yukSeviyelendirmeNode = new YukSeviyelendirmeNode() { Position = position, X = position.X, Y = position.Y };
                        yukSeviyelendirmeNode.ClassName = item.ClassName;
                        yukSeviyelendirmeNode.X = item.X;
                        yukSeviyelendirmeNode.Y = item.Y;
                        _diagram.Nodes.Add(yukSeviyelendirmeNode);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
