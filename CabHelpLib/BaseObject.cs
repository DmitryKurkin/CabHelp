namespace Emerson.Common
{
    using AcyclicVisitor;

    public abstract class BaseObject
    {
        public abstract void Accept(IVisitor<BaseObject> visitor);
    }
}