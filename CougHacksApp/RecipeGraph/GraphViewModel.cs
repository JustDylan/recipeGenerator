using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CougHacksApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;
//using Microsoft.Msagl;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Color = Microsoft.Msagl.Drawing.Color;
//using LabelPlacement = Microsoft.Msagl.Core.Layout.LabelPlacement;
using ModifierKeys = System.Windows.Input.ModifierKeys;
using Size = System.Windows.Size;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using static Microsoft.Msagl.Core.Layout.LgNodeInfo;
using Orientation = System.Windows.Controls.Orientation;
using CougHacksApp.Model;

namespace CougHacksApp.RecipeGraph
{
    internal class GraphViewModel
    {
        public event EventHandler? GraphNodeChanged;

        private Graph graph;

        public GraphViewModel(Graph graph, List<Node> graphNodes)
        {
            GraphNodeChanged = delegate { };
            this.graph = graph;
            foreach (Node node in graphNodes)
            {
                node.Attr.VisualsChanged += (o, e) => { this.OnGraphNodeChange(node, e); };
            }
        }

        /// <summary>
        /// assigns this graph to the graph viewer
        /// </summary>
        /// <param name="graphViewer"></param>
        public void AssignGraph(GraphViewer graphViewer)
        {
            graphViewer.Graph = this.graph;
        }

        private void OnGraphNodeChange(object? o, EventArgs e)
        {
            if(this.GraphNodeChanged != null &&
                o != null && ((Node)o).Attr.LineWidth == 2)
                this.GraphNodeChanged(((Node)o).UserData, e);
        }
    }
}
