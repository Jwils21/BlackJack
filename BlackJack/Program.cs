using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack {
	class Program {

		static void PlayBlackJack(List<Player> Players, Dealer dealer, CardDeck cardDecks) {
			//Place a bet
			foreach(Player player in Players) {
				Console.WriteLine($"How much would you like to bet {player.Name}? Your balance is: ${player.Chips}");
				//Capture bet
				long BetAmt = Convert.ToInt64(Console.ReadLine());
				player.MakeBet(BetAmt);
			}

			//Deal cards
			cardDecks.DealCards(Players, dealer);
			cardDecks.DisplayAllCards(Players, dealer);
			System.Threading.Thread.Sleep(2000);
			//Set up player action
			foreach(Player player in Players) {
				Console.WriteLine("------------------------------------------------------------------------");
				cardDecks.PlayerCardAction(player, dealer);
			}
			cardDecks.DealerCardAction(dealer);
			List<Player> RemovePlayers = new List<Player>();
			foreach(Player player in Players) {
				if(dealer.Score >= player.Score) {
					Console.WriteLine($"{player.Name}, you lost {player.BetAmt} chips.");
					player.Chips -= player.BetAmt;
					if(player.Chips == 0) {
						RemovePlayers.Add(player);
					}
				} else {
					if(player.HasBlackJack == true) {
						player.BetAmt *= 2;
						Console.WriteLine($"Winner Winner Chicken Dinner {player.Name}! You had blackjack and win {player.BetAmt}! That is double your bet.");
						player.Chips += player.BetAmt;
					} else {
						Console.WriteLine($"{player.Name}, you won {player.BetAmt} chips.");
						player.Chips += player.BetAmt;
					}
				}
			}
			//Remove Bankrupt players
			foreach (Player player in RemovePlayers) {
				Console.WriteLine($"{player.Name}, you are bankrupt. We have kicked you out of the casino.");
				System.Threading.Thread.Sleep(1000);
				Players.Remove(player);
			}
			foreach(Player player in Players) {
				player.ClearDealerData(player);
			}
			dealer.ClearDealerData(dealer);
			//reset card deck
			cardDecks.SetResetDeck();
		}

		static void Main(string[] args) {
			//Initialize deck
			CardDeck CardDecks = new CardDeck();
			CardDecks.SetResetDeck();

			//Set up dealer
			Dealer dealer = new Dealer();

			//Start the game
			Console.WriteLine("----------------------------------------");
			Console.WriteLine("Welcome to BlackJack");
			Console.WriteLine("----------------------------------------");


			//Create Players
			int PlayerCnt = 1;
			Player Player2 = new Player();
			Player Player3 = new Player();
			Player Player4 = new Player();

			Console.WriteLine("Please enter player name");
			String PlayerName = Console.ReadLine();
			Player Player1 = new Player(PlayerName);
			Console.WriteLine($"Player '{Player1.Name}' created. Would you like to add another player?");
			//Case to create player 2
			if(Console.ReadLine().ToLower() != "no") {
				Console.WriteLine("Please enter player name");
				PlayerName = Console.ReadLine();
				Player2 = new Player(PlayerName);
				Console.WriteLine($"Player '{Player2.Name}' created. Would you like to add another player?");
				PlayerCnt++;
				//case to create player 3
				if(Console.ReadLine().ToLower() != "no") {
					Console.WriteLine("Please enter player name");
					PlayerName = Console.ReadLine();
					Player3 = new Player(PlayerName);
					Console.WriteLine($"Player '{Player3.Name}' created. Would you like to add another player?");
					PlayerCnt++;
					//case to create player 4
					if(Console.ReadLine().ToLower() != "no") {
						Console.WriteLine("Please enter player name");
						PlayerName = Console.ReadLine();
						Player4 = new Player(PlayerName);
						Console.WriteLine($"Player '{Player4.Name}' created.");
						PlayerCnt++;
					}
				}
			}
			Console.WriteLine("Lets start!");
			List<Player> Players = new List<Player>();
			//Create a list of players based on players created
			switch(PlayerCnt) {
				case 1:
					Players.Add(Player1);
					break;
				case 2:
					Players.Add(Player1);
					Players.Add(Player2);
					break;
				case 3:
					Players.Add(Player1);
					Players.Add(Player2);
					Players.Add(Player3);
					break;
				case 4:
					Players.Add(Player1);
					Players.Add(Player2);
					Players.Add(Player3);
					Players.Add(Player4);
					break;
			}
			string ContinueGame;
			do {
				PlayBlackJack(Players, dealer, CardDecks);
				Console.WriteLine("Would you like to play again?");
				ContinueGame = Console.ReadLine().ToLower();
			} while(ContinueGame != "no");
		}
	}
}
