namespace CQRS_Example.API.Model.Base
{
    public class ErrorEnvelope: Envelope<string>
    {
        protected internal ErrorEnvelope(string result, string errorIdentifier): base(result)
        {
            ErrorIdentifier = errorIdentifier;
        }

        public string ErrorIdentifier { get; private set; }
    }
}
