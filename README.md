# RecipeGenerator

<p align="center">
  <img width="15%" src="https://github.com/JustDylan/recipeGenerator/blob/main/CougHacksApp/Icons/logo512.png" alttext="Recipe Generator Logo: An abstract, open fridge/cabinet with nodes just outside the door with connections originating from inside the container. The nodes are connected to a common node closer to the right. The upper node is yellow, the lower is green. The right node, being an interpretation of a more complex recipe is red.">
</p>

The Recipe Generator is a tool designed to analyze available recipes with given ingredients into its UI. The program dynamically resolves all known recipes from our custom SQL database to a human-readable node graph. The "simplest" recipes are listed in the bottom and the complexity rises with each row of nodes. A connection between a higher and lower level node denotes that the less complex recipe is atleast a partial subset of the former. Clicking on a node brings up a window to quickly examine its respective recipe.

This dynamic analysis could enable flexibility for many of its users; busy professionals, families, and college students could utilize the tool to determine the food to cook by their needs (i.e. time, rationing ingredient use, preference, and shopping plans). We designed both the database and a associated pseudo-API to act as a proof of concept. Other services that offer a recipe database charge for excess API calls such as Edamam. Self-hosting and offline access of a recipe database is key towards scalability by distribution.

# Sources & Attributation

As a disclaimer, we utilized AI to augment our abilities which allowed us to implement most of this proof of concept within a 24 hour deadline. However, we hand-designed the premise, UML diagram, UI, and this README. 

Our print icon and program logo/icon are our own work; all other icons were retrieved from [Iconoir](https://iconoir.com/).

We utilized this extensive, [free recipe offline database](https://recipenlg.cs.put.poznan.pl/dataset) as our initial source for recipes.

# Future Roadmap
We ran out of time before our deadline to implement the following features:
- color coding with respect to number of ingredients
- properly saving bookmarks to disk
- implement a more sophisticated recipe filtering (filter out by diet, calories, etc) to allow faster searching
- exporting the recipe node graph to disk
- advanced portability with proper proceedure for self-hosting a database (such as an installation wizard for client-side private use)
- database editing: allow custom recipes from the user 

# UML Diagram
We have created a UML diagram to act as a master outline through this project: https://github.com/JustDylan/recipeGenerator/blob/main/Classdiagram.mdj
The UML can also be found as an image:

![UML Diagram as a jpg image](https://github.com/JustDylan/recipeGenerator/blob/main/ClassDiagram.jpg)
