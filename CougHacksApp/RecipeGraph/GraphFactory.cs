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

        private static List<Node>[] nodeLevels =
            new List<Node>[4];

        private static GraphViewModel CreateGraph(List<Recipe> recipes)
        {
            Graph organizedGraph = new Graph();
            List<Node> nodes = new List<Node>();

            recipes.Sort((x, y) => { return x.FoodItems.Count - y.FoodItems.Count; });
            int layerSize = recipes.Count();

            for (int i = 0; i < recipeLevels.Length; ++i)
            {
                List<Node> nodeLevel = new List<Node>();
                List<Recipe> level = new List<Recipe>();
                recipeLevels[i] = level;
                nodeLevels[i] = nodeLevel;

                for (int j = 0; j < layerSize; ++j)
                {
                    // insert recipe and node into level i
                    if (recipes.Count != 0)
                    {
                        Recipe recipe = recipes.Last();
                        Node recipeNode = new Node(recipe.Label);
                        recipeNode.UserData = recipe;
                        nodeLevel.Add(recipeNode);

                        level.Add(recipe);
                        recipes.RemoveAt(recipes.Count-1);
                    }
                }
            }

            for (int i = 1; i < recipeLevels.Length; ++i)
            {
                List<Recipe> levelPrev = recipeLevels[i-1];
                List<Node> nodeLevelPrev = nodeLevels[i-1];
                List<Recipe> levelNext = recipeLevels[i];
                List<Node> nodeLevelNext = nodeLevels[i];

                for (int j = 0; j < levelPrev.Count; ++j)
                {
                    for (int k = 0; k < levelNext.Count; ++k)
                    {
                        if (IsSubsetOf(levelPrev[j], levelNext[k]))
                        {
                            ConnectionToGraph connection = new ConnectionToGraph();
                            organizedGraph.AddPrecalculatedEdge(
                                new Edge(
                                    nodeLevelPrev[j],
                                    nodeLevelNext[k],
                                    connection));
                        }
                    }
                }
            }

            List<Node> returnedNodes = new List<Node>();
            for(int i = 0; i < nodeLevels.Length; ++i)
            {
                returnedNodes.Concat(nodeLevels[i]);
            }

            return new GraphViewModel(organizedGraph, returnedNodes);
        }

        /// <summary>
        /// If superset recipe contains subset recipe then true is returned.
        /// </summary>
        /// <returns></returns>
        private static bool IsSubsetOf(Recipe supersetRecipe, Recipe subsetRecipe)
        {
            foreach(string fooditem in supersetRecipe.FoodItems)
            {
                if(subsetRecipe.FoodItems.Contains(fooditem))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
