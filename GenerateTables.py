import json
import psycopg2

# Load recipes from JSON file
with open('recipes.json', 'r') as file:
    recipes_data = json.load(file)

# Function to create a table of recipe data
def create_recipe_table(cursor):
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS recipe (
        id SERIAL PRIMARY KEY,
        recipe_name TEXT,
        url TEXT
    )
    """)

    for recipe in recipes_data:
        recipe_name = recipe['recipe']['label']
        url = recipe['recipe']['url']
        cursor.execute("""
        INSERT INTO recipe (recipe_name, url)
        VALUES (%s, %s)
        """, (recipe_name, url))

# Function to create a table of recipe ingredients
def create_recipe_ingredient_table(cursor):
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS recipeingredient (
        id SERIAL PRIMARY KEY,
        recipe_id INTEGER REFERENCES recipe(id),
        ingredient TEXT
    )
    """)

    for recipe in recipes_data:
        recipe_name = recipe['recipe']['label']
        recipe_id = cursor.execute("""
        SELECT id FROM recipe WHERE recipe_name = %s
        """, (recipe_name,))
        ingredients = recipe['recipe']['ingredients']
        for ingredient in ingredients:
            cursor.execute("""
            INSERT INTO recipeingredient (recipe_id, ingredient)
            VALUES (%s, %s)
            """, (recipe_id, ingredient['food']))

# Function to extract unique ingredients
def extract_ingredients(recipe_data):
    all_ingredients = set()
    for recipe in recipe_data:
        ingredients = recipe['recipe']['ingredients']
        for ingredient in ingredients:
            all_ingredients.add(ingredient['food'])
    return all_ingredients

# Function to create a table of unique ingredients
def create_ingredients_table(cursor, unique_ingredients):
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS ingredient (
        id SERIAL PRIMARY KEY,
        ingredient_name TEXT
    )
    """)
    for ingredient in unique_ingredients:
        cursor.execute("""
        INSERT INTO ingredient (ingredient_name)
        VALUES (%s)
        """, (ingredient,))

# Connect to PostgreSQL database
conn = psycopg2.connect(
    dbname='recipedatabase',
    user='postgres',
    password='3026',
    host='localhost',
)

# Extract unique ingredients
unique_ingredients = extract_ingredients(recipes_data)

# Create tables
with conn.cursor() as cursor:
    create_recipe_table(cursor)
    create_recipe_ingredient_table(cursor)
    create_ingredients_table(cursor, unique_ingredients)

# Commit changes and close connection
conn.commit()
conn.close()