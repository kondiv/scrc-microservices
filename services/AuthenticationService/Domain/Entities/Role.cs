namespace Domain.Entities;

public class Role
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public string NormalizedName { get; }
    public virtual ICollection<User> Users { get; private set; } = [];

    public Role(string name)
    {
        Name = name;
        NormalizedName = name.Replace(" ", "").ToUpper();
    }
}