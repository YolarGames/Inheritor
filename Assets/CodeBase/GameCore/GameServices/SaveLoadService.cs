using System.Threading.Tasks;

namespace GameCore.GameServices
{
	public class SaveLoadService : ISaveLoadService
	{
		public Task Init()
		{
			return Task.CompletedTask;
		}
	}

	public interface ISaveLoadService : IService { }
}