using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack {
	public class Dealer {
		public List<string> Hand { get; set; }
		public int Score { get; set; }

		public Dealer() {
			Hand = new List<string>();
		}

		public void ClearDealerData(Dealer dealer) {
				dealer.Hand.Clear();
				dealer.Score = 0;
		}

	}
}
