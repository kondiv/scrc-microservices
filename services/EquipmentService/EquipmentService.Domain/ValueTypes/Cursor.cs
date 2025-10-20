using System.Text;
using System.Text.Json;

namespace EquipmentService.Domain.ValueTypes;

public sealed record Cursor(DateTime Date, Guid LastId)
{
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static string Encode(DateTime date, Guid lastId)
    {
        var cursor = new Cursor(date, lastId);
        var json = JsonSerializer.Serialize(cursor, SerializerOptions);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    public static Cursor? Decode(string cursor)
    {
        if (string.IsNullOrEmpty(cursor))
        {
            return null;
        }

        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            return JsonSerializer.Deserialize<Cursor>(json, SerializerOptions);
        }
        catch (Exception e)
        {
            return null;
        }
    }
}