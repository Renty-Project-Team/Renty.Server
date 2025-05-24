using Renty.Server.Post.Domain;

namespace Renty.Server.Product.Domain
{
    public enum CategoryType
    {
        ClothingAndFashion = 1,
        Electronics,
        FurnitureAndInterior,
        Beauty,
        Books,
        Stationery,
        CarAccessories,
        Sports,
        InfantsAndChildren,
        PetSupplies,
        HealthAndMedical,
        Hobbies
    }

    public class Categorys
    {
        public int Id { get; set; }
        public required CategoryType Name { get; set; }

        public List<Items> Items { get; set; } = [];
        public List<BuyerPosts> BuyerPosts { get; set; } = [];
    }
}
