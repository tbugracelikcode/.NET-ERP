using Syncfusion.Blazor.Diagram.SymbolPalette;
using Syncfusion.Blazor.Diagram;
using System.Collections.ObjectModel;
using DevExpress.Utils;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
using TsiErp.DashboardUI.Data;
using System.Text.Json;

namespace TsiErp.DashboardUI.Pages.Vsm
{
    public partial class VsmDesigner
    {
        #region Diagram
        private int connectorCount = 0;
        DiagramSelectionSettings selectionSettings = new DiagramSelectionSettings();

        public DiagramSize SymbolPreview;

        public SymbolMargin SymbolMargin = new SymbolMargin { Left = 15, Right = 15, Top = 15, Bottom = 15 };

        public SfDiagramComponent Diagram;

        public SfSymbolPaletteComponent PaletteInstance;

        DiagramObjectCollection<UserHandle> handles = new DiagramObjectCollection<UserHandle>();

        //Defines Diagram's nodes collection
        private DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();

        //Defines Diagram's connectors collection
        private DiagramObjectCollection<Connector> connectors = new DiagramObjectCollection<Connector>();

        //Define palettes collection
        private DiagramObjectCollection<Palette> palettes = new DiagramObjectCollection<Palette>();

        // Defines palette's flow-shape collection
        private DiagramObjectCollection<NodeBase> ShapeSymbols = new DiagramObjectCollection<NodeBase>();

        // Defines interval values for GridLines
        public double[] GridLineIntervals { get; set; }

        // Defines palette's connector collection
        private DiagramObjectCollection<NodeBase> connectorSymbols = new DiagramObjectCollection<NodeBase>();

        public async void SaveDiagramClick()
        {
            //string data = Diagram.SaveDiagram();

            //await Diagram.LoadDiagram(data);

            SaveDiagram();
        }
        string nodesJson = "";

        public void SaveDiagram()
        {
            nodesJson = new string(JsonSerializer.Serialize(nodes.Select(i => (Node)i.Clone())));
        }

        //public async Task DiagramLoadJson(string diagramId = "")
        //{
        //    nodes.Clear();
        //    nodes = new DiagramObjectCollection(JsonSerializer.Deserialize(nodesJson));
        //    foreach (Node vnode in nodes)
        //    {
        //        var origAnnotation = new DiagramObjectCollection(vnode.Annotations.Select(i => (ShapeAnnotation)i.Clone()));
        //        vnode.Annotations.Clear();
        //        vnode.Annotations = new DiagramObjectCollection(origAnnotation);
        //    }
        //}

        public void OnDrop(DropEventArgs args)
        {
            args.Cancel = true;
            var NewNode = args.Element as Node;
            //var annot = NewNode.Annotations.ElementAt(0).Content;
            var nodeInfo = NewNode.AdditionalInfo.Where(t => t.Key == "nodeInfo").Select(t => t.Value).FirstOrDefault();
            switch (nodeInfo)
            {
                case "1":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 179,
                        Height = 182,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML,
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "2":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 100,
                        Height = 100,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "3":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 80,
                        Height = 30,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "4":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 119,
                        Height = 100,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "5":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 35,
                        Height = 80,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "6":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 35,
                        Height = 80,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "7":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 82,
                        Height = 80,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo
                    });
                    break;
                case "8":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 132,
                        Height = 72,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "9":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 132,
                        Height = 72,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "10":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 132,
                        Height = 72,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "11":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 100,
                        Height = 75,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "12":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 100,
                        Height = 75,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "13":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 62,
                        Height = 72,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "14":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 93,
                        Height = 48,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "15":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 97,
                        Height = 37,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "16":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 93,
                        Height = 48,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;
                case "17":
                    nodes.Add(new Node()
                    {
                        OffsetX = NewNode.OffsetX,
                        OffsetY = NewNode.OffsetY,
                        Width = 93,
                        Height = 48,
                        Annotations = NewNode.Annotations,
                        Shape = new Syncfusion.Blazor.Diagram.Shape()
                        {
                            Type = NodeShapes.HTML
                        },
                        AdditionalInfo = NewNode.AdditionalInfo



                    });
                    break;

                default:
                    break;
            }
        }

        private void OnSelectionChanged(SelectionChangedEventArgs args)
        {
            if (args.NewValue.Count > 0 && args.NewValue[0] is Node)
            {
                Diagram.SelectionSettings.Constraints = Diagram.SelectionSettings.Constraints | SelectorConstraints.UserHandle;
            }
            else if (args.NewValue.Count > 0)
            {
                Diagram.SelectionSettings.Constraints = Diagram.SelectionSettings.Constraints & ~SelectorConstraints.UserHandle;
            }
        }

        private void OnCreated()
        {
            if (Diagram.Nodes.Count > 0)
                Diagram.Select(new ObservableCollection<IDiagramObject>() { Diagram.Nodes[0] });
        }
        // Method to customize the tool
        public InteractionControllerBase GetCustomTool(DiagramElementAction action, string id)
        {
            InteractionControllerBase tool = null;
            if (id == "Draw")
            {
                tool = new DrawTool(Diagram);
            }
            else
            {
                tool = new AddDeleteTool(Diagram);
            }
            return tool;
        }
        // Custom tool to delete the node.
        public class AddDeleteTool : DragController
        {
            SfDiagramComponent sfDiagram;
            Node deleteObject = null;
            public AddDeleteTool(SfDiagramComponent Diagram) : base(Diagram)
            {
                sfDiagram = Diagram;
            }
            public override void OnMouseDown(DiagramMouseEventArgs args)
            {
                if (sfDiagram.SelectionSettings.Nodes.Count > 0 && ((sfDiagram.SelectionSettings.Nodes[0]) is Node))
                {
                    deleteObject = (sfDiagram.SelectionSettings.Nodes[0]) as Node;
                }
            }
            public override void OnMouseUp(DiagramMouseEventArgs args)
            {
                if (deleteObject != null)
                {
                    sfDiagram.BeginUpdate();
                    sfDiagram.Nodes.Remove(deleteObject);
                    _ = sfDiagram.EndUpdate();
                }
                base.OnMouseUp(args);
                this.InAction = true;
            }
        }

        public class DrawTool : ConnectorDrawingController
        {
            SfDiagramComponent sfDiagram;
            Connector newConnector = null;
            public DrawTool(SfDiagramComponent Diagram) : base(Diagram, DiagramElementAction.ConnectorSourceEnd)
            {
                sfDiagram = Diagram;
                newConnector = new Connector()
                {
                    ID = "OrthogonalConnector",
                    SourceID = sfDiagram.SelectionSettings.Nodes[0].ID,
                    Type = ConnectorSegmentType.Orthogonal,
                };
#pragma warning disable BL0005
                Diagram.InteractionController = DiagramInteractions.DrawOnce;
                Diagram.DrawingObject = newConnector;
#pragma warning restore BL0005
            }
            public override void OnMouseDown(DiagramMouseEventArgs args)
            {
                base.OnMouseDown(args);
            }
            public override void OnMouseUp(DiagramMouseEventArgs args)
            {
                base.OnMouseUp(args);
            }
        }

        private SymbolInfo GetSymbolInfo(IDiagramObject symbol)
        {
            if (symbol is Node)
            {
                SymbolInfo SymbolInfo = new SymbolInfo();
                string text = null;
                text = (symbol as Node).AdditionalInfo.Where(t => t.Key == "toolTips").Select(t => t.Value).FirstOrDefault().ToString();
                SymbolInfo.Description = new SymbolDescription() { Text = text };
                return SymbolInfo;
            }
            else
            {
                SymbolInfo SymbolInfo = new SymbolInfo();
                string text = null;
                text = (symbol as Connector).AdditionalInfo.Where(t => t.Key == "toolTips").Select(t => t.Value).FirstOrDefault().ToString();
                SymbolInfo.Description = new SymbolDescription() { Text = text };
                return SymbolInfo;
            }
        }

        private void InitPaletteModel()
        {
            palettes = new DiagramObjectCollection<Palette>();

            //SymbolPreview = new DiagramSize
            //{
            //    Width = 150,
            //    Height = 150
            //};

            ShapeSymbols = new DiagramObjectCollection<NodeBase>();

            CreatePaletteNode("images/VSM/Bilgi_Kutusu.png", "1", "1", "Bilgi Kutusu");
            CreatePaletteNode("images/VSM/Operator.png", "2", "2", "Operatör");
            CreatePaletteNode("images/VSM/Itme_Hareketi.png", "3", "3", "İtme Hareketi");
            CreatePaletteNode("images/VSM/Stok_Isareti.png", "4", "4", "Stok İşareti");
            CreatePaletteNode("images/VSM/Emniyet_Stogu.png", "5", "5", "Emniyet Stoğu");
            CreatePaletteNode("images/VSM/Kontrollu_Parca_Stogu.png", "6", "6", "Kontrollü Parça Stoğu");
            CreatePaletteNode("images/VSM/Fifo_Transferi.png", "7", "7", "FİFO Transferi");
            CreatePaletteNode("images/VSM/Lojistik_Sevkiyat.png", "8", "8", "Lojistik/Sevkiyat");
            CreatePaletteNode("images/VSM/Cekme_Kanbani.png", "9", "9", "Çekme Kanbanı");
            CreatePaletteNode("images/VSM/Uretim_Kanbani.png", "10", "10", "Üretim Kanbanı");
            CreatePaletteNode("images/VSM/Sinyal_Kanbani.png", "11", "11", "Sinyal Kanbanı");
            CreatePaletteNode("images/VSM/Dis_Kaynaklar.png", "12", "12", "Dış Kaynaklar");
            CreatePaletteNode("images/VSM/Kanban_Kutusu.png", "13", "13", "Kanban Kutusu");
            CreatePaletteNode("images/VSM/Konveyor.png", "14", "14", "Konveyör");
            CreatePaletteNode("images/VSM/Bitmis_Urun.png", "15", "15", "Bitmiş Ürün");
            CreatePaletteNode("images/VSM/Yuk_Seviyelendirme.png", "16", "16", "Yük Seviyelendirme");
            CreatePaletteNode("images/VSM/Iyilestirme_Calismasi.png", "17", "17", "İyileştirme Çalışması");

            connectorSymbols = new DiagramObjectCollection<NodeBase>();

            CreatePaletteConnector("Link1", ConnectorSegmentType.Orthogonal, DecoratorShape.Arrow, "");
            CreatePaletteConnector("Link2", ConnectorSegmentType.Orthogonal, DecoratorShape.None, "");
            CreatePaletteConnector("Link3", ConnectorSegmentType.Straight, DecoratorShape.Arrow, "");
            CreatePaletteConnector("Link4", ConnectorSegmentType.Straight, DecoratorShape.None, "");
            CreatePaletteConnector("Link5", ConnectorSegmentType.Bezier, DecoratorShape.None, "");

            palettes = new DiagramObjectCollection<Palette>()
            {
                #pragma warning disable BL0005
                new Palette() {Symbols = ShapeSymbols, Title = "VSM Shapes", ID = "VSM Shapes", IconCss = "e-ddb-icons e-flow"},
                new Palette() {Symbols = connectorSymbols, Title = "Connectors", IsExpanded = true, IconCss = "e-ddb-icons e-connector"}
                #pragma warning restore BL0005
            };
        }

        private void CreatePaletteNode(string imgSrc, string id, string tip, string tooltip)
        {
            Dictionary<string, object> NodeInfo = new Dictionary<string, object>();
            NodeInfo.Add("nodeInfo", tip);
            NodeInfo.Add("toolTips", tooltip);

            ShapeAnnotation shapeAnnotation = new ShapeAnnotation() { Content = string.Empty };
            Node diagramNode = new Node()
            {
                ID = id,
                Shape = new ImageShape() { Type = NodeShapes.Image, Source = imgSrc },
                Style = new ShapeStyle() { StrokeColor = "#757575", StrokeWidth = 1 },
                Annotations = new DiagramObjectCollection<ShapeAnnotation>() { shapeAnnotation },
                Width = 150,
                Height = 150,
                AdditionalInfo = NodeInfo
            };

            double oldWidth = Convert.ToDouble(diagramNode.Width);
            double oldHeight = Convert.ToDouble(diagramNode.Height);
            double ratio = 150 / oldWidth;
            diagramNode.Width = 150;
            diagramNode.Height *= ratio;
            ShapeSymbols.Add(diagramNode);
        }

        private void OnNodeCreating(IDiagramObject obj)
        {
            Node node = obj as Node;
            node.Style.Fill = "#357BD2";
            if (!(node.ID.StartsWith("Annotation") || node.ID.StartsWith("Sequential Data")))
                node.Style.StrokeColor = "White";
            node.Style.Opacity = 1;
        }

        private void OnConnectorCreating(IDiagramObject obj)
        {
            Connector node = obj as Connector;
            node.Style.Fill = "black";
            node.Style.StrokeColor = "black";
            node.Style.Opacity = 1;
            node.TargetDecorator.Style.Fill = "black";
            node.TargetDecorator.Style.StrokeColor = "black";
        }

        // used to create a Port.
        private DiagramObjectCollection<PointPort> CreatePort()
        {
            DiagramObjectCollection<PointPort> defaultsPorts = new DiagramObjectCollection<PointPort>();
            PointPort port1 = new PointPort()
            {
                ID = "port1",
                Shape = PortShapes.Circle,
                Offset = new DiagramPoint() { X = 0, Y = 0.5 }
            };
            PointPort port2 = new PointPort()
            {
                ID = "port2",
                Shape = PortShapes.Circle,
                Offset = new DiagramPoint() { X = 0.5, Y = 0 }
            };
            PointPort port3 = new PointPort()
            {
                ID = "port3",
                Shape = PortShapes.Circle,
                Offset = new DiagramPoint() { X = 1, Y = 0.5 }
            };
            PointPort port4 = new PointPort()
            {
                ID = "port4",
                Shape = PortShapes.Circle,
                Offset = new DiagramPoint() { X = 0.5, Y = 1 }
            };
            defaultsPorts.Add(port1);
            defaultsPorts.Add(port2);
            defaultsPorts.Add(port3);
            defaultsPorts.Add(port4);
            return defaultsPorts;
        }

        // Method is used to create a Connector for the diagram.
        private void CreateConnector(string sourceId, string targetId, string label = default(string), bool isDashLine = false, string sport = "", string tport = "")
        {
            Connector diagramConnector = new Connector()
            {
                ID = string.Format("connector{0}", ++connectorCount),
                SourceID = sourceId,
                TargetID = targetId,
                SourcePortID = sport,
                TargetPortID = tport
            };
            if (isDashLine)
            {
                diagramConnector.Style = new ShapeStyle() { StrokeDashArray = "2,2" };
            }
            if (label != default(string))
            {
                var annotation = new PathAnnotation()
                {
                    Content = label,
                    Style = new TextStyle() { Fill = "white" }
                };
                if ((sourceId == "node5" && targetId == "node6") || label == "Yes" || label == "No")
                {
                    annotation.Height = 10;
                    annotation.Width = 15;
                }
                diagramConnector.Annotations = new DiagramObjectCollection<PathAnnotation>() { annotation };
            }
            diagramConnector.Type = ConnectorSegmentType.Orthogonal;
            connectors.Add(diagramConnector);
        }

        // Method is used to create a Connector for the palette.
        private void CreatePaletteConnector(string id, ConnectorSegmentType type, DecoratorShape decoratorShape, string tooltip)
        {
            Dictionary<string, object> NodeInfo = new Dictionary<string, object>();
            NodeInfo.Add("toolTips", tooltip);
            Connector diagramConnector = new Connector()
            {
                ID = id,
                Type = type,
                SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
                TargetPoint = new DiagramPoint() { X = 60, Y = 60 },
                Style = new ShapeStyle() { StrokeWidth = 1, StrokeColor = "#757575" },
                TargetDecorator = new DecoratorSettings()
                {
                    Shape = decoratorShape,
                    Style = new ShapeStyle() { StrokeWidth = 1, StrokeColor = "#757575", Fill = "#757575" }
                },
                AdditionalInfo = NodeInfo
            };
            connectorSymbols.Add(diagramConnector);
        }

        private void UpdateHandle()
        {
            UserHandle deleteHandle = AddHandle("Delete", "delete", Direction.Bottom, 0.5);
            UserHandle drawHandle = AddHandle("Draw", "draw", Direction.Right, 0.5);
            handles.Add(deleteHandle);
            handles.Add(drawHandle);
            selectionSettings.UserHandles = handles;
        }

        private UserHandle AddHandle(string name, string path, Direction direction, double offset)
        {
            UserHandle handle = new UserHandle()
            {
                Name = name,
                Visible = true,
                Offset = offset,
                Side = direction,
                Margin = new DiagramThickness() { Top = 0, Bottom = 0, Left = 0, Right = 0 }
            };
            if (path == "delete")
            {
                handle.PathData = "M0.54700077,2.2130003 L7.2129992,2.2130003 7.2129992,8.8800011 C7.2129992,9.1920013 7.1049975,9.4570007 6.8879985,9.6739998 6.6709994,9.8910007 6.406,10 6.0939997,10 L1.6659999,10 C1.3539997,10 1.0890004,9.8910007 0.87200136,9.6739998 0.65500242,9.4570007 0.54700071,9.1920013 0.54700077,8.8800011 z M2.4999992,0 L5.2600006,0 5.8329986,0.54600048 7.7599996,0.54600048 7.7599996,1.6660004 0,1.6660004 0,0.54600048 1.9270014,0.54600048 z";
            }
            else
            {
                handle.PathData = "M3.9730001,0 L8.9730001,5.0000007 3.9730001,10.000001 3.9730001,7.0090005 0,7.0090005 0,2.9910006 3.9730001,2.9910006 z";
            }
            return handle;
        }
        #endregion


        protected override void OnInitialized()
        {
            GridLineIntervals = new double[] { 1, 9, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75 };
            InitPaletteModel();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                UpdateHandle();
            }
            PaletteInstance.Targets = new DiagramObjectCollection<SfDiagramComponent>
        {
            Diagram
        };
        }

       
    }
}
