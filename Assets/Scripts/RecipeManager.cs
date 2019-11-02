using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace iCook
{
    public enum eRecipe
    {
        RECIPE_AMERICAN_PEPPER_CHICKEN,
        RECIPE_COUNT
    }

    public enum eIngredientType
    {
        ING_NONE,
        ING_OIL,
        ING_ONION,
        ING_CAPSICUM,
        ING_CHICKEN,
        ING_SALT,
        ING_PEPPER
    }

    public enum eRecipeTask
    {
        TASK_NONE,
        TASK_ADD_INGREDIENT,
        TASK_STIR,
        TASK_SET_TEMPERATURE,
        TASK_IDLE,
        TASK_COUNT
    }

    [System.Serializable]
    public class RecipeTask
    {
        public string recipeTask;
        public eRecipeTask recipeTaskEnum;
        public string payload;
        public float duration;

        private bool inited = false;

        public void Init()
        {
            if(inited)
            {
                return;
            }

            inited = true;

            recipeTaskEnum =  RecipeManager.GetRecipeTaskEnum(recipeTask);
        }
    }

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public eRecipe recipe;
        public List<RecipeTask> recipeTasks = new List<RecipeTask>();

        private bool inited = false;

        public void Init()
        {
            if(inited)
            {
                return;
            }

            inited = true;

            foreach(RecipeTask recipeTask in recipeTasks)
            {
                recipeTask.Init();
            }
        }

        public void AddRecipeTask(RecipeTask recipeTask)
        {
            recipeTasks.Add(recipeTask);
        }

        public RecipeTask GetRecipeTask(int taskIndex)
        {
            return recipeTasks[taskIndex];
        }

        public bool isTaskPresent(int taskIndex)
        {
            return taskIndex < recipeTasks.Count;
        }
    }

    public class RecipeManager
    {
        private List<Recipe> recipes = new List<Recipe>();
        private bool inited = false;

        public static eIngredientType GetIngredientTypeEnum(string ingredient)
        {
            switch (ingredient)
            {
                case "Oil": return eIngredientType.ING_OIL;
                case "Onion": return eIngredientType.ING_ONION;
                case "Capsicum": return eIngredientType.ING_CAPSICUM;
                case "Chicken": return eIngredientType.ING_CHICKEN;
                case "Salt": return eIngredientType.ING_SALT;
                case "Pepper": return eIngredientType.ING_PEPPER;
            }

            return eIngredientType.ING_NONE;
        }

        public static eRecipeTask GetRecipeTaskEnum(string recipeTask)
        {
            switch (recipeTask)
            {
                case "Add Ingredient": return eRecipeTask.TASK_ADD_INGREDIENT;
                case "Stir": return eRecipeTask.TASK_STIR;
                case "Set Temperature": return eRecipeTask.TASK_SET_TEMPERATURE;
                case "Idle": return eRecipeTask.TASK_IDLE;
            }

            return eRecipeTask.TASK_NONE;
        }

        public void Init()
        {
            if (inited)
                return;

            LoadAllRecipes();

            inited = true;
        }

        private void LoadAllRecipes()
        {
            int numRecipes = (int)eRecipe.RECIPE_COUNT;

            for(int i = 0; i < numRecipes; i++)
            {
                string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/Recipe/" + "American Pepper Chicken.txt");
                Recipe recipe = JsonUtility.FromJson<Recipe>(jsonText);
                recipe.Init();
                recipes.Add(recipe);
            }
        }

        public Recipe GetRecipe(eRecipe recipeType)
        {
            foreach(Recipe recipe in recipes)
            {
                if(recipe.recipe == recipeType)
                {
                    return recipe;
                }
            }

            return null;
        }
    }
}