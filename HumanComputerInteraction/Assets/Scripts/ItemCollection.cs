using System.Collections.Generic;

[System.Serializable]
public class ItemCollection
{
    public List<ItemEntry> items;

    [System.Serializable]
    public class ItemEntry
    {
        public string key;
        public Item value;
    }

    public Dictionary<string, Item> ToDictionary()
    {
        Dictionary<string, Item> dict = new Dictionary<string, Item>();
        foreach (ItemEntry entry in items)
        {
            dict[entry.key] = entry.value;
        }
        return dict;
    }
}