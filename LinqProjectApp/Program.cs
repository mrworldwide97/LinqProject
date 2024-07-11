using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        // Chemin d'accès relatif aux fichiers dans le répertoire de sortie
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
        string xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");
        string transformedXmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transformed_data.xml");

        // Lire et afficher des données JSON
        var jsonData = File.ReadAllText(jsonFilePath);
        var personsFromJson = JsonConvert.DeserializeObject<List<Person>>(jsonData);

        // Lire et afficher des données XML
        XDocument xdoc = XDocument.Load(xmlFilePath);
        var personsFromXml = xdoc.Descendants("Person")
                                 .Select(x => new Person
                                 {
                                     Name = x.Element("Name").Value,
                                     Age = int.Parse(x.Element("Age").Value)
                                 }).ToList();

        while (true)
        {
            Console.WriteLine("\nChoisissez une option:");
            Console.WriteLine("1. Afficher les données JSON");
            Console.WriteLine("2. Afficher les données XML");
            Console.WriteLine("3. Rechercher des adultes dans les données JSON");
            Console.WriteLine("4. Trier les personnes par âge dans les données XML");
            Console.WriteLine("5. Transformer les données JSON en XML");
            Console.WriteLine("6. Rechercher par nom dans les deux sources");
            Console.WriteLine("7. Trier les personnes par nom dans les deux sources");
            Console.WriteLine("0. Quitter");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Données JSON:");
                    foreach (var person in personsFromJson)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }
                    break;
                case "2":
                    Console.WriteLine("\nDonnées XML:");
                    foreach (var person in personsFromXml)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }
                    break;
                case "3":
                    var adultsFromJson = personsFromJson.Where(p => p.Age > 18).ToList();
                    Console.WriteLine("\nAdultes (JSON):");
                    foreach (var adult in adultsFromJson)
                    {
                        Console.WriteLine($"{adult.Name}, {adult.Age}");
                    }
                    break;
                case "4":
                    var sortedPersonsFromXml = personsFromXml.OrderBy(p => p.Age).ToList();
                    Console.WriteLine("\nPersonnes triées par âge (XML):");
                    foreach (var person in sortedPersonsFromXml)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }
                    break;
                case "5":
                    var transformedXml = new XElement("Persons",
                        personsFromJson.Select(p => new XElement("Person",
                            new XElement("Name", p.Name),
                            new XElement("Age", p.Age)
                        ))
                    );
                    transformedXml.Save(transformedXmlFilePath);
                    Console.WriteLine($"\nLes données JSON ont été transformées en XML et sauvegardées dans {transformedXmlFilePath}");
                    break;
                case "6":
                    Console.WriteLine("Entrez le nom à rechercher:");
                    string searchName = Console.ReadLine();
                    var foundInJson = personsFromJson.Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
                    var foundInXml = personsFromXml.Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

                    Console.WriteLine($"\nRecherche de {searchName} dans JSON:");
                    foreach (var person in foundInJson)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }

                    Console.WriteLine($"\nRecherche de {searchName} dans XML:");
                    foreach (var person in foundInXml)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }
                    break;
                case "7":
                    var sortedByNameInJson = personsFromJson.OrderBy(p => p.Name).ToList();
                    var sortedByNameInXml = personsFromXml.OrderBy(p => p.Name).ToList();

                    Console.WriteLine("\nPersonnes triées par nom (JSON):");
                    foreach (var person in sortedByNameInJson)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }

                    Console.WriteLine("\nPersonnes triées par nom (XML):");
                    foreach (var person in sortedByNameInXml)
                    {
                        Console.WriteLine($"{person.Name}, {person.Age}");
                    }
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide, veuillez réessayer.");
                    break;
            }
        }
    }
}
