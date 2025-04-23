namespace MyApi.Models;

public class Gift
{
    public int Id {get; set;}

    public string Name { get; set; }
    public int UserId { get; set; }

    public int Price { get; set; }

    public string? Summary { get; set; }
}