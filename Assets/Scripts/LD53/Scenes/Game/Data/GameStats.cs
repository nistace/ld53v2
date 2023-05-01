namespace LD53.Scenes.Game.Data {
	public class GameStats {
		public int  score              { get; set; }
		public int  crashes            { get; set; }
		public int  deliveries         { get; set; }
		public bool onlineDataGathered { get; set; }
		public int  onlineRanking      { get; set; }
		public int  entriesInDb        { get; set; }
	}
}