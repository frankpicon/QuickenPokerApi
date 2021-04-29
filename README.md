# QuickenPokerApi

##########################################################################
##########################################################################
FRANK PICON - .NET Core API for Quicken
##########################################################################
##########################################################################

Following is the endpoint for this api 

GET https://localhost/pokergame
return JSON Payload List<PlayerViewModel>

POST https://localhost/pokergame
ACCEPT JSON Payload PlayersViewModel (which consists of a list of PokerPlayers)
return JSON Payload List<WinnerViewModel> 

NOTES:
The API can generate its own default 2 players if an empty JSON file is posted. 
If No payload is sent with players and cards, the api will generate a default player and randomly generate 5 cards and return the players if GET or Winners if POST

FEATURES
-Poker Game can support multiple players
-API Winner accepts a list of players along with cards and the Resultset returns a JSON payload of Winner along with list of players who played, all the players cards and each players highest card of their hand
-API Rule Types (High Cards) are data driven and values can change by changing the HandType values ie Straight Flush, Full House, etc.. are rules 
-Factory Pattern Implemented
-Dependency Injection Implemented
-Singleton Implemented
-The Models were generated using the Entity Framework Core database first approach. DDLs for DB is missing but can be provided. 
-View Models implemented
-Custom Base Controller and Services Implemented

MISSING
-Authentication -have placeholders for generating JWT Tokens and custom Token Controller Attribute which will be implemented in the middleware.
-Unit Testing were not created however have methods that will create custom MOCK objects 
-Some DI IOC is implemented using .Net Core default IOC
-Logging is missing but began some implementation and injected logging service

POST api will accept a JSON Payload in the body that will consist of the following

PLAYER
  CARDS
  HAND
    HANDTYPE

SAMPLE JSON Post 


