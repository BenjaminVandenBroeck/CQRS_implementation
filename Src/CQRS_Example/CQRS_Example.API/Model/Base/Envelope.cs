using NodaTime;

namespace CQRS_Example.API.Model.Base
{
    public class Envelope<T>
    {
        protected internal Envelope(T result)
        {
            Result = result;
            TimeGenerated = SystemClock.Instance.GetCurrentInstant();
        }

        public T Result { get; private set; }
        public Instant TimeGenerated { get; private set; }
    }
}
