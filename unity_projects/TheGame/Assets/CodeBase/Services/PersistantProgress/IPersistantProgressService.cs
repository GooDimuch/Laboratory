﻿namespace CodeBase.Services.PersistantProgress {
	public interface IPersistantProgressService : IService {
		Data.PlayerProgress Progress { get; set; }
	}
}