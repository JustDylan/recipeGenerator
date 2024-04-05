import json
import requests

# Replace 'YOUR_APP_ID' and 'YOUR_APP_KEY' with your actual Edamam API credentials
app_id = '16d65f0a'
app_key = '6ed317d144816d26818653de32091b04'

# Function to search for recipes by ingredients
def search_recipes(ingredients):
    url = f'https://api.edamam.com/search?q={",".join(ingredients)}&app_id={app_id}&app_key={app_key}'
    response = requests.get(url)
    if response.status_code == 200:
        data = response.json()
        return data['hits']  # Returns a list of recipes
    else:
        print("Error:", response.status_code)
        return None

# Example usage
ingredients = ['chicken', 'garlic', 'ginger']
recipes = search_recipes(ingredients)
if recipes:
    with open('recipes.json', 'w') as file:
        json.dump(recipes, file, indent=4)
    print("Recipes saved to 'recipes.json'")