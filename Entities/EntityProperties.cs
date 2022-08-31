using System.Collections.Generic;

namespace AdventureGame
{
    /*
    public class EntityProperties
    {
        public ItemProperties Items { get; set; }
    }
    */

    // Converted using https://json2csharp.com/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Filename { get; set; }
    }

    public class Item
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Filename { get; set; }
        public List<string> Collectable { get; set; }
    }

    public class Map
    {
        public string Filename { get; set; }
        public List<Trigger> Triggers { get; set; }
    }

    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Filename { get; set; }
        public List<Spritesheet> Spritesheet { get; set; }
    }

    public class Root
    {
        public Map Map { get; set; }
        public List<Player> Players { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }
        public string TestProperty { get; set; }
    }

    public class Spritesheet
    {
        public string Key { get; set; }
        public List<List<int>> Sprite { get; set; }
    }

    public class Trigger
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

}
