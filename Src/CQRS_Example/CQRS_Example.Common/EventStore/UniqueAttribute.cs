namespace CQRS_Example.Common.EventStore
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =true)]
    public class UniqueAttribute: Attribute
    {
    }
}
