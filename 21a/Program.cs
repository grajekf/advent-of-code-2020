using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _21a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines("input.txt");
            var foodList = inputLines.Select(Food.Parse).ToList();

            var allAllergens = foodList.SelectMany(f => f.Allergens).Distinct();
            var allIngredients = foodList.SelectMany(f => f.Ingredients).Distinct();

            var possibleAllergensDict = new Dictionary<string, IList<string>>();

            foreach (var i in allIngredients)
            {
                possibleAllergensDict[i] = new List<string>();
            }

            foreach (var allergen in allAllergens)
            {
                var foodsWithAllergen = foodList.Where(f => f.Allergens.Contains(allergen));
                var possibleIngredients =
                    foodsWithAllergen.Aggregate(allIngredients, (i, next) => i.Intersect(next.Ingredients));

                foreach (var i in possibleIngredients)
                {
                    possibleAllergensDict[i].Add(allergen);
                }
            }

            var ingredientsWithoutAllergens = allIngredients.Where(i => possibleAllergensDict[i].Count == 0);
            Console.WriteLine(ingredientsWithoutAllergens.Sum(i => foodList.Count(f => f.Ingredients.Contains(i))));

        }
    }

    class Food
    {
        public Food(IEnumerable<string> ingredients, IEnumerable<string> allergens)
        {
            Ingredients = ingredients;
            Allergens = allergens;
        }

        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Allergens { get; set; }

        public static Food Parse(string input)
        {
            var items = input.Split(" (contains ");
            var ingredientString = items[0];
            var allergensString = items[1].Replace(")", "");

            var ingredients = ingredientString.Split(" ");
            var allergens = allergensString.Split(", ");

            return new Food(ingredients, allergens);
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Ingredients)} (contains {string.Join(", ", Allergens)})";
        }
    }
}
