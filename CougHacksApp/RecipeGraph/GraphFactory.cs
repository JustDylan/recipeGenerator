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
using RecipeQueryEngine;
using System.Runtime.CompilerServices;

namespace CougHacksApp.RecipeGraph
{
    internal static class GraphFactory
    {
        private static List<Recipe>[] recipeLevels =
            new List<Recipe>[4];

        private static GraphViewModel CreateGraph(List<Recipe> recipes)
        {
            Graph organizedGraph = new Graph();

            recipes.Sort((x, y) => { return x.FoodItems.Count - y.FoodItems.Count; });
            int layerSize = recipes.Count();

            for (int i = 0; i < recipeLevels.Length; ++i)
            {
                List<Recipe> level = new List<Recipe>();
                recipeLevels[i] = level;

                for (int j = 0; j < layerSize; ++j)
                {
                    if (recipes.Count != 0)
                    {
                        level.Add(recipes.Last());
                        recipes.RemoveAt(recipes.Count-1);
                    }
                }
            }

            return new GraphViewModel(organizedGraph, new List<Node> ());
            
        }
    }
}
