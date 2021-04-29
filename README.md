# QuickenPokerApi

##########################################################################
##########################################################################
FRANK PICON - .NET Core API for Quicken
##########################################################################
##########################################################################

Following is the endpoint for this api 

GET https://localhost/pokergame
POST https://localhost/pokergame


NOTES:
The API can generate its own default 2 players if an empty JSON file is posted. 
If No payload is sent with players and cards, the api will generate a default player and randomly generate 5 cards and return the players if GET or Winners if POST

The Models were generated using the Entity Framework Core database first approach. DDLs for DB is missing but can be provided. 

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


