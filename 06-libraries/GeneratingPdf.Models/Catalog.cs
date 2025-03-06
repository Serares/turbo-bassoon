namespace GeneratingPdf.Models;

public class Catalog
{
    public List<Category> Categories { get; set; } = null!; //I know this looks like it could be null, but trust me, it won't be null when it's actually used
}