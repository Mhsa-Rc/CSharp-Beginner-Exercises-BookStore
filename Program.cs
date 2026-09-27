using System;
using System.Collections.Generic;
using System.Linq;

namespace project2_bookstore
{
    internal class Program
    {
        static void Main()
        {
            var book = new Book();
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine(
                    "Welcome to the Bookstore!" +
                    "\n 1. Add a new book" +
                    "\n 2. Delete book with name" +
                    "\n 3. View all books" +
                    "\n 4. All books count" +
                    "\n 5. Search by book name" +
                    "\n 0. Exit");

                string menuChoice = Console.ReadLine();
                Console.Clear();

                switch (menuChoice)
                {
                    case "1":
                        book.AddNewBook();
                        break;
                    case "2":
                        book.DeleteBook();
                        break;
                    case "3":
                        book.ShowAllBooks();
                        break;
                    case "4":
                        book.ShowBookCount();
                        break;
                    case "5":
                        book.SearchBook();
                        break;
                    case "0":
                        Console.WriteLine("Exiting the program.");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        BackMenu.run();
                        break;
                }
            }
        }
    }

    public class BackMenu
    {
        public static void run()
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Enter Any key to back Main Menu ...");
            Console.ReadKey(true);
            Console.ResetColor();
        }
    }

    public class Book
    {
        List<string> BookTitles = new List<string>();
        List<string> BookAuthor = new List<string>();
        List<double> BookPrices = new List<double>();

        public void AddNewBook() 
        {
            int i = BookTitles.Count + 1;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Book {i} name or enter 0 to back to menu:");
                string nameInput = Console.ReadLine();

                if (nameInput == "0")
                {
                    break;
                }

                if (!string.IsNullOrWhiteSpace(nameInput))
                {
                    Console.Clear();
                    Console.WriteLine($"Book {i} author:");
                    string authorInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(authorInput))
                    {
                        authorInput = "Unknown";
                    }

                    
                    bool isAlreadyExists = false;
                    for (int k = 0; k < BookTitles.Count; k++)
                    {
                        if (BookTitles[k].Equals(nameInput, StringComparison.OrdinalIgnoreCase) &&
                            BookAuthor[k].Equals(authorInput, StringComparison.OrdinalIgnoreCase))
                        {
                            isAlreadyExists = true;
                            break;
                        }
                    }

                    if (isAlreadyExists)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: Book '{nameInput}' with author '{authorInput}' added to library before!");
                        Console.ResetColor();
                        BackMenu.run();
                        continue;
                    }

                    Console.Clear();
                    double priceInput;
                    Console.WriteLine($"Book {i} price:");

                    while (!double.TryParse(Console.ReadLine(), out priceInput) || priceInput < 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid input:");
                        Console.WriteLine($"Enter Book {i} price again:");
                    }

                    
                    BookTitles.Add(nameInput);
                    BookAuthor.Add(authorInput);
                    BookPrices.Add(priceInput);
                    i++;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Input cannot be empty. Please enter a valid book title.");
                    BackMenu.run();
                    continue;
                }
            }

            Console.Clear();
            Console.WriteLine($"You have total {BookTitles.Count} books in the library.");
            BackMenu.run();
        }

        public void DeleteBook() 
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter the name of the book to delete (or 0 to return):");
                string bookNameToDelete = Console.ReadLine();

                if (bookNameToDelete == "0") break;

                
                List<int> matchingIndices = new List<int>();
                for (int k = 0; k < BookTitles.Count; k++)
                {
                    if (BookTitles[k].Equals(bookNameToDelete, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingIndices.Add(k);
                    }
                }

                if (matchingIndices.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("The book not found!");
                    BackMenu.run();
                }
                else if (matchingIndices.Count == 1)
                {
                    int indexToDel = matchingIndices[0];
                    string deletedBookName = BookTitles[indexToDel];
                    string deletedAuthor = BookAuthor[indexToDel];

                    BookTitles.RemoveAt(indexToDel);
                    BookAuthor.RemoveAt(indexToDel);
                    BookPrices.RemoveAt(indexToDel);

                    Console.Clear();
                    Console.WriteLine($"Book '{deletedBookName}' by '{deletedAuthor}' deleted from library successfully.");
                    BackMenu.run();
                }
                else
                {
                    
                    Console.Clear();
                    Console.WriteLine($"Multiple books found with the name '{bookNameToDelete}'. Please choose which one to delete:");

                    for (int idx = 0; idx < matchingIndices.Count; idx++)
                    {
                        int realIndex = matchingIndices[idx];
                        Console.WriteLine($"{idx + 1}. Author: {BookAuthor[realIndex]} => Price: {BookPrices[realIndex]}$");
                    }

                    Console.WriteLine("Enter the number of the book you want to delete (or 0 to cancel):");
                    if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= matchingIndices.Count)
                    {
                        int targetRealIndex = matchingIndices[choice - 1];
                        string deletedBookName = BookTitles[targetRealIndex];
                        string deletedAuthor = BookAuthor[targetRealIndex];

                        BookTitles.RemoveAt(targetRealIndex);
                        BookAuthor.RemoveAt(targetRealIndex);
                        BookPrices.RemoveAt(targetRealIndex);

                        Console.Clear();
                        Console.WriteLine($"Book '{deletedBookName}' by '{deletedAuthor}' deleted successfully.");
                        BackMenu.run();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid selection.");
                        BackMenu.run();
                    }
                }

                Console.Clear();
                Console.WriteLine("Do you want to delete another book? (y/n)");
                string choiceToDelete = Console.ReadLine();

                if (choiceToDelete?.ToLower() == "n")
                {
                    break;
                }
            }
        }

        public void ShowAllBooks() 
        {
            Console.Clear();
            if (BookTitles.Count == 0)
            {
                Console.WriteLine("No books in Library!");
            }
            else
            {
                Console.WriteLine("Red Ones are expensive (>100$).\n");

                for (int j = 0; j < BookTitles.Count; j++)
                {
                    if (BookPrices[j] > 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{j + 1}. {BookTitles[j]} by {BookAuthor[j]} => {BookPrices[j]}$");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"{j + 1}. {BookTitles[j]} by {BookAuthor[j]} => {BookPrices[j]}$");
                    }
                }
            }
            BackMenu.run();
        }

        public void ShowBookCount() 
        {
            Console.Clear();
            double totalPrice = BookPrices.Sum();
            Console.WriteLine($"You have {BookTitles.Count} books in price of {totalPrice}$ in library.");
            BackMenu.run();
        }

        public void SearchBook() 
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter the book name you wanna search:");
                string searchName = Console.ReadLine();

                List<int> matchingIndices = new List<int>();
                for (int k = 0; k < BookTitles.Count; k++)
                {
                    if (BookTitles[k].Equals(searchName, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingIndices.Add(k);
                    }
                }

                if (matchingIndices.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Your book not found in our bookstore.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Found {matchingIndices.Count} book(s) with title '{searchName}':");
                    for (int idx = 0; idx < matchingIndices.Count; idx++)
                    {
                        int realIdx = matchingIndices[idx];
                        Console.WriteLine($"{idx + 1}. Author: {BookAuthor[realIdx]} => Price: {BookPrices[realIdx]}$");
                    }

                    Console.WriteLine("\nWas your book in this list? (yes/no)");
                    string foundChoice = Console.ReadLine()?.Trim().ToLower();

                    if (foundChoice == "yes" || foundChoice == "y")
                    {
                        Console.WriteLine("Great! Enjoy your reading.");
                    }
                    else
                    {
                        Console.WriteLine("We will try to add it soon.");
                    }
                }

                Console.WriteLine("\nDo you want to search another book? (y/n)");
                string searchAgain = Console.ReadLine()?.Trim().ToLower();
                if (searchAgain != "y")
                {
                    break;
                }
            }
        }
    }
}