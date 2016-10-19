using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJackAssignment
{
    class Program
    {

        /*
         * Bryony Burniston
         * S00150642
         * 14th December 2014
         * 
         * Program Description:
         * Console Application will mimic a game of Blackjack (21)
         * Game Rules: Hands over 21 go 'bust', Ace worth 11, Jack, Queen, King worth 10, rest of cards face value
         * Player goes first, dealt 2 cards, can stick or twist(dealt another card) unless goes bust
         * When player sticks, dealer dealt 2 cards, if score < 17 dealt another card, else stick
         * Highest score wins
         * 
         * Algorithm:
         * 1.Declare required variables
         * 2.Initialise array to hold cards
         * 3.Start game loop
         * 4.Player dealt 2 cards - display values and score
         *      4a. Check if bust
         * 5.ask player to stick or twist (loop til stick/bust)
         *      5a. if twist deal another card - display value and score
         *      5b. if stick end this loop - dealers turn
         * 6. Dealer Plays
         *      6a. Dealt 2 cards - display values and score
         *      6b. Check if bust
         *      6c. If score < 17 deal another card - display value and score (loop til score >=17 or bust)
         *      6d. If score >= 17 dealer sticks
         * 7. Compare Scores, say who wins or draw
         * 8. Ask player if they want to play again
         *      8a. If yes reset deck and continue game loop
         *      8b. If no exit game loop 
         *  
         */

        //Class variables
        //Initialise array for deck of 52 cards - to be filled by ResetDeck method below
        static int[,] deck = new int[4, 13];

        static Random randomNumber = new Random();

        //Main method
        static void Main(string[] args)
        {
            //declare variables
            bool playGame = true;
            int playerScore = 0;
            int dealerScore = 0;
            bool isPlayerBust = false;            
            bool isDealerBust = false;
            string playerInput = "";

            do //start of game loop
            {
                //Clear console
                Console.Clear();

                //New deck of cards
                ResetDeck();

                //Reset Variables for new game
                playerScore = 0;
                dealerScore = 0;
                isPlayerBust = false;                
                isDealerBust = false;
                playerInput = "";
                
                //Player starts

                //Deal 2 cards to player hand, display cards dealt, add values to score
                playerScore += DealCard();
                playerScore += DealCard();

                //Display current player score
                Console.WriteLine("Your score is {0}", playerScore);

                //Player loop til bust or stick
                while (!isPlayerBust)
                {
                    //Check if bust - game over
                    if (playerScore > 21)
                    {
                        isPlayerBust = true;
                        Console.WriteLine("Your score is over 21, BUST, sorry game over, dealer wins!");
                        break; //ends player loop
                    }
                    else
                    {
                        //Ask player if wants to stick or twist
                        //Check for valid entry
                        bool isValidEntry = false;

                        while (!isValidEntry)
                        {
                            Console.WriteLine("Do you want to stick or twist - s/t?");
                            playerInput = Console.ReadLine().ToLower();

                            if (playerInput == "s" || playerInput == "t") //valid input
                            {
                                isValidEntry = true;
                                break; //end while loop, continue with game                                
                            }                            
                        }//end validation while loop               
                                                
                        //If twist, deal another card - update score, continue player loop
                        if (playerInput == "t")
                        {
                            playerScore += DealCard();
                            Console.WriteLine("Your score is {0}", playerScore);
                        }                         
                        else if (playerInput == "s")
                        {                            
                            break; //ends player loop
                        }                        
                    }
                } //end player loop

                //if player is bust dealer wins without playing
                if (!isPlayerBust)//if player not bust then dealer's turn
                {
                    //Dealer turn
                    Console.WriteLine("Dealer plays");

                    //Deal 2 cards to dealer hand, display cards dealt, add values to score
                    dealerScore += DealCard();
                    dealerScore += DealCard();

                    //Display current dealer score
                    Console.WriteLine("Dealer's score is {0}", dealerScore);

                    //Dealer loop
                    while (!isDealerBust)
                    {
                        //Check if bust - game over
                        if (dealerScore > 21)
                        {
                            isDealerBust = true;
                            Console.WriteLine("Dealer's score is over 21, BUST, player wins!");
                            break; //ends dealer loop
                        }
                        else
                        {
                            //If score < 17 deal another card - display card and update score
                            if (dealerScore < 17)
                            {
                                dealerScore += DealCard();
                                Console.WriteLine("Dealer's score is {0}", dealerScore);
                            }
                            else //dealer sticks
                            {
                                Console.WriteLine("Dealer score 17 or more, dealer sticks");
                                break; //ends dealer loop
                            }
                        }
                    } //end dealer loop
                }//end dealer turn


                //Compare scores & display winner
                if (!isPlayerBust && !isDealerBust)
                {
                    if (playerScore == dealerScore)
                    {
                        Console.WriteLine("It's a draw!");
                    }
                    else if (playerScore > dealerScore)
                    {
                        Console.WriteLine("Player wins!");
                    }
                    else //dealer's score > player's score
                    {
                        Console.WriteLine("Dealer wins!");
                    }
                }//end score compare

                //Ask player if wants to play again
                //Check for valid entry
                bool isValid = false;
                string playOption = "";

                while (!isValid)
                {
                    Console.WriteLine("Do you want to play again? y/n");
                    playOption = Console.ReadLine().ToLower();

                    if (playOption == "y" || playOption == "n") //valid input
                    {
                        isValid = true;
                        break; //end while loop, continue with game                                
                    }
                }//end validation while loop   
                //exit option
                if (playOption == "n")
                {
                    playGame = false;
                    Console.WriteLine("press enter to exit");
                }

            } while (playGame); //end of game do/while loop

            Console.ReadLine();

        }//end of main method

        //Method to fill deck with 52 cards
        public static void ResetDeck()
        {
            for (int i = 0; i < 4; i++) //4 suits
            {
                for (int j = 0; j < 13; j++)
                {
                    deck[i, j] = j; //values 0 - 12 to represent 13 cards                     
                }
            }
        }//end of method

        //Method to deal a card
        public static int DealCard()
        {
            int suit = -1;
            int face = -1;
            bool cardFound = false;
            int cardValue = 0;

            //Loop til an undealt card is found
            while (cardFound == false)
            {
                //Random number generated to select suit
                int suitNumber = randomNumber.Next(0, 4);

                //Random number generated to select card
                int faceNumber = randomNumber.Next(0, 13);

                //If element value is not -1 (not yet dealt), assign values to suit and face, change relevant deck array element value to -1
                if (deck[suitNumber, faceNumber] != -1)
                {
                    suit = suitNumber;
                    face = faceNumber;
                    deck[suitNumber, faceNumber] = -1; //indicating that this card has now been dealt 
                    cardFound = true;
                }
            }//end of while loop

            //Get card value
            cardValue = GetCardValue(face); //note suit is irrelevant for scoring          

            //Display the card dealt and its value
            DisplayCard(face, suit, cardValue);

            //return the value of the card dealt
            return cardValue;
        }//end of method

        //Method to convert card dealt to value
        public static int GetCardValue(int faceNumber)
        {
            int cardValue = 0;

            if (faceNumber == 0) //represents an Ace
            {
                cardValue = 11;
            }
            else if (faceNumber == 9 || faceNumber == 10 || faceNumber == 11 || faceNumber == 12) //10, Jack, Queen, King
            {
                cardValue = 10;
            }
            else //other cards
            {
                cardValue = faceNumber + 1; //array indices being 1-8 need to add 1 to get face value
            }

            return cardValue;
        }//end of method

        //Helper method to display a dealt card and its value - called when a card is dealt
        public static void DisplayCard(int faceNumber, int suitNumber, int cardValue)
        {
            string suit = "";
            string face = "";

            switch (suitNumber)
            {
                case 0:
                    suit = "Clubs";
                    break;
                case 1:
                    suit = "Diamonds";
                    break;
                case 2:
                    suit = "Hearts";
                    break;
                case 3:
                    suit = "Spades";
                    break;
                default:
                    suit = "Error";
                    break;
            }

            switch (faceNumber)
            {
                case 0:
                    face = "Ace";
                    break;
                case 1:
                    face = "2";
                    break;
                case 2:
                    face = "3";
                    break;
                case 3:
                    face = "4";
                    break;
                case 4:
                    face = "5";
                    break;
                case 5:
                    face = "6";
                    break;
                case 6:
                    face = "7";
                    break;
                case 7:
                    face = "8";
                    break;
                case 8:
                    face = "9";
                    break;
                case 9:
                    face = "10";
                    break;
                case 10:
                    face = "Jack";
                    break;
                case 11:
                    face = "Queen";
                    break;
                case 12:
                    face = "King";
                    break;
                default:
                    face = "Error";
                    break;
            }

            Console.WriteLine("Card dealt is the {0} of {1}, value {2}", face, suit, cardValue);

        } //end of method       

    }//end of class
}
