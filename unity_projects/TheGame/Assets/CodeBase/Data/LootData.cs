using System;

namespace CodeBase.Data {
	[Serializable]
	public class LootData {
		public int Collected;
		public LootPieceDataDictionary LootPiecesOnScene = new LootPieceDataDictionary();

		public event Action Changed;

		public void Collect(Loot loot) =>
			Collect(loot.Value);

		public void Collect(int value) {
			Collected += value;
			Changed?.Invoke();
		}
	}
}