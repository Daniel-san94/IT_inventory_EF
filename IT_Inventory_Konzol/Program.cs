using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT_inventory_EF;

namespace IT_Inventory_Konzol
{
    class Program
    {

        static cnIT_Inventory cnIT_Inventory;

        /// <summary>
        /// Ez a függvény meghívja az Adatlekérdez metódust és beolvassa a sorokat. Példányosítom a cnIT_Inventoryt.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            cnIT_Inventory = new cnIT_Inventory();
            //Adatfelvitel();
            Adatlekerdez();
            Console.ReadLine();
        }
           
        /// <summary>
        /// Ez a metódus feltölt egy listát adattal, majd hozzáadja az adatbázishoz és elmenti, teszt célja van.
        /// </summary>
        private static void Adatfelvitel()
        {
            var k = new TE_Leltar { Nev = "Asztali gép", Hely = "IT", Felhasznalo = "Gipsz Jakab", Csoport = "IT", 
                Statusz = "Használva", Tipusok = "ThinkCentre", Gyarto = "Lenovo", Modell = "K23", Leltari_Szam = "0111", Sorozatszam = "5006kd" };

            cnIT_Inventory.TE_Leltar.Add(k);
            cnIT_Inventory.SaveChanges();
        }

        /// <summary>
        /// Ez a metódus lekérdezi az összes felhasználót és helyet az adatbázisból egy listába és kiirja.
        /// </summary>
        private static void Adatlekerdez()
        {
            Console.WriteLine("Össze adat\r\n---------------------");
            foreach(var x in cnIT_Inventory.TE_Leltar)
            {
                var s = x.Felhasznalo + " " + x.Hely;
                Console.WriteLine(s);
            }
        }
    }
}
