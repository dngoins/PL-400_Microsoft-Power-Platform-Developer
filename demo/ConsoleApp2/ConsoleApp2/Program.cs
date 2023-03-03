using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
// this area above is for namespaces
// a namespace is a way to uniquely name/identify classes in a package
// a package is an .Net assembly


namespace ConsoleApp2
{
    internal class Program
    {
        //methods...
        static void Main(string[] args)
        {
            // Console class - WriteLine , ReadLine

            // Psuedo code
            // display the app directions
            // use Console.WriteLine to display directions...
            Console.WriteLine("-------------\nHi User, welcome\n--------------\n");
            Console.WriteLine("Enter 1 to select Audi\nEnter 2 to select Toyota\nEnter 3 to ask Elon Musk to get on his private jet\n");

            // ask user to select an item from menu
            Console.Write("Please enter a value >:");

            // read the user selection
            var userSelection = Console.ReadLine();
            int iUserSelection = 99;
           
            try
            {
                // valid the input
                // never ever ever ever never
                // trust user input
                // ValidateInput(userSelection);
                iUserSelection = int.Parse(userSelection);
            }
            catch (Exception ex)
            {
                // we could ask to type again
                // or fail gracefully
                Console.WriteLine(ex.Message);
                return;
            }

            // if the user selects 1 - then create an audi
            // if the user selects 2 - then they wan to create a toyota
            // if the user selects 3 - then they want a private jet to mars
            // boolean
            // James Boole
            // on / Off
            // T / False
            // 1 / 0
            // AND, OR, XOR
            //if (iUserSelection == 1)
            //{

            //}
            Car c = null;

            switch (iUserSelection)
            {
                case 1:
                    // Create a Audi car
                    c = CreateCar("audi");

                    break;
                case 2:
                    // Create a Toyota car
                    c = CreateCar("toyota");

                    break;
                case 3:
                    // Create a Private Jet Request
                    c = CreatePrivateJetRequest();

                    break;
                default:
                    Console.WriteLine("Sorry buddy you must have me for someone else, choose a number from 1 to 3...\n");
                    break;
            }

           
            // then display a random report
            PrintCar(c);
            Console.ReadLine();

        }

        private static void PrintCar(Car c)
        {
            Console.WriteLine(c);
        }

        private static Car CreatePrivateJetRequest()
        {
            return null;
        }

        private static Car CreateCar(string v)
        {
            // once the car selection is made 
            // ask user to type in the name and year
            Car c = new Car();
            c.OnMakeChange += C_OnMakeChange;
            c.Make = v;
            c.Model = "Random";
            Console.Write("\nPlease type in a name:>");
            var name = Console.ReadLine();
            Console.Write("\nPlease type in a year:>");
            var year = 1900;
            try
            {
                year = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return c;
            }
            
            c.Year = year;
            c.Name = name;
            return c;
        }

        private static void C_OnMakeChange(object sender)
        {
            // called when ever anyone makes an "Make" car change
            Console.WriteLine("Car Make property was changed...\n");
        }
                    

    }
}
