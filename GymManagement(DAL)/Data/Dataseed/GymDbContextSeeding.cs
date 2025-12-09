using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using System.Text.Json;
namespace GymManagementDAL.Data.Dataseed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext gymDbContext)
        {
            try
            {
                var HasPlans = gymDbContext.Plans.Any();
                var HasCategories = gymDbContext.Categories.Any();
                if (HasPlans && HasCategories) return false;

                if (!HasPlans)
                {
                    var Plans = LoadDataFromJsonFile<Plan>("Plans.json");
                    if (Plans.Any())
                        gymDbContext.Plans.AddRange(Plans);
                }
                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFile<Category>("Categories.json");
                    if (Categories.Any())
                        gymDbContext.Categories.AddRange(Categories);
                }

                return gymDbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Data seeding failed: {ex.Message}");
               return false;   
            }

        }

        private static List<T> LoadDataFromJsonFile<T>(string FileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();

            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions ()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }

    }
}
