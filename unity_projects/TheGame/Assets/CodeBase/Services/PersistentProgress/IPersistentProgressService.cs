namespace CodeBase.Services.PersistentProgress {
	public interface IPersistentProgressService : IService {
		Data.PlayerProgress Progress { get; set; }
	}
}