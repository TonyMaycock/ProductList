using System;
using System.IO;
using System.Collections.Generic;


namespace CashMachine
{
    public class CashMachine
    {
        int MAX_CREATE_ATTEMPTS = 10;
        int ONE_POUND = 99;

        string PRODUCT_FILE_LIST_PATH = "C:\\Users\\Tony\\Documents\\Tech Test - Vertical Leap\\ProductList.txt";

        List<string> ExistingProductList = new List<string>();

        public CashMachine()
        {

        }

        /// <summary>
        /// LoadExistingProducts()
        /// Creates a local list from the stored product list
        /// Normally I would use a database here as the product list will
        /// expand to a large quantity and will be easier and quicker to 
        /// manage if part of a database
        /// </summary>
        public void LoadExistingProducts()
        {
            StreamReader sr = null;

            // file handling can have many pitfalls, here I have only tried to manage to
            // basic exceptions, more can be added quickly at a later point
            try
            {
                // Load the list of current products stored
                sr = new StreamReader(PRODUCT_FILE_LIST_PATH);
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine("Product list file not found, trying to create one.\n");

                // slight overkill when creating a new file under this circumstance, 
                // but better to be safe than sorry
                int attempts = 0;
                FileStream fs = null;
                try
                {
                    while ((null == fs) && (attempts < MAX_CREATE_ATTEMPTS))
                    {
                        fs = File.Create(PRODUCT_FILE_LIST_PATH);
                        attempts++;
                    }

                    if ((fs == null) || (attempts > MAX_CREATE_ATTEMPTS))
                    {
                        Console.WriteLine("Unable to create product file list, ending process\n");

                    }
                    else if (fs != null)
                    {
                        sr = new StreamReader(PRODUCT_FILE_LIST_PATH);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot create product list file\n");
                }
            }
            catch (FieldAccessException fae)
            {
                Console.WriteLine("File Accessing Exception accessing product list file\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("General Exception accessing product list file\n");
            }


            // quick check to guarantee that we are working with an intialised stream reader
            if (null != sr)
            {
                // retrieve all of the entries from the product list file
                while (!sr.EndOfStream)
                {
                    string streamString = sr.ReadLine();

                    if (streamString.Length > 0)
                    {
                        // look for product entries in the file
                        if (streamString.StartsWith('['))
                        {
                            ExistingProductList.Add(streamString);
                        }
                    }
                }

                sr.Close();
            }
        }

        /// <summary>
        /// GetShoppingListTotal(List<string> shoppingList)
        /// simple function that retrieves the product price from the 
        /// product list and adds it to the total of the shop
        /// </summary>
        /// <param name="shoppingList">
        /// list of items in the basket
        /// </param>
        /// <returns>
        /// the total cost of the basket as a string (for display purposes
        /// </returns>
        public string GetShoppingListTotal(List<string> shoppingList)
        {
            int itemPrice = 0;
            int totalPrice = 0;

            // iterate through each item in the basket retrieving its price
            foreach (string item in shoppingList)
            {
                itemPrice = GetItemPrice(item);

                totalPrice += itemPrice;
            }

            // format the cost to a string
            return FormatTotalPrice(totalPrice);
        }

        /// <summary>
        /// GetItemPrice(string item)
        /// finds out the price of an item from the product list
        /// </summary>
        /// <param name="item">
        /// the item to get a price for
        /// </param>
        /// <returns>
        /// the price of the item, 0 if not found
        /// </returns>
        private int GetItemPrice(string item)
        {
            int itemPrice = 0;

            // iterate through each item in teh existing product list looking
            // for its price
            foreach(string product in ExistingProductList)
            {
                // if we find the item in the product list, then retrieve its price 
                if(product.Contains(item))
                {
                    product.Trim();
                    int price = product.IndexOf(',');
                    string strPrice = product.Substring(++price);
                    strPrice = strPrice.Substring(0, strPrice.Length - 1);
                    itemPrice = Convert.ToInt32(strPrice);
                }
            }

            return itemPrice;
        }

        /// <summary>
        /// FormatTotalPrice(int totalPrice)
        /// formats the total of the basket into a string that is in £'s
        /// </summary>
        /// <param name="totalPrice">
        /// the total price of the basket
        /// </param>
        /// <returns>
        /// the final price of the basket, formated as a string representation
        /// </returns>
        private string FormatTotalPrice(int totalPrice)
        {
            string finalPrice = totalPrice.ToString();

            // if the total price is £1 or over
            if (totalPrice >= ONE_POUND)
            {
                finalPrice = finalPrice.Insert(finalPrice.Length - 2, ".");
                finalPrice = string.Format("{0}{1}", "£", finalPrice);
            }
            else
            {
                // format the string as £0.xx
                finalPrice = string.Format("{0}.{1}p", "£", totalPrice);
            }
            
            return finalPrice;
        }

        /// <summary>
        /// AddNewProduct(string newProduct, int newProductPrice)
        /// Add a new product to the product list, including its price
        /// </summary>
        /// <param name="newProduct">
        /// name of the new product
        /// </param>
        /// <param name="newProductPrice">
        /// price of the new product
        /// </param>
        public void AddNewProduct(string newProduct, int newProductPrice)
        {
            StreamWriter sw = new StreamWriter(PRODUCT_FILE_LIST_PATH, true);

            if (null != sw)
            {
                string newItem = string.Format("[{0}, {1}]", newProduct, newProductPrice);

                // some additional code needs adding in here to prevent adding duplicate products

                sw.WriteLine(newItem);

                sw.Close();
            }
        }

        /// <summary>
        /// DeleteProduct(string productName)
        /// TBC
        /// </summary>
        /// <param name="productName">
        /// name of the product to remove from the product list
        /// </param>
        public void DeleteProduct(string productName)
        {
        }

        /// <summary>
        /// EditProductPrice(string prodcutName, int newPrice)
        /// TBC
        /// </summary>
        /// <param name="prodcutName">
        /// the name of the product to change
        /// </param>
        /// <param name="newPrice">
        /// the new price for the product
        /// </param>
        public void EditProductPrice(string prodcutName, int newPrice)
        {
        }


        /// <summary>
        /// AddNewOffer()
        /// TBC
        /// </summary>
        public void AddNewOffer()
        {
        }

        /// <summary>
        /// DeleteOffer()
        /// TBC
        /// </summary>
        public void DeleteOffer()
        { 
        }
    }
}
