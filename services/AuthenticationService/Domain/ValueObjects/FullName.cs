namespace Domain.ValueObjects;

public record FullName(string FirstName, string Surname, string Patronymic)
{
    public override string ToString()
    {
        return string.Join(' ', new [] {Surname, FirstName, Patronymic}.Where(s => !string.IsNullOrEmpty(s)));
    }
}