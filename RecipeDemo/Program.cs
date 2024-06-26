﻿using CougHacksApp;
using CougHacksApp.Model;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

RecipeQueryManager recipeGetter = new RecipeQueryManager();

//string input = Console.ReadLine();

List<Recipe> recipes = recipeGetter.GetRecipesFromIngredientsAsync(new List<string> { "apple", "banana", "coconut" }).Result;

foreach (Recipe recipe in recipes)
{
    Console.WriteLine(recipe.Label);

    foreach (string foodItem in recipe.Ingredients)
    {
        Console.Write(foodItem + ", ");
    }

    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine();

Console.WriteLine(recipeGetter.GetRecipeByIdAsync(recipes[2].ID).Result.Label);

Console.ReadLine();