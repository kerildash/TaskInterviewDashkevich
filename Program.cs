using System;

namespace TaskInterviewDashkevich
{
    
    
    class Discount
    {
        private decimal amount;
        public delegate decimal AffectTheValue(decimal value);
        public AffectTheValue AffectThePrice;
        public TypeOfDiscount type;

        
        public decimal GetAmount
        {
            get
            {
                return amount;
            }
        }
        public enum TypeOfDiscount
        {
            None,
            Percentage,
            Transport,
            FreeBonusItem
        }
        public Discount(TypeOfDiscount type, decimal amount)
        {
            try
            {
                switch (type)
                {
                    case TypeOfDiscount.None:
                        this.amount = 0;
                        AffectThePrice = price => price;
                        this.type = type;
                        break;

                    case TypeOfDiscount.Percentage:
                        if (amount < 0 || amount > 100)
                        {
                            throw new Exception("Incorrect persentage of discount");
                        }
                        this.amount = amount;
                        AffectThePrice = price => price * ((100 - amount) / 100);
                        this.type = type;
                        break;

                    case TypeOfDiscount.Transport:
                    case TypeOfDiscount.FreeBonusItem:
                        if (amount < 0)
                        {
                            throw new Exception("Incorrect discount");
                        }
                        this.amount = amount;
                        AffectThePrice = price => price - this.amount;
                        this.type = type;
                        break;
                }
            }
            catch
            {
                this.amount = 0;
                AffectThePrice = price => price;
                this.type = TypeOfDiscount.None;
            }
        }
        public Discount()
        {
            amount = 0;
            AffectThePrice = price => price;
            type = TypeOfDiscount.None;
        }
    }
    
    static class Product
    {
        public const decimal price = 50;
    }
    class Purchase
    {
        public int day;
        public int quantity;
        public decimal priceOfPurchace;
        public Discount discount;

        public Purchase(decimal priceOfProduct, int day, int quantity)
        {
            this.day = day;
            this.quantity = quantity;
            priceOfPurchace = priceOfProduct * quantity;
            discount = new Discount();
        }

        public Purchase(decimal priceOfProduct, int day, int quantity,
            Discount.TypeOfDiscount typeOfDiscount, decimal amountOfDiscount)
        {
            this.day = day;
            this.quantity = quantity;
            discount = new Discount(typeOfDiscount, amountOfDiscount);
            priceOfPurchace = priceOfProduct * quantity;
            priceOfPurchace = discount.AffectThePrice(priceOfPurchace);
        }
    }

    static class MethodsForArraysOfPurchases
    {
        public static void Show(Purchase[] purchases)
        {

            Console.WriteLine("__________________________________________________________________________________________");
            Console.WriteLine("Day\t\t\tQuantity\tPrice\t\tDiscount type\tAmount of Discount");
            Console.WriteLine("__________________________________________________________________________________________");

            int totalQuantity = 0;
            decimal TotalPrice = 0;
            foreach (Purchase item in purchases)
            {
                string typeOfDiscount;
                string amountOfDiscount;
                Discount discount = item.discount;
                typeOfDiscount = GetTypeOfDiscount(item.discount);
                amountOfDiscount = GetAmountOfDiscount(item.discount);

                Console.WriteLine("{0}th of that month\t{1}\t\t{2:f2} р.\t{3}\t{4}",
                    item.day, item.quantity, item.priceOfPurchace, typeOfDiscount, amountOfDiscount);

                TotalPrice += item.priceOfPurchace;
                totalQuantity += item.quantity;

            }
            Console.WriteLine("__________________________________________________________________________________________");
            Console.WriteLine("\t\t\t{0}\t\t{1:f2} p.", totalQuantity, TotalPrice);
            Console.WriteLine("__________________________________________________________________________________________");
            Console.WriteLine("__________________________________________________________________________________________\n");

        }
        public static void sortByDay(Purchase[] purchases)
        {
            for (int i = 0; i < purchases.Length - 1; i++)
            {
                for (int j = i + 1; j < purchases.Length; j++)
                {
                    if (purchases[j].day < purchases[i].day)
                    {
                        Swap(purchases, i, j);
                    }
                }
            }
        }
        public static void sortByTypeOfDiscount(Purchase[] purchases)
        {

            int indexOfFirstUnsortedElement = 0;
            for (int enumIndex = 0; enumIndex < 4; enumIndex++)
            {
                for (int indexOfCurrentElement = indexOfFirstUnsortedElement;
                    indexOfCurrentElement < purchases.Length; indexOfCurrentElement++)
                {
                    if (purchases[indexOfCurrentElement].discount.type == (Discount.TypeOfDiscount)enumIndex)
                    {
                        Swap(purchases, indexOfFirstUnsortedElement, indexOfCurrentElement);
                        indexOfFirstUnsortedElement++;
                    }
                }
            }
        }
        public static void CheckPurchasesInSomeDay(Purchase[] purchases, int day)
        {
            int numberOfPurchases = 0;
            for (int index = 0; index < purchases.Length; index++)
            {
                if (purchases[index].day == day)
                {
                    numberOfPurchases++;
                }
            }
            if (numberOfPurchases == 0)
            {
                Console.WriteLine($"There are no purchases in the {day}th of that month");
            }
            else if (numberOfPurchases == 1)
            {
                Console.WriteLine($"There is 1 purchase in the {day}th of that month");
            }
            else
            {
                Console.WriteLine($"There are {numberOfPurchases} purchases in the {day}th of that month");
            }
        }
        private static void Swap(Purchase[] purchases, int firstIndex, int secondIndex)
        {
            Purchase buffer = purchases[firstIndex];
            purchases[firstIndex] = purchases[secondIndex];
            purchases[secondIndex] = buffer;
        }

        private static string GetTypeOfDiscount(Discount discount)
        {

            switch (discount.type)
            {
                case Discount.TypeOfDiscount.Percentage:
                    return "Percent\t";
                case Discount.TypeOfDiscount.Transport:
                    return "Free Shipping";
                case Discount.TypeOfDiscount.FreeBonusItem:
                    return "Free Item";
                default:
                    return "None";
            }
        }
        private static string GetAmountOfDiscount(Discount discount)
        {

            switch (discount.type)
            {
                case Discount.TypeOfDiscount.Percentage:
                    return $"-{discount.GetAmount} %";
                case Discount.TypeOfDiscount.Transport:
                case Discount.TypeOfDiscount.FreeBonusItem:
                    return $"-{discount.GetAmount:f2} р.";
                case Discount.TypeOfDiscount.None:
                    return "";
                default:
                    return "";
            }
        }

    }
    class Program
    {

        static Purchase[] MakeManyPurchases()
        {
            Purchase[] purchases =
            {
                new Purchase(Product.price, 6, 15),
                new Purchase(Product.price, 8, 4),
                new Purchase(Product.price, 13, 40, Discount.TypeOfDiscount.Percentage, 15),
                new Purchase(Product.price, 2 , 20, Discount.TypeOfDiscount.Transport, 130.5m),
                new Purchase(Product.price, 5, 8, Discount.TypeOfDiscount.FreeBonusItem, 90),
                new Purchase(Product.price, 2 , 30, Discount.TypeOfDiscount.FreeBonusItem, 65),
                new Purchase(Product.price, 10, 12),
                new Purchase(Product.price, 8, 60, Discount.TypeOfDiscount.Percentage, 20),
                new Purchase(Product.price, 21 , 40, Discount.TypeOfDiscount.Transport, 200),
                new Purchase(Product.price, 3 , 25, Discount.TypeOfDiscount.Transport, 800),
                new Purchase(Product.price, 26, 43, Discount.TypeOfDiscount.FreeBonusItem, 63.7m),
            };
            return purchases;
        }
        static void Main()
        {
            Purchase[] purchases = MakeManyPurchases();

            MethodsForArraysOfPurchases.sortByDay(purchases);
            MethodsForArraysOfPurchases.Show(purchases);

            MethodsForArraysOfPurchases.sortByTypeOfDiscount(purchases);
            MethodsForArraysOfPurchases.Show(purchases);

            MethodsForArraysOfPurchases.CheckPurchasesInSomeDay(purchases, 10);
        }
    }
}