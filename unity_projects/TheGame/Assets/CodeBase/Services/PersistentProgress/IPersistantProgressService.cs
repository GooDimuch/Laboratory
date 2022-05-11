namespace CodeBase.Services.PersistentProgress {
	public interface IPersistantProgressService : IService {
		Data.PlayerProgress Progress { get; set; }
	}
}