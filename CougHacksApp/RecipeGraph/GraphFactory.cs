using Microsoft.Msagl.Drawing;
using CougHacksApp.Model;

namespace CougHacksApp.RecipeGraph
{
    internal static class GraphFactory
    {
        // percent limit of nonmatching food items when performing subset compare
        private static float nonMatchLimit = 0.5F;

        private static List<Recipe>[] recipeLevels =
            new List<Recipe>[5];

        private static List<Node>[] nodeLevels =
            new List<Node>[5];

        public static GraphViewModel CreateGraph(List<Recipe> recipes)
        {
            Graph organizedGraph = new Graph();
            organizedGraph.Attr.LayerDirection = LayerDirection.LR;

            HashSet<string> formedPairs = new HashSet<string>(); 

            List<Node> nodes = new List<Node>();
            
            List<Recipe> templist = new List<Recipe>();
            for (int i = 0; i < recipes.Count && i < 40; i++)
                templist.Add(recipes[i]);
            recipes = templist;

            recipes.Sort((x, y) => { return x.FoodItems.Count - y.FoodItems.Count; });

            int layerSize = recipes.Count()/recipeLevels.Length;

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
                        organizedGraph.AddNode(recipeNode);

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
                            string pair = levelPrev[j].Label + levelNext[k].Label;
                            /*
                            bool containsMatchingEdge = false;
                            foreach (Edge outEdge in nodeLevelNext[k].OutEdges)
                            {
                                if (nodeLevelNext[k].InEdges.Contains(outEdge))
                                    containsMatchingEdge = true;
                            }*/
                            /*ConnectionToGraph connection =
                                new ConnectionToGraph();

                            organizedGraph.AddPrecalculatedEdge(
                                new Edge(
                                    nodeLevelPrev[j],
                                    nodeLevelNext[k],
                                    connection));*/
                            if (!formedPairs.Contains(pair) && levelPrev[j].Label != levelNext[k].Label)
                            {
                                formedPairs.Add(pair);
                                //organizedGraph.AddEdge(levelPrev[j].Label, levelNext[k].Label);
                                ConnectionToGraph connection =
                                new ConnectionToGraph();

                                organizedGraph.AddPrecalculatedEdge(
                                    new Edge(
                                        nodeLevelPrev[j],
                                        nodeLevelNext[k],
                                        connection));
                            }
                        }
                    }
                }
            }

            List<Node> returnedNodes = new List<Node>();
            for(int i = 0; i < nodeLevels.Length; ++i)
            {
                returnedNodes = returnedNodes.Concat(nodeLevels[i]).ToList();
            }

            return new GraphViewModel(organizedGraph, returnedNodes);
        }

        /// <summary>
        /// If superset recipe contains subset recipe then true is returned.
        /// </summary>
        /// <returns></returns>
        private static bool IsSubsetOf(Recipe supersetRecipe, Recipe subsetRecipe)
        {
            //return false;
            // number of unmatched food items.
            int unMatched = 0;

            foreach(string fooditem in subsetRecipe.FoodItems)
            {
                if(!supersetRecipe.FoodItems.Contains(fooditem) &&
                    ((float)++unMatched)/subsetRecipe.FoodItems.Count() > nonMatchLimit)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
