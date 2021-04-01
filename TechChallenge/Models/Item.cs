namespace TechChallenge.Models
{
    class Item
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
    }
}