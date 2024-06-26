# RecipeGenerator

<p align="center">
  <img width="15%" src="https://github.com/JustDylan/recipeGenerator/blob/main/CougHacksApp/Icons/logo512.png" alttext="Recipe Generator Logo: An abstract, open fridge/cabinet with nodes just outside the door with connections originating from inside the container. The nodes are connected to a common node closer to the right. The upper node is yellow, the lower is green. The right node, being an interpretation of a more complex recipe is red.">
</p>

The Recipe Generator is a tool designed to analyze available recipes with given ingredients into its UI. The program dynamically resolves all known recipes from our custom SQL database to a human-readable node graph. The "simplest" recipes are listed in the bottom and the complexity rises with each row of nodes. A connection between a higher and lower level node denotes that the less complex recipe is atleast a partial subset of the former. Clicking on a node brings up a window to quickly examine its respective recipe. In other words, a user can generate an intuitive model from a database for known recipies with the ingredients they have, and any simpler recipes that could also be made from a subset of those ingredients .

We believe our dynamic analysis enables flexibility for many people: busy professionals, families, and college students could utilize the tool to determine the food to cook by their needs (i.e. time, rationing ingredient use, preference, and shopping plans). We designed both the database and a associated pseudo-API to act as a proof of concept. Other services that offer a recipe database charge for excess API calls such as Edamam. Self-hosting and offline access of a recipe database is key towards scalability by distribution.

# Sources & Attribution

As a disclaimer, we utilized AI to augment our abilities which allowed us to implement most of this proof of concept within a 24 hour deadline. However, we hand-designed the premise, UML diagram, UI, and this README. 

Our print icon and program logo/icon are our own work; all other icons were retrieved from [Iconoir](https://iconoir.com/).

We utilized this extensive, [free recipe offline database](https://recipenlg.cs.put.poznan.pl/dataset) as our initial source for recipes.

# Future Roadmap
We ran out of time before our deadline to implement the following features:
- properly saving bookmarks to disk
- implement a more sophisticated recipe filtering (filter out by diet, calories, etc) to allow faster searching
- exporting the recipe node graph to disk
- advanced portability with proper proceedure for self-hosting a database (such as an installation wizard for client-side private use)
- database editing: allow custom recipes from the user
- enhanced stability

# Architecture
We used C# to develop the core front/middle-end due to an apparent aptitude for fast prototyping. As development progressed, we branched out into including python code for parsing our database file into a SQL server. C# handles the end-user interractions, querying the SQL server for any recipe it needs. The front-end additionally performs the "middle-end" analysis to build and display a graph of recipies as described in the summary.

# UML Diagram
We have created a [UML diagram](https://github.com/JustDylan/recipeGenerator/blob/main/Classdiagram.mdj) to act as a master outline through this project:

![UML Diagram as a jpg image render.](https://github.com/JustDylan/recipeGenerator/blob/main/ClassDiagram.jpg)
