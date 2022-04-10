using CodeBase.Data;

namespace CodeBase.Services.SaveLoadService {
	public interface ISaveLoadService : IService {
		PlayerProgress LoadProgress();
		void SaveProgress();
	}
}