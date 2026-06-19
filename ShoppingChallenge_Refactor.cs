using System;
using System.Collections.Generic;

namespace CodingChallenge.Shopping
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.ChristmasShoppingAtTheGroceryStore();
            program.BuyingFoodAtTheGroceryStore();
        }
        
        void ChristmasShoppingAtTheGroceryStore()
        {
            var customer = new Customer
            {
                Carts = new List<CartItem>
                {
                    new CartItem {ProductName = "Lights", Category = ProductCategory.Christmas, Price = 5.99m, Quantity = 10},
                    new CartItem {ProductName = "Tree", Category = ProductCategory.Christmas, Price = 169m, Quantity = 1},
                    new CartItem {ProductName = "Ornaments", Category = ProductCategory.Christmas, Price = 8m, Quantity = 15},
                }
            };

            ICheckoutCalculator calculator = new GroceryStoreCheckoutCalculator();

            var total = calculator.Calculate(customer, new DateTime(2020,11,30));
            Console.WriteLine("Christmas shopping total: " + total);
            
            total = calculator.Calculate(customer, new DateTime(2020,12,30));
            Console.WriteLine("Christmas shopping total after Christmas: " + total);
        }
        
        void BuyingFoodAtTheGroceryStore()
        {
            var customer = new Customer
            {
                Carts = new List<CartItem>
                {
                    new CartItem {ProductName = "Apple", Category = ProductCategory.Food, Price = 3.27m, Weight = 0.79m},
                    new CartItem {ProductName = "Scallop", Category = ProductCategory.Food, Price = 18m, Weight = 1.5m},
                    new CartItem {ProductName = "Salad", Category = ProductCategory.Food, Price = 6.99m, Quantity = 1},
                    new CartItem {ProductName = "Ground Beef", Category = ProductCategory.Food, Price = 7.99m, Weight = 1.5m},
                    new CartItem {ProductName = "Red Wine", Category = ProductCategory.Food, Price = 25.99m, Quantity = 1}
                }
            };
            
            ICheckoutCalculator calculator = new GroceryStoreCheckoutCalculator();

            var total = calculator.Calculate(customer, new DateTime(2020,11,30));
            Console.WriteLine("Food cart total: " + total);
            
            total = calculator.Calculate(customer, new DateTime(2020,11,30, 7, 11, 0));
            Console.WriteLine("Food cart total (senior hour): " + total);
        }
        
    }


    #region Shared

    public enum ProductCategory
    {
        Unknown = 0,
        Christmas = 1,
        Food = 2
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductCategory Category { get; set; }
        
        public decimal Weight { get; set; }
    }

    public class Customer
    {
        public bool FirstResponder { get; set; }
        public bool MilitaryVeteran { get; set; }
        public bool Employee { get; set; }
        public List<CartItem> Carts { get; set; } = new List<CartItem>();
    }

    #endregion
    

    #region Pricing

    /// <summary>
    /// Pricing strategy interface for calculating base prices of items.
    /// Supports different pricing models (quantity-based, weight-based, etc.).
    /// Follows the Strategy Pattern to make discounts easily extensible.
    /// </summary>
    public interface IPricingStrategy
    {
        /// <summary>Determines if this pricing strategy applies to the given item.</summary>
        bool Applies(CartItem item);

        /// <summary>Calculates the base price using this strategy.</summary>
        decimal CalculatePrice(CartItem item);
    }

    /// <summary>Quantity-based pricing strategy (default).</summary>
    public class QuantityBasedPricing : IPricingStrategy
    {
        public bool Applies(CartItem item) => item.Weight == 0 && item.Quantity > 0;

        public decimal CalculatePrice(CartItem item) => item.Quantity * item.Price;
    }

    /// <summary>Weight-based pricing strategy for produce and meats.</summary>
    public class WeightBasedPricing : IPricingStrategy
    {
        public bool Applies(CartItem item) => item.Weight > 0;

        public decimal CalculatePrice(CartItem item) => item.Weight * item.Price;
    }

    #endregion


    #region Discounts

    /// <summary>
    /// Discount strategy interface for applying different discount rules.
    /// </summary>
    public interface IDiscount
    {
        /// <summary>Determines if this discount applies to the given item, date, and customer.</summary>
        bool CanApply(CartItem item, DateTime checkOutDate, Customer customer);

        /// <summary>Applies the discount to the base price.</summary>
        decimal Apply(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer);
    }

    /// <summary>
    /// Christmas discount: 20% off before Dec 15, 60% off Dec 15-25, 90% off after Dec 25.
    /// </summary>
    public class ChristmasDiscount : IDiscount
    {
        public bool CanApply(CartItem item, DateTime checkOutDate, Customer customer)
        {
            return item.Category == ProductCategory.Christmas && checkOutDate.Month == 12;
        }

        public decimal Apply(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer)
        {
            decimal discountRate = checkOutDate.Day < 15 ? 0.20m
                                 : checkOutDate.Day <= 25 ? 0.60m
                                 : 0.90m;
            return basePrice * (1 - discountRate);
        }
    }

    /// <summary>
    /// Senior hour discount: 10% off for food items purchased between 6:00 AM and 8:00 AM.
    /// </summary>
    public class SeniorHourDiscount : IDiscount
    {
        public bool CanApply(CartItem item, DateTime checkOutDate, Customer customer)
        {
            return item.Category == ProductCategory.Food 
                && checkOutDate.TimeOfDay.Hours > 6 
                && checkOutDate.TimeOfDay.Hours <= 8;
        }

        public decimal Apply(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer)
        {
            return basePrice * 0.9m;
        }
    }

    /// <summary>
    /// First Responder customer discount: 10% off
    /// </summary>
    public class FirstResponderDiscount : IDiscount
    {
        public bool CanApply(CartItem item, DateTime checkOutDate, Customer customer)
        {
            return customer.FirstResponder == true;
        }

        public decimal Apply(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer)
        {
            return basePrice * 0.9m;
        }
    }

    
    /// <summary>
    /// Discount engine for applying multiple discount strategies in sequence.
    /// </summary>
    public interface IDiscountEngine
    {
        /// <summary>Applies all applicable discounts to the base price.</summary>
        decimal ApplyDiscounts(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer);
    }

    /// <summary>Default implementation of discount engine using strategy pattern.</summary>
    public class DiscountEngine : IDiscountEngine
    {
        private readonly List<IDiscount> _discounts;

        public DiscountEngine(params IDiscount[] discounts)
        {
            _discounts = new List<IDiscount>(discounts);
        }

        public decimal ApplyDiscounts(decimal basePrice, CartItem item, DateTime checkOutDate, Customer customer)
        {
            decimal price = basePrice;
            customer ??= new Customer();

            foreach (var discount in _discounts)
            {
                if (discount.CanApply(item, checkOutDate, customer))
                {
                    price = discount.Apply(price, item, checkOutDate, customer);
                }
            }

            return price;
        }
    }

    #endregion


    #region Checkout

    /// <summary>
    /// Public interface for the checkout calculator to enable DI and testing.
    /// </summary>
    public interface ICheckoutCalculator
    {
        decimal Calculate(Customer customer, DateTime checkOutDate);
    }

    /// <summary>
    /// Calculates checkout totals by composing pricing and discount strategies.
    /// Uses the Strategy and Composition patterns for maximum extensibility.
    /// </summary>
    public class GroceryStoreCheckoutCalculator : ICheckoutCalculator
    {
        private readonly IPricingStrategy[] _pricingStrategies;
        private readonly IDiscountEngine _discountEngine;

        /// <summary>Creates calculator with default pricing strategies and discounts.</summary>
        public GroceryStoreCheckoutCalculator() 
        {
            _pricingStrategies = new IPricingStrategy[] { new QuantityBasedPricing(), new WeightBasedPricing() };
            _discountEngine = new DiscountEngine(new ChristmasDiscount(), new SeniorHourDiscount(), new FirstResponderDiscount());
        }

        /// <summary>Creates calculator with custom pricing strategies and discount engine.</summary>
        public GroceryStoreCheckoutCalculator(IPricingStrategy[] pricingStrategies, IDiscountEngine discountEngine)
        {
            _pricingStrategies = pricingStrategies;
            _discountEngine = discountEngine ?? throw new ArgumentNullException(nameof(discountEngine));
        }

        /// <summary>Calculates the total price for a customer's cart of items after applying pricing and discounts.</summary>
        public decimal Calculate(Customer customer, DateTime checkOutDate)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (customer.Carts == null) throw new ArgumentNullException(nameof(customer.Carts));

            decimal itemTotal = 0m;

            foreach (var item in customer.Carts)
            {
                ValidateItem(item);

                decimal itemPrice = CalculateItemPrice(item, checkOutDate, customer);
                itemTotal += itemPrice;
            }

            return itemTotal;
        }

        /// <summary>Calculates the final price for a single item with pricing strategy and discounts.</summary>
        private decimal CalculateItemPrice(CartItem item, DateTime checkOutDate, Customer customer)
        {
            decimal basePrice = CalculateBasePrice(item);
            return _discountEngine.ApplyDiscounts(basePrice, item, checkOutDate, customer);
        }

        /// <summary>Calculates the base price using the first applicable pricing strategy.</summary>
        private decimal CalculateBasePrice(CartItem item)
        {
            foreach (var strategy in _pricingStrategies)
            {
                if (strategy.Applies(item))
                {
                    return strategy.CalculatePrice(item);
                }
            }

            // Fallback: quantity-based pricing
            return item.Quantity * item.Price;
        }

        private static void ValidateItem(CartItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.Category == ProductCategory.Unknown)
                throw new ArgumentException("Item must have a valid product category.", nameof(item.Category));

            if (item.Price < 0m)
                throw new ArgumentOutOfRangeException(nameof(CartItem.Price), "Price cannot be negative.");

            if (item.Weight < 0m)
                throw new ArgumentOutOfRangeException(nameof(CartItem.Weight), "Weight cannot be negative.");

            if (item.Quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(CartItem.Quantity), "Quantity cannot be negative.");

            if (item.Weight > 0m && item.Quantity > 0)
                throw new ArgumentException("Item cannot be both weighed and counted. Use either Weight or Quantity.", nameof(item));

            if (item.Weight == 0m && item.Quantity == 0)
                throw new ArgumentException("Item must have either a positive Quantity or a positive Weight.", nameof(item));

            if (string.IsNullOrWhiteSpace(item.ProductName))
                throw new ArgumentException("Item must have a product name.", nameof(item.ProductName));
        }
    }
    #endregion
}