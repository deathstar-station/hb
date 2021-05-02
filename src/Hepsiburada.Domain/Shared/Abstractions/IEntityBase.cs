namespace Hepsiburada.Domain.Shared.Abstractions
{
    public interface IEntityBase
    {
        public int Id { get; }

        public void SetId(int id);
    }
}
