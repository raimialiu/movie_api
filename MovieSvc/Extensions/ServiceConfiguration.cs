namespace MovieSvc.Extensions;

public class ServiceConfiguration<T>
{
    public T ServiceCaller { get; set; }
    public string LifeCycle { get; set; }
    public object Params { get; set; }
}