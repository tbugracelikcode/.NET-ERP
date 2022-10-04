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
using TsiErp.VsmBuilder.Entities;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using System.Collections.Immutable;
using System.Security.Cryptography;
using TsiErp.VsmBuilder.Components;
using Blazor.Diagrams.Core.Models.Base;
using System.Threading.Tasks;
using System.Reflection;

namespace TsiErp.VsmBuilder.VSM
{

    public partial class VsmDesigner
    {
        private int? _draggedType;
        private int? nodeID;
        private readonly Diagram _diagram = new Diagram();
        public List<ProductGroup> productGroupList = new List<ProductGroup>()
        {
            new ProductGroup(){ GroupID = 1, Name = "Viraj Rotu"},
            new ProductGroup(){ GroupID = 2, Name = "Rot Mili"},
            new ProductGroup(){ GroupID = 3, Name = "Oynar Burç"},
            new ProductGroup(){ GroupID = 4, Name = "Rotil"},
            new ProductGroup(){ GroupID = 5, Name = "Aşık"}
        };
        public List<Product> productList = new List<Product>()
        {
            new Product(){ ProductID = 1, GroupID = 1, Name = "3389"},
            new Product(){ ProductID = 2, GroupID = 1, Name = "8809-1"},
            new Product(){ ProductID = 3, GroupID = 1, Name = "3346"},
            new Product(){ ProductID = 4, GroupID = 2, Name = "2256"},
            new Product(){ ProductID = 5, GroupID = 2, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 2, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 3, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 3, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 4, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 4, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 5, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 5, Name = "3369-2"},
            new Product(){ ProductID = 5, GroupID = 5, Name = "3369-2"}
        };
        public List<SerializedNode> globalList = new List<SerializedNode>();
        public List<SerializedNode> serializedNodeList = new List<SerializedNode>();
        public List<SerializedNode> modalData = new List<SerializedNode>();
        public NodeModel selectedNode = new NodeModel();
        private Dialog.Category category = new Dialog.Category();
        private string message;
        private bool DialogIsOpen = false;

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
            _diagram.RegisterModelComponent<ZamanCizgisiNode, ZamanCizgisiWidget>();

            #endregion
            _diagram.Options.DeleteKey = "Delete butonunu manuel kontrol etmek için düzenlendi.";
            _diagram.KeyDown += _diagram_KeyDown;
        }
        private void _diagram_KeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Delete")
            {
                category = Dialog.Category.DeleteNot;
                message = "Bu nesneyi silmek istediğinize emin misiniz ?";
                selectedNode = _diagram.Nodes.Where(t => t.Selected == true).FirstOrDefault();
                DialogIsOpen = true;
                StateHasChanged();
            }
        }
        private void OpenDialog()
        {
            if (globalList.Where(t => t.ClassName == "BilgiKutusuNode").Count() > 0)
            {
                category = Dialog.Category.SaveNot;
                message = "Seçtiğiniz nesneyi bir bilgi kutusu ile eşleştiriniz.";
                globalList = GetCurrentNodes(true);
                string nodeClass = globalList.Where(t => t.nodeID == selectedNode.Id).Select(t => t.ClassName).FirstOrDefault();
                switch (nodeClass)
                {
                    case "StokIsaretiNode":
                        modalData = globalList.Where(t => t.ClassName == "BilgiKutusuNode" && string.IsNullOrEmpty(t.StokIsaretiNodeID)).ToList();
                        break;
                    case "OperatorNode":
                        modalData = globalList.Where(t => t.ClassName == "BilgiKutusuNode" && string.IsNullOrEmpty(t.OperatorNodeID)).ToList();
                        break;
                    case "ZamanCizgisi":
                        modalData = globalList.Where(t => t.ClassName == "BilgiKutusuNode" && string.IsNullOrEmpty(t.ZamanCizgisiNodeID)).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                category = Dialog.Category.Okay;
                message = "Bu nesneyi eklemeden önce diagrama bir Bilgi Kutusu ekleyiniz.";
            }
            DialogIsOpen = true;
        }
        private void OnDialogClose(bool ok)
        {
            if (category == Dialog.Category.SaveNot && !ok)
            {
                globalList.RemoveAt(globalList.Count - 1);
                _diagram.Nodes.Remove(_diagram.Nodes.Last());
            }
            else if (category == Dialog.Category.SaveNot && ok)
            {
                
            }
            if (category == Dialog.Category.DeleteNot && ok)
            {
                _diagram.Nodes.Remove(selectedNode);
                globalList.Remove(globalList.Where(t => t.nodeID == selectedNode.Id).FirstOrDefault());
            }
            DialogIsOpen = false;
            StateHasChanged();
        }
        private int BilgiKutusuSayisiHesapla()
        {
            int sayi = globalList.Where(t => t.ClassName == "BilgiKutusuNode").Count();
            return sayi;

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
            SerializedNode globalNode;
            switch (_draggedType)
            {
                case 1:
                    node = new BilgiKutusuNode() { Position = position, X = position.X, Y = position.Y, Name = "Bilgi Kutusu " + BilgiKutusuSayisiHesapla().ToString() };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.Name = "Bilgi Kutusu " + BilgiKutusuSayisiHesapla().ToString();
                    globalNode.ClassName = "BilgiKutusuNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    selectedNode = node;
                    globalList.Add(globalNode);
                    break;
                case 2:
                    if (BilgiKutusuSayisiHesapla() > 0)
                    {
                        node = new StokIsaretiNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode.ClassName = "StokIsaretiNode";
                        globalNode.nodeID = node.Id;
                        _diagram.Nodes.Add(node);
                        selectedNode = node;
                        globalList.Add(globalNode);
                        OpenDialog();
                    }
                    else { node = null; OpenDialog(); }
                    break;
                case 3:
                    if (BilgiKutusuSayisiHesapla() > 0)
                    {
                        node = new OperatorNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode.ClassName = "OperatorNode";
                        globalNode.nodeID = node.Id;
                        _diagram.Nodes.Add(node);
                        selectedNode = node;
                        globalList.Add(globalNode);
                        OpenDialog();
                    }
                    else { node = null; OpenDialog(); }
                    break;
                case 4:
                    node = new ItmeHareketiNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "ItmeHareketiNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 5:
                    node = new EmniyetStoguNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "EmniyetStoguNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 6:
                    node = new KontrolluParcaStoguNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "KontrolluParcaStoguNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 7:
                    node = new LojistikSevkiyatNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "LojistikSevkiyatNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 8:
                    node = new FifoTransferiNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "FifoTransferiNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 9:
                    node = new DisKaynaklarNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "DisKaynaklarNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 10:
                    node = new BitmisUrunNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "BitmisUrunNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 11:
                    node = new CekmeKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "CekmeKanbaniNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 12:
                    node = new IyilestirmeCalismasiNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "IyilestirmeCalismasiNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 13:
                    node = new KanbanKutusuNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "KanbanKutusuNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 14:
                    node = new KonveyorNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "KonveyorNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 15:
                    node = new SinyalKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "SinyalKanbaniNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 16:
                    node = new UretimKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "UretimKanbaniNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 17:
                    node = new YukSeviyelendirmeNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                    globalNode.ClassName = "YukSeviyelendirmeNode";
                    globalNode.nodeID = node.Id;
                    _diagram.Nodes.Add(node);
                    globalList.Add(globalNode);
                    break;
                case 18:
                    if (BilgiKutusuSayisiHesapla() > 0)
                    {
                        node = new ZamanCizgisiNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                        globalNode.ClassName = "ZamanCizgisiNode";
                        globalNode.nodeID = node.Id;
                        _diagram.Nodes.Add(node);
                        selectedNode = node;
                        globalList.Add(globalNode);
                        OpenDialog();
                    }
                    else { node = null; OpenDialog(); }
                    break;
                default:
                    break;
            }
            if (node != null)
            {
                node.AddPort(PortAlignment.Top);
                node.AddPort(PortAlignment.Bottom);

            }
            else { node = null; }
            _draggedType = null;
        }

        public List<SerializedNode> GetCurrentNodes(bool reload)
        {
            string serializedString = JsonConvert.SerializeObject(_diagram.Nodes.ToList(), Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var serializedNodeList = JsonConvert.DeserializeObject<List<SerializedNode>>(serializedString, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Auto });

            if (!reload)
            {
                return serializedNodeList;
            }
            else
            {
                var nodeList = _diagram.Nodes.ToList();
                _diagram.Nodes.Clear();
                globalList.Clear();

                int i = 0;

                foreach (var item in serializedNodeList)
                {
                    Blazor.Diagrams.Core.Geometry.Point position = nodeList[i].Position;
                    SerializedNode globalNode;

                    switch (item.ClassName)
                    {
                        case "BilgiKutusuNode":
                            BilgiKutusuNode bilgiKutusuNode = new BilgiKutusuNode() { Position = position, X = position.X, Y = position.Y };
                            bilgiKutusuNode.Name = item.Name;
                            bilgiKutusuNode.ClassName = item.ClassName;
                            bilgiKutusuNode.CevrimZamani = item.CevrimZamani;
                            bilgiKutusuNode.Departman = item.Departman;
                            bilgiKutusuNode.FireOrani = item.FireOrani;
                            bilgiKutusuNode.IslemeZamani = item.IslemeZamani;
                            bilgiKutusuNode.Istasyon = item.Istasyon;
                            bilgiKutusuNode.VardiyaSayisi = item.VardiyaSayisi;
                            bilgiKutusuNode.X = position.X;
                            bilgiKutusuNode.Y = position.Y;
                            bilgiKutusuNode.ParentID = item.ParentID;
                            bilgiKutusuNode.nodeID = nodeList[i].Id;
                            bilgiKutusuNode.OperatorNodeID = item.OperatorNodeID;
                            bilgiKutusuNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            bilgiKutusuNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            _diagram.Nodes.Add(bilgiKutusuNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.Name = item.Name;
                            globalNode.ClassName = item.ClassName;
                            globalNode.CevrimZamani = item.CevrimZamani;
                            globalNode.Departman = item.Departman;
                            globalNode.FireOrani = item.FireOrani;
                            globalNode.IslemeZamani = item.IslemeZamani;
                            globalNode.Istasyon = item.Istasyon;
                            globalNode.VardiyaSayisi = item.VardiyaSayisi;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalNode.ParentID = item.ParentID;
                            globalNode.nodeID = nodeList[i].Id;
                            globalNode.OperatorNodeID = item.OperatorNodeID;
                            globalNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            globalNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            globalList.Add(globalNode);
                            break;
                        case "BitmisUrunNode":
                            BitmisUrunNode bitmisUrunNode = new BitmisUrunNode() { Position = position, X = position.X, Y = position.Y };
                            bitmisUrunNode.ClassName = item.ClassName;
                            bitmisUrunNode.X = position.X;
                            bitmisUrunNode.Y = position.Y;
                            _diagram.Nodes.Add(bitmisUrunNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "CekmeKanbaniNode":
                            CekmeKanbaniNode cekmeKanbaniNode = new CekmeKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                            cekmeKanbaniNode.ClassName = item.ClassName;
                            cekmeKanbaniNode.X = position.X;
                            cekmeKanbaniNode.Y = position.Y;
                            _diagram.Nodes.Add(cekmeKanbaniNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "DisKaynaklarNode":
                            DisKaynaklarNode disKaynaklarNode = new DisKaynaklarNode() { Position = position, X = position.X, Y = position.Y };
                            disKaynaklarNode.ClassName = item.ClassName;
                            disKaynaklarNode.X = position.X;
                            disKaynaklarNode.Y = position.Y;
                            _diagram.Nodes.Add(disKaynaklarNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "EmniyetStoguNode":
                            EmniyetStoguNode emniyetStoguNode = new EmniyetStoguNode() { Position = position, X = position.X, Y = position.Y };
                            emniyetStoguNode.ClassName = item.ClassName;
                            emniyetStoguNode.X = position.X;
                            emniyetStoguNode.Y = position.Y;
                            _diagram.Nodes.Add(emniyetStoguNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "FifoTransferiNode":
                            FifoTransferiNode fifoTransferiNode = new FifoTransferiNode() { Position = position, X = position.X, Y = position.Y };
                            fifoTransferiNode.ClassName = item.ClassName;
                            fifoTransferiNode.X = position.X;
                            fifoTransferiNode.Y = position.Y;
                            _diagram.Nodes.Add(fifoTransferiNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "ItmeHareketiNode":
                            ItmeHareketiNode itmeHareketiNode = new ItmeHareketiNode() { Position = position, X = position.X, Y = position.Y };
                            itmeHareketiNode.ClassName = item.ClassName;
                            itmeHareketiNode.X = position.X;
                            itmeHareketiNode.Y = position.Y;
                            _diagram.Nodes.Add(itmeHareketiNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "IyilestirmeCalismasiNode":
                            IyilestirmeCalismasiNode iyilestirmeCalismasiNode = new IyilestirmeCalismasiNode() { Position = position, X = position.X, Y = position.Y };
                            iyilestirmeCalismasiNode.ClassName = item.ClassName;
                            iyilestirmeCalismasiNode.X = position.X;
                            iyilestirmeCalismasiNode.Y = position.Y;
                            _diagram.Nodes.Add(iyilestirmeCalismasiNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "KanbanKutusuNode":
                            KanbanKutusuNode kanbanKutusuNode = new KanbanKutusuNode() { Position = position, X = position.X, Y = position.Y };
                            kanbanKutusuNode.ClassName = item.ClassName;
                            kanbanKutusuNode.X = position.X;
                            kanbanKutusuNode.Y = position.Y;
                            _diagram.Nodes.Add(kanbanKutusuNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "KontrolluParcaStoguNode":
                            KontrolluParcaStoguNode kontrolluParcaStoguNode = new KontrolluParcaStoguNode() { Position = position, X = position.X, Y = position.Y };
                            kontrolluParcaStoguNode.ClassName = item.ClassName;
                            kontrolluParcaStoguNode.X = position.X;
                            kontrolluParcaStoguNode.Y = position.Y;
                            _diagram.Nodes.Add(kontrolluParcaStoguNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "KonveyorNode":
                            KonveyorNode konveyorNode = new KonveyorNode() { Position = position, X = position.X, Y = position.Y };
                            konveyorNode.ClassName = item.ClassName;
                            konveyorNode.X = position.X;
                            konveyorNode.Y = position.Y;
                            _diagram.Nodes.Add(konveyorNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "LojistikSevkiyatNode":
                            LojistikSevkiyatNode lojistikSevkiyatNode = new LojistikSevkiyatNode() { Position = position, X = position.X, Y = position.Y };
                            lojistikSevkiyatNode.ClassName = item.ClassName;
                            lojistikSevkiyatNode.X = position.X;
                            lojistikSevkiyatNode.Y = position.Y;
                            _diagram.Nodes.Add(lojistikSevkiyatNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "OperatorNode":
                            OperatorNode operatorNode = new OperatorNode() { Position = position, X = position.X, Y = position.Y };
                            operatorNode.ClassName = item.ClassName;
                            operatorNode.Sayi = item.Sayi;
                            operatorNode.X = position.X;
                            operatorNode.Y = position.Y;
                            operatorNode.ParentID = item.ParentID;
                            operatorNode.nodeID = nodeList[i].Id;
                            _diagram.Nodes.Add(operatorNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.Sayi = item.Sayi;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalNode.ParentID = item.ParentID;
                            globalNode.nodeID = nodeList[i].Id;
                            globalList.Add(globalNode);
                            break;
                        case "SinyalKanbaniNode":
                            SinyalKanbaniNode sinyalKanbaniNode = new SinyalKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                            sinyalKanbaniNode.ClassName = item.ClassName;
                            sinyalKanbaniNode.X = position.X;
                            sinyalKanbaniNode.Y = position.Y;
                            _diagram.Nodes.Add(sinyalKanbaniNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "StokIsaretiNode":
                            StokIsaretiNode stokIsaretiNode = new StokIsaretiNode() { Position = position, X = position.X, Y = position.Y };
                            stokIsaretiNode.ClassName = item.ClassName;
                            stokIsaretiNode.Adet = item.Adet;
                            stokIsaretiNode.Gun = item.Gun;
                            stokIsaretiNode.X = position.X;
                            stokIsaretiNode.Y = position.Y;
                            stokIsaretiNode.nodeID = nodeList[i].Id;
                            stokIsaretiNode.ParentID = item.ParentID;
                            stokIsaretiNode.OperatorNodeID = item.OperatorNodeID;
                            stokIsaretiNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            stokIsaretiNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            _diagram.Nodes.Add(stokIsaretiNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.Adet = item.Adet;
                            globalNode.Gun = item.Gun;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalNode.nodeID = nodeList[i].Id;
                            globalNode.ParentID = item.ParentID;
                            globalNode.OperatorNodeID = item.OperatorNodeID;
                            globalNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            globalNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            globalList.Add(globalNode);
                            break;
                        case "UretimKanbaniNode":
                            UretimKanbaniNode uretimKanbaniNode = new UretimKanbaniNode() { Position = position, X = position.X, Y = position.Y };
                            uretimKanbaniNode.ClassName = item.ClassName;
                            uretimKanbaniNode.X = position.X;
                            uretimKanbaniNode.Y = position.Y;
                            _diagram.Nodes.Add(uretimKanbaniNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "YukSeviyelendirmeNode":
                            YukSeviyelendirmeNode yukSeviyelendirmeNode = new YukSeviyelendirmeNode() { Position = position, X = position.X, Y = position.Y };
                            yukSeviyelendirmeNode.ClassName = item.ClassName;
                            yukSeviyelendirmeNode.X = position.X;
                            yukSeviyelendirmeNode.Y = position.Y;
                            _diagram.Nodes.Add(yukSeviyelendirmeNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalList.Add(globalNode);
                            break;
                        case "ZamanCizgisiNode":
                            ZamanCizgisiNode zamanCizgisiNode = new ZamanCizgisiNode() { Position = position, X = position.X, Y = position.Y };
                            zamanCizgisiNode.ClassName = item.ClassName;
                            zamanCizgisiNode.X = position.X;
                            zamanCizgisiNode.Y = position.Y;
                            zamanCizgisiNode.nodeID = nodeList[i].Id;
                            zamanCizgisiNode.ParentID = item.ParentID;
                            zamanCizgisiNode.OperatorNodeID = item.OperatorNodeID;
                            zamanCizgisiNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            zamanCizgisiNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            _diagram.Nodes.Add(zamanCizgisiNode);

                            globalNode = new SerializedNode() { Position = position, X = position.X, Y = position.Y };
                            globalNode.ClassName = item.ClassName;
                            globalNode.X = position.X;
                            globalNode.Y = position.Y;
                            globalNode.nodeID = nodeList[i].Id;
                            globalNode.ParentID = item.ParentID;
                            globalNode.OperatorNodeID = item.OperatorNodeID;
                            globalNode.StokIsaretiNodeID = item.StokIsaretiNodeID;
                            globalNode.ZamanCizgisiNodeID = item.ZamanCizgisiNodeID;
                            globalList.Add(globalNode);
                            break;
                        default:
                            break;
                    }
                    i++;
                }

                return globalList;

            }
        }
    }
}
