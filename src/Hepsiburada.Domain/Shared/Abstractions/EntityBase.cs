namespace Hepsiburada.Domain.Shared.Abstractions
{
    public class EntityBase : IEntityBase
    {
        public virtual int Id { get; protected set; }

        //SetId methodunu asıl entity nesnelerınde gorunmeyecek şekilde gizliyoruz.
        void IEntityBase.SetId(int id)
        {
            Id = id;
        }
    }
}
