public abstract class Item : ICollectible
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    protected Item(string name, string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public abstract void OnCollected(Player player);
    public abstract void Use(Player player);
}