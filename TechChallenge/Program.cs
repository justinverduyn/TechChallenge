using System;
using System.Collections.Generic;
using System.Linq;
using TechChallenge.Models;

namespace TechChallenge
{
    internal class Program
    {
        private static void Main()
        {
            var db = new TechContext();
            // Add entries to the database
            Create(db);
            // Find the list of books to order
            var items = Read(db);
            Display(db, items);
        }

        // Apply Discount
        private static decimal ApplyDiscount(decimal total, decimal discount)
        {
            return total * (100 - discount) / 100;
        }

        // Apply Shipping Rate of $5.95
        private static decimal ApplyShipping(decimal totalWithoutTax)
        {
            const decimal deliveryFee = 5.95M;
            return totalWithoutTax + deliveryFee;
        }

        // Apply Tax Rate of 10%
        private static decimal ApplyTax(decimal totalWithoutTax)
        {
            const decimal taxRate = 1.10M;
            return totalWithoutTax * taxRate;
        }

        private static void Display(TechContext db, List<Item> items)
        {
            decimal totalWithoutTax = 0;
            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
                totalWithoutTax += Total(db, item);
            }

            totalWithoutTax = HandleShipping(totalWithoutTax);
            decimal totalWithTax = ApplyTax(totalWithoutTax);
            Console.WriteLine("");
            Console.WriteLine("Cost without tax: {0:c}", totalWithoutTax);
            Console.WriteLine("Cost with tax: {0:c}", totalWithTax);
        }

        // For orders less than $20 add a delivery fee
        private static decimal HandleShipping(decimal totalWithoutTax)
        {
            const decimal deliveryMinimum = 20;

            if (totalWithoutTax < deliveryMinimum)
            {
                totalWithoutTax = ApplyShipping(totalWithoutTax);
            }

            return totalWithoutTax;
        }

        private static decimal Total(TechContext db, Item item)
        {
            var category = db.Categories.Find(item.CategoryId);

            // get price
            var total = item.Price;

            // apply discount?
            if (category.Discount > 0)
            {
                total = ApplyDiscount(total, category.Discount);
            }

            return total;
        }

        private static void Create(TechContext db)
        {
            Console.WriteLine("Inserting new Categories");
            db.Add(new Category {Name = "Crime", Discount = 5});
            db.SaveChanges();
            db.Add(new Category {Name = "Fantasy"});
            db.SaveChanges();
            db.Add(new Category {Name = "Romance"});
            db.SaveChanges();

            Console.WriteLine("Inserting new Items");
            db.Add(new Item
                {Author = "Emily G. Thompson, Amber Hunt", Title = "Unsolved murders", Price = 10.99M, CategoryId = 1});
            db.SaveChanges();
            db.Add(new Item {Author = "Lewis Carroll", Title = "Alice in Wonderland", Price = 5.99M, CategoryId = 2});
            db.SaveChanges();
            db.Add(new Item {Author = "Roland Merullo", Title = "A Little Love Story", Price = 2.40M, CategoryId = 3});
            db.SaveChanges();
            db.Add(new Item {Author = "S J Parris", Title = "Heresy", Price = 6.80M, CategoryId = 2});
            db.SaveChanges();
            db.Add(new Item {Author = "Michael Ende", Title = "The Neverending Story", Price = 7.99M, CategoryId = 2});
            db.SaveChanges();
            db.Add(new Item {Author = "Philip Sugden", Title = "Jack The Ripper", Price = 16.00M, CategoryId = 1});
            db.SaveChanges();
            db.Add(new Item {Author = "Greg Hildebrandt", Title = "The Tolkien Years", Price = 22.90M, CategoryId = 2});
            db.SaveChanges();

            Console.WriteLine("");
        }

        private static List<Item> Read(TechContext db)
        {
            IEnumerable<Item> itemQuery =
                from item in db.Items
                where (item.Title == "Unsolved murders" 
                       || item.Title == "A Little Love Story" 
                       || item.Title == "Heresy" 
                       || item.Title == "Jack The Ripper" 
                       || item.Title == "The Tolkien Years")
                select item;

            return itemQuery.ToList();
        }
    }
}