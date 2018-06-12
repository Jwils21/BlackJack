using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack {
	public class Player: Dealer {
		public String Name { get; set; }
		public long Chips { get; set; }
		public long BetAmt { get; set; }
		public bool HasBlackJack { get; set; }




		static void AddPlayer(Dictionary<string, long> PlayerDict, string PlayerName) {
			PlayerDict.Add(PlayerName, 500);
		}

		////Method to make a bet
		public void MakeBet(long betAmt) {
			while(Chips < betAmt) {
				Console.WriteLine($"You do not have enough chips... Please place a bet less than or equal to {Chips}");
				betAmt =Convert.ToInt64( Console.ReadLine());
			}
			BetAmt = betAmt;
		}

		public void ClearPlayerData(Player player) {
			player.Hand.Clear();
			player.Score = 0;
			player.HasBlackJack = false;
		}

		public Player(string PlayerName) {
			Name = PlayerName;
			Chips = 500;
			HasBlackJack = false;
		}

		public Player() {

		}
	}
}
