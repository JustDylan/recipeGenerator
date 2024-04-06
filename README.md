# RecipeGenerator
The Recipe Generator is a tool designed to analyze available recipes given ingredients into its UI. The program dynamically resolves all known recipes from our custom SQL database to a human-readable node graph. The "simplest" recipes are listed in the bottom and the complexity rises with each row of nodes. A connection between a higher and lower level node denotes that the less complex recipe is atleast a partial subset of the former. Clicking on a node brings up 

This dynamic analysis could enable flexibility for its users; busy professionals, families, and college students could utilize the tool to determine the food to cook by their needs (i.e. time, rationing ingredient use, preference, and shopping plans). We designed both the database and a pseudo-API to acess as a proof of concept: other services that offer a recipe database charge for excess API calls, such as  We imagine that this proves a proof of concept of hosting a pseudo-API for said dataset

 Sources & Attributation

As a disclaimer, we utilized AI to augment our abilities. However, we handdesigned the premise, UML diagram, and most of the UI. 

The reci
