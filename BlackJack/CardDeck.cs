using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack {
	
	class CardDeck {
		public List<string> Decks { get; set; }
		public List<int> DrawnCards { get; set; }
		//Get random numbers
		static Random rnd = new Random();

		public void SetResetDeck() {
			Decks.Clear();

			//Generate cards for deck that are numbers
			for(int i = 2;i < 11;i++) {
				for(int NumofSuits = 1;NumofSuits < 5;NumofSuits++) {
					Decks.Add(i.ToString());
				}
			}

			//Generate Face cards
			string[] FaceCards = { "J", "Q", "K", "A" };
			for(int i = 0;i < (FaceCards.Length);i++)
				for(int NumofSuits = 1;NumofSuits < 5;NumofSuits++) {
					Decks.Add(FaceCards[i]);
				}
		}

		public string AddCard() {
			bool IsNewCard = false;
			int CardIdx;
			string Card;
			//Grab random number of a card
			CardIdx = rnd.Next(0, 52);
			//Test to see if that card has already been drawn
			while(IsNewCard == false) {
				IsNewCard = true;
				foreach(int card in DrawnCards) {
					if(CardIdx == card) {
						IsNewCard = false;
						CardIdx = rnd.Next(0, 52);
					}
				}
			}
			DrawnCards.Add(CardIdx);
			//return the string of that card
			return Card=Decks[CardIdx];
		}

		public void DealCards(List<Player> Players, Dealer dealer) {
			//Deal cards one at a time to the players
			//Loop twice to give each player a card
			for(int idx = 0;idx < 2;idx++) {
				//Dealer gets dealt to first
				dealer.Hand.Add(AddCard());
				foreach(Player player in Players) {
					player.Hand.Add(AddCard());
				}

			}
			
		}
		
		public void DisplayAllCards(List<Player> Players, Dealer dealer) {
			DisplayDealerCards(dealer);

			foreach(Player player in Players) {
				DisplayPlayerCards(player);
			}
		}

		public void PlayerCardAction(Player player, Dealer dealer) {
			string PlayerAction = null;
			bool IsBusted;
			do {
				DisplayDealerCards(dealer);
				DisplayPlayerCards(player);
				IsBusted = EvaluatePlayerHand(player);
				if(IsBusted == true) {
					Console.WriteLine($"{player.Name}, you have busted!");
					player.Score = 0;
					System.Threading.Thread.Sleep(3000);
					break;
				}
				Console.WriteLine($"{player.Name}, would you like to hit? Yes or No");
				PlayerAction = Console.ReadLine().ToLower();
				if(PlayerAction == "yes") {
					player.Hand.Add(AddCard());
				}
				if (player.Hand.Count == 2 && player.Score == 21) {
					player.HasBlackJack = true;
				}
			}
			while(PlayerAction != "no");

		}

		public void DealerCardAction(Dealer dealer) {
			bool IsBusted;
			do {
				DisplayDealerCards(dealer, true);
				IsBusted = EvaluateDealerHand(dealer);
				if(dealer.Score < 17) {
					dealer.Hand.Add(AddCard());
					Console.WriteLine($"Dealer hits");
					System.Threading.Thread.Sleep(1500);
				} else {
					Console.WriteLine($"Dealer stays");
					System.Threading.Thread.Sleep(3000);
				}
				IsBusted = EvaluateDealerHand(dealer);
				if(IsBusted == true) {
					DisplayDealerCards(dealer, true);
					Console.WriteLine($"The dealer has busted!");
					dealer.Score = 0;
					System.Threading.Thread.Sleep(3000);
					break;
				}

			} while(dealer.Score < 17);
			DisplayDealerCards(dealer, true);

		}

		public void DisplayPlayerCards(Player player) {
			int PlayerCards = player.Hand.Count;
			string CardOutline = null;
			string PlayerHand = null;
			Console.WriteLine($"{player.Name}, your hand is...");

			for(int i = 0;i < (PlayerCards);i++) {
				if(player.Hand[i] == "10") {
					CardOutline += "----  ";
				} else {
					CardOutline += "---  ";
				}
				PlayerHand += $"|{player.Hand[i]}|  ";
			}

			Console.WriteLine(CardOutline);
			Console.WriteLine(PlayerHand);
			Console.WriteLine(CardOutline);
		}

		public void DisplayDealerCards(Dealer dealer, bool ShowAll = false) {
			int DealerCards = dealer.Hand.Count;
			string CardOutline = null;
			string DealerHand = null;
			Console.WriteLine($"The dealer shows...");
			//Generate first card outline
			if(ShowAll == true) {
				for(int i = 0;i < (DealerCards);i++) {
					if(dealer.Hand[i] == "10") {
						CardOutline += "----  ";
					} else {
						CardOutline += "---  ";
					}
					DealerHand += $"|{dealer.Hand[i]}|  ";
				}
			} else {
				CardOutline += "---  ";
				DealerHand += $"|?|  ";
				for(int i = 1;i < (DealerCards);i++) {
					if(dealer.Hand[i] == "10") {
						CardOutline += "----  ";
					} else {
						CardOutline += "---  ";
					}
					DealerHand += $"|{dealer.Hand[i]}|  ";
				}
			}

			Console.WriteLine(CardOutline);
			Console.WriteLine(DealerHand);
			Console.WriteLine(CardOutline);
		}

		public CardDeck() {
			Decks = new List<string>();
			DrawnCards = new List<int>();
		}

		public bool EvaluateDealerHand(Dealer dealer) {
			int TotScore = CardMath(dealer.Hand);
			dealer.Score = TotScore;
			if(TotScore > 21) {
				return true;
			} else {
				return false;
			}
			// Determine if they busted or can keep playing.
		}

		public bool EvaluatePlayerHand(Player player) {
			int TotScore = CardMath(player.Hand);
			player.Score = TotScore;
			if(TotScore > 21) {
				return true;
			} else {
				return false;
			}
			// Determine if they busted or can keep playing.
		}

		public int CardMath(List<string> Cards) {
			int CardVal;
			int CurrScore=0;
			List<int> ScoreList = new List<int>();
			foreach(string card in Cards) {
				if(card == "A") {
					CardVal = 11;
				} else if(card == "J" || card == "Q" || card == "K") {
					CardVal = 10;
				} else {
					CardVal = Convert.ToInt32(card);
				}
				ScoreList.Add(CardVal);
			}
			ScoreList.Sort();
			//Count the number of aces to specify their values
			int Ace = 11;
			int AceCntr = 0;
			foreach(int score in ScoreList) {
				if (score == Ace) {
					AceCntr++;
				}
			}

			//Calculate the score
			foreach(int score in ScoreList) {
				if(score == 11) {
					if (CurrScore > 10) {
						CurrScore += 1;
					} else if (CurrScore == 10) {
						if(score == ScoreList.Last()) {
							CurrScore += score;
						} else {
							CurrScore += 1;
						}
					} else {
						CurrScore = score;
					}
				} else {
					CurrScore += score;
				}
			}
			return CurrScore;
		}

	}
}
