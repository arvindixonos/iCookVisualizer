using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum eRecipe
    {
        RECIPE_AMERICAN_PEPPER_CHICKEN
    }

    public enum eRecipeStep
    {
        STEP_ADD_INGREDIENT
    }

    public class RecipeStep
    {
        public eRecipeStep receipeStep;
    }

    public class Recipe
    {
        private eRecipe recipe;
        public eRecipe GetRecipe
        {
            get { return recipe; }
        }

        private List<RecipeStep> recipeSteps = new List<RecipeStep>();
        public void AddRecipeStep(RecipeStep receipeStep)
        {
            recipeSteps.Add(receipeStep);
        }

        public RecipeStep GetRecipeStep(int recipeStep)
        {
            return recipeSteps[recipeStep];
        }
    }

    public class ReceipeManager
    {
        private List<Recipe> recipes = new List<Recipe>();
        private bool inited = false;

        public void AddRecipe()
        {

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

        }
    }
}