namespace CodeBase.Services {
	public class AllServices {
		private static AllServices _container;
		public static AllServices Container => _container ??= new AllServices();

		public void RegisterSingle<TService>(TService implementation) where TService : IService =>
			Implementation<TService>.ServiceInstance = implementation;

		public TService Single<TService>() where TService : IService => 
			Implementation<TService>.ServiceInstance;

		private static class Implementation<TService> where TService : IService {
			public static TService ServiceInstance;
		}
	}
}