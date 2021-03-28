namespace curso.api.Business.Repositories
{
	public interface IRepository<T>
	{
		void Adicionar(T entidade);
		void Commit();
	}
}
