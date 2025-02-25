namespace CQRS_Example.Common.EventStore
{
    public class UniqueCheck
    {
        public required string Id { get; set; } 
        public required string PropertyName { get; set; }
        public required string UniqueValue { get; set; }    
    }
}
