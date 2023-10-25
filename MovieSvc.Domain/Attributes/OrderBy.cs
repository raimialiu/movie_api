namespace MovieSvc.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OrderByColumn : Attribute
{

}
    
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SortOrder : Attribute
{
    public class OrderBy
    {
        public const string Ascending = "asc";
        public const string Descending = "desc";
    }
    public SortOrder(string order, string orderBy="ASC")
    { 
        Order = orderBy;
    }
    public SortOrder(string order)
    {
        Order = order;
    }

    public string Order { get; private set; }
}