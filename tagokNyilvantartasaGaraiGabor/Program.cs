using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace tagokNyilvantartasaGaraiGabor
{
    internal class Program
    {
        static List<Tag> tagList = new List<Tag>();
        static MySqlConnection connection = null;
        static MySqlCommand command = null;
        static void Beolvas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "tagdij";
            sb.CharacterSet = "utf8";
            connection = new MySqlConnection(sb.ConnectionString);
            command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT * FROM `ugyfel`";
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tag tag = new Tag(dr.GetInt32("azon"), dr.GetString("nev"), dr.GetInt32("szulev"), dr.GetInt32("irszam"), dr.GetString("orsz"));
                        tagList.Add(tag);
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {

                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        private static void UjTagFelvetele(int azon, string nev, int szulev, int irszam, string orsz)
        {
            Tag newTag = new Tag(azon,nev,szulev,irszam,orsz);
            command.CommandText = "INSERT INTO `ugyfel`(`azon`, `nev`, `szulev`, `irszam`, `orsz`) VALUES (@azon,@nev,@szulev,@irszam,@orsz)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", newTag.Azon);
            command.Parameters.AddWithValue("@nev", newTag.Nev);
            command.Parameters.AddWithValue("@szulev", newTag.Szulev);
            command.Parameters.AddWithValue("@irszam", newTag.Irszam);
            command.Parameters.AddWithValue("@orsz", newTag.Orsz);
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"A(z) {newTag.Azon} azonosítójú tag sikeresen felvéve.");
                    Console.WriteLine();
                    tagList.Add(newTag);
                }
                else
                {
                    Console.WriteLine($"Nem sikerült a(z) {newTag.Azon} azonosítójú tag felvétele.");
                    Console.WriteLine();
                }

                connection.Close();
            }
            catch (MySqlException ex)
            {

                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

        }

        private static void TagTorlese(int azonosito)
        {

            command.CommandText = "DELETE FROM `ugyfel` WHERE `azon` = @azon";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", azonosito);

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"A(z) {azonosito} azonosítójú tag sikeresen törölve.");
                    Console.WriteLine();
                    Tag removedTag = tagList.SingleOrDefault(tag => tag.Azon == azonosito);
                    if (removedTag != null)
                    {
                        tagList.Remove(removedTag);
                    }
                }
                else
                {
                    Console.WriteLine($"Nem található a(z) {azonosito} azonosítójú tag.");
                    Console.WriteLine();
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TagokListazasa()
        {
            for (int i = 0; i < tagList.Count; i++)
            {
                Console.WriteLine(  $"#{i+1}\n" +
                                    $"{tagList[i]}");
            }
        }
        static void Main(string[] args)
        {
            Beolvas();
            TagokListazasa();
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            //Az UjTagFelvetele() metódust csak akkor futassuk le, ha nincs az adatbázisban a megadott azonosítóval megegyező tag
            UjTagFelvetele(17,"Kovács Béla",1971,9663,"H");
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            TagokListazasa();
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            TagTorlese(17);
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            TagokListazasa();
            Console.WriteLine("\nVége!");
            Console.ReadKey();
        }

    }
}
