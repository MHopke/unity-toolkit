//
//  GameKitManager.m
//  Unity-iPhone
//
//  Created by Mike Hopke on 2/11/14.
//
//

#import "BWG_Definitions.h"
#import "GameKitManager.h"

UIViewController *UnityGetGLViewController();

#ifdef TURN_BASED_INCLUDED
const NSString* PARTICIPANTS = @"Participants";
const NSString* MATCH_DATA = @"Data";
const NSString* MATCH_ID = @"id";
const NSString* PLAYER_TURN = @"your_turn";
const NSString* STATUSES = @"status";
#endif

@implementation GameKitManager

+ (GameKitManager*)sharedManager
{
	static GameKitManager *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[GameKitManager alloc] init];
	
	return sharedManager;
}

#pragma mark Delegate Methods
- (void)gameCenterViewControllerDidFinish:(GKGameCenterViewController *)gameCenterViewController
{
    NSLog(@"view did finish");
}

#pragma mark Internal Methods
- (void) authenticateLocalPlayer
{
    GKLocalPlayer *localPlayer = [GKLocalPlayer localPlayer];
    localPlayer.authenticateHandler = ^(UIViewController *viewController, NSError *error)
    {
        /*if(error)
         NSLog(error.debugDescription);*/
        
        if (viewController != nil)
        {
            [UnityGetGLViewController() presentViewController: viewController animated: YES completion:nil];
            //[self showAuthenticationDialogWhenReasonable: viewController];
        }
        else if (localPlayer.isAuthenticated)
        {
            _achievementsDictionary = [[NSMutableDictionary alloc] init];
            
            [self authenticatedPlayer: localPlayer];
            [self loadLeaderboardInfo];
            [self loadAchievements];
            //[self loadMatches];
            
            //[GKTurnBasedEventHandler sharedTurnBasedEventHandler].delegate = self;
            
            //[self sendInitialize];
        }
        else
        {
            [self disableGameCenter];
            
            //[self sendInitialize];
        }
    };
}

-(void) authenticatedPlayer:(GKLocalPlayer*)localPlayer
{
    if(_localPlayerID == nil)
        _localPlayerID = localPlayer.playerID;
    else if (_localPlayerID != localPlayer.playerID)
    {
        //A new player has logged in do something
    }
    
    //This really shouldn't be done here
    //unregister listenters to prevent duplicates
    [[GKLocalPlayer localPlayer] unregisterListener:self];
    
    //register listener
    [[GKLocalPlayer localPlayer] registerListener:self];
    
    //Generate player status string
    NSMutableString* status = [[[NSMutableString alloc] initWithString:[GKLocalPlayer localPlayer].playerID]autorelease];
    
    [status appendString:@","];
    [status appendString:[GKLocalPlayer localPlayer].alias];
    
    UnitySendMessage("GameKitManager", "Authenticated", [status cStringUsingEncoding:NSASCIIStringEncoding]);
}

- (void) disableGameCenter
{
    UnitySendMessage("GameKitManager", "Authenticated", "false");
}

- (void) loadLeaderboardInfo
{
    [GKLeaderboard loadLeaderboardsWithCompletionHandler:^(NSArray *leaderboards, NSError *error) {
        _leaderboards = leaderboards;
    }];
}

- (void) loadPlayerData: (NSArray *) identifiers
{
    [GKPlayer loadPlayersForIdentifiers:identifiers withCompletionHandler:^(NSArray *players, NSError *error) {
        if (error != nil)
        {
            //NSLog(error.description);// Handle the error.
        }
        if (players != nil)
        {
            NSMutableDictionary* friends = [[[NSMutableDictionary alloc] init]autorelease];
            
            // Process the array of GKPlayer objects.
            for(GKPlayer* myElement in players)
            {
                [friends setValue:myElement.displayName forKey:myElement.playerID ];
            }
            
            if(friends.count > 0)
            {
                NSError* jsonError;
                //convert object to data
                NSData* jsonData = [NSJSONSerialization dataWithJSONObject:friends
                                                                   options:NSJSONWritingPrettyPrinted error:&jsonError];
                
                UnitySendMessage("GameKitManager", "RetrievedFriends", [[[[NSString alloc] initWithData:jsonData encoding:NSASCIIStringEncoding] autorelease] cStringUsingEncoding:NSASCIIStringEncoding]);
            }
        }
    }];
}

- (void) reportScore: (int64_t) score forLeaderboardID: (NSString*) identifier
{
    GKScore *scoreReporter = [[GKScore alloc] initWithLeaderboardIdentifier: identifier];
    scoreReporter.value = score;
    scoreReporter.context = 0;
    
    NSArray *scores = @[scoreReporter];
    [GKScore reportScores:scores withCompletionHandler:^(NSError *error) {
        //Do something interesting here.
    }];
}
- (void) retrieveFriends
{
    GKLocalPlayer *lp = [GKLocalPlayer localPlayer];
    if (lp.authenticated)
    {
        [lp loadFriendsWithCompletionHandler:^(NSArray *friendIDs, NSError *error)
         {
             if (friendIDs != nil)
             {
                 [self loadPlayerData: friendIDs];
             }
         }];
    }
}

- (void) loadAchievements
{
    [GKAchievement loadAchievementsWithCompletionHandler:^(NSArray *achievements, NSError *error)
     {
         if (error == nil)
         {
             for (GKAchievement* achievement in achievements)
                 [_achievementsDictionary setObject: achievement forKey: achievement.identifier];
             
             if(_achievementsDictionary != nil)
             {
                 NSError* jsonError;
                 
                 NSData* jsonData = [NSJSONSerialization dataWithJSONObject:_achievementsDictionary
                                                                    options:NSJSONWritingPrettyPrinted error:&jsonError];
                 
                 UnitySendMessage("GameKitManager", "AchievementsLoaded", [[[[NSString alloc] initWithData:jsonData encoding:NSASCIIStringEncoding] autorelease] cStringUsingEncoding:NSASCIIStringEncoding]);
                 //UnitySendMessage("BWGPluginManager", "AchievementsLoaded", (char*)[jsonData bytes]);
             }
         }
     }];
}

- (GKAchievement*) getAchievementForIdentifier: (NSString*) identifier
{
    GKAchievement *achievement = [_achievementsDictionary objectForKey:identifier];
    if (achievement == nil)
    {
        achievement = [[GKAchievement alloc] initWithIdentifier:identifier];
        [_achievementsDictionary setObject:achievement forKey:achievement.identifier];
    }
    return achievement;
}

- (void) reportAchievementIdentifier: (string) identifier percentComplete: (float) percent
{
    GKAchievement *achievement = [self getAchievementForIdentifier:[[[NSString alloc] initWithBytes:identifier length:strlen(identifier) encoding:NSASCIIStringEncoding]autorelease]];
    if (achievement)
    {
        achievement.percentComplete = percent;
        [achievement reportAchievementWithCompletionHandler:^(NSError *error)
         {
             if (error != nil)
             {
                 // Log the error.
             }
         }];
    }
}

#pragma mark Extern C Methods
extern "C"
{
    void GKAuthenticatePlayer(){ [[GameKitManager sharedManager] authenticateLocalPlayer]; }
    
    void GKRetrieveFriends(){ [[GameKitManager sharedManager] retrieveFriends];}
    
    void GKPostLeaderboardScore (int score, string leaderboardIdentifier)
    {
        [[GameKitManager sharedManager] reportScore:score forLeaderboardID:[[[NSString alloc] initWithUTF8String:leaderboardIdentifier]autorelease]];
    }
    
    void GKViewLeaderboard(string leaderboardIdentifier){}
    
    void GKReportAchievement(string identifier, float progess) { [[GameKitManager sharedManager] reportAchievementIdentifier:identifier percentComplete:progess];}

}

#ifdef TURN_BASED_INCLUDED
#pragma mark Delegate Methods
//Turn Based Listener
- (void)player:(GKPlayer *)player didRequestMatchWithPlayers:(NSArray *)playerIDsToInvite
{
    NSLog(@"request match");
}

- (void)player:(GKPlayer *)player receivedTurnEventForMatch:(GKTurnBasedMatch *)match didBecomeActive:(BOOL)didBecomeActive
{
    NSLog(@"did receive update");
    
    //User selected the notification from the banner
    if(didBecomeActive)
    {
        if(match.currentParticipant.status == GKTurnBasedParticipantStatusInvited)
            NSLog(@"invited to game");
        
        NSLog(@"game became active");
        
        _activeMatch = match;
        [self loadMatchData];
        
        return;
    }
    
    //the current match has been updated
    if([_activeMatch isEqual:match])
    {
        if(match.status == GKTurnBasedMatchStatusEnded)
            NSLog(@"game ended");
        [self loadMatchData];
    }
    else
    {
        if(match.status == GKTurnBasedMatchStatusEnded)
            NSLog(@"game ended");
        
        if ([match.currentParticipant.playerID isEqualToString:[GKLocalPlayer localPlayer].playerID])
        {
            //Ask if the player wants to go to the quest
            
            //Check if a player has quit
            for(GKTurnBasedParticipant* element in _activeMatch.participants)
            {
                if(element.matchOutcome == GKTurnBasedMatchOutcomeQuit)
                {
                    [self endGameFromQuit];
                    break;
                }
            }
        }
    }
}

- (void) declineInvite
{
    [_activeMatch declineInviteWithCompletionHandler:^(NSError *error) {
        //declined match
    }];
}

- (void) acceptInvite
{
    [_activeMatch acceptInviteWithCompletionHandler:^(GKTurnBasedMatch *match, NSError *error) {
        
        if(match)
        {
            _activeMatch = match;
            
            UnitySendMessage("GameKitManager", "MatchSuccessfullyCreated", [match.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
        }
    }];
}

- (void)player:(GKPlayer *)player matchEnded:(GKTurnBasedMatch *)match
{
    NSLog(@"Game has ended");
    UnitySendMessage("GameKitManager", "MatchEnded", [match.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
}

- (void)player:(GKPlayer *)player receivedExchangeCancellation:(GKTurnBasedExchange *)exchange forMatch:(GKTurnBasedMatch *)match
{
    
}

- (void)player:(GKPlayer *)player receivedExchangeReplies:(NSArray *)replies forCompletedExchange:(GKTurnBasedExchange *)exchange forMatch:(GKTurnBasedMatch *)match
{
    
}

- (void)player:(GKPlayer *)player receivedExchangeRequest:(GKTurnBasedExchange *)exchange forMatch:(GKTurnBasedMatch *)match
{
    
}

#pragma mark Internal Methods
-(void) createMatch:(string)challengedFriend
{
    GKMatchRequest *request = [[GKMatchRequest alloc] init];
    request.minPlayers = 2;
    request.maxPlayers = 2;
    
    if(challengedFriend != nil)
        request.playersToInvite = [[[NSArray alloc] initWithObjects:[[[NSString alloc] initWithUTF8String:challengedFriend] autorelease], nil] autorelease];
    
    [GKTurnBasedMatch findMatchForRequest: request withCompletionHandler:^(GKTurnBasedMatch *match, NSError *error)
     {
         if(match != nil)
         {
             _activeMatch = match;
             
             UnitySendMessage("GameKitManager", "MatchCreationSuccess", [match.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
         }
         else
             UnitySendMessage("GameKitManager", "MatchCreationFailure", "");
     }];
    
    /*GKTurnBasedMatchmakerViewController *mmvc = [[GKTurnBasedMatchmakerViewController alloc] initWithMatchRequest:request];
     mmvc.turnBasedMatchmakerDelegate = self;
     
     [self.rootViewController presentViewController:mmvc animated:YES completion:nil];*/
}

- (NSString*)decodeData:(NSData*)matchData
{
    if([matchData length] > 0)
    {
        //Check for junk data
        string data = (string)[matchData bytes];
        //printf(data);
        if(data[strlen(data) - 1] == '}')
        {
            return [[[NSString alloc] initWithUTF8String:data]autorelease];
        }
        else
            return @"";
    }
    else
    {
        //printf("Empty Data");
        return @"";
    }
    
    return @"";
}

- (void)loadMatches
{
    //NSLog([GKLocalPlayer localPlayer].playerID);
    [GKTurnBasedMatch loadMatchesWithCompletionHandler:^(NSArray *matches, NSError *error)
     {
         if (matches)
         {
             NSMutableString* matchIDs = [[[NSMutableString alloc] init]  autorelease];
             NSMutableString* matchData = [[[NSMutableString alloc] init]  autorelease];
             NSMutableString* statuses = [[[NSMutableString alloc]init] autorelease];
             //NSMutableString* participants = [[[NSMutableString alloc] init]  autorelease];
             NSMutableString* turns = [[[NSMutableString alloc] init]  autorelease];
             
             NSMutableArray* participants = [[[NSMutableArray alloc] init] autorelease];
             
             //NSLog([GKLocalPlayer localPlayer].playerID);
             for(GKTurnBasedMatch* match in matches)
             {
                 //NSLog(@"%ld",(long)match.status);
                 //NSLog(match.currentParticipant.playerID);
                 //self.activeMatch = match;
                 /*Byte* fake = (Byte*)malloc(1);
                  [self updateMatchData:fake :1];
                  [self quit];*/
                 /*Byte* fake = (Byte*)malloc(1);
                  [self updateMatchData:fake :1];
                  [self endGame];*/
                 if(match.status != GKTurnBasedMatchStatusUnknown)
                 {
                     
                     [matchIDs appendString:match.matchID];
                     [matchIDs appendString:@"|"];
                     
                     [matchData appendString:[self decodeData:match.matchData]];
                     [matchData appendString:@"|"];
                     
                     [statuses appendFormat:@"%i",match.status];
                     [statuses appendString:@"|"];
                     
                     //[participants appendString:[GKLocalPlayer localPlayer].alias];
                     
                     for (GKTurnBasedParticipant* participant in match.participants)
                     {
                         //NSLog(participant.playerID);
                         if(![participant.playerID isEqualToString:[GKLocalPlayer localPlayer].playerID])
                         {
                             [participants addObject:participant.playerID];
                         }
                     }
                     //[participants appendString:@"|"];
                     
                     if([match.currentParticipant.playerID isEqualToString:[GKLocalPlayer localPlayer].playerID])
                         [turns appendString:@"yes"];
                     else
                         [turns appendString:@"no"];
                     
                     [turns appendString:@"|"];
                 }
             }
             
             //Look up all the associated players and then send the data
             [GKPlayer loadPlayersForIdentifiers:participants withCompletionHandler:^(NSArray *players, NSError *error)
              {
                  NSMutableString* names = [[[NSMutableString alloc] init]  autorelease];
                  
                  for (GKPlayer* gkPlayer in players) {
                      [names appendString:[gkPlayer alias]];
                      [names appendString:@"|"];
                  }
                  
                  NSMutableDictionary* info = [NSMutableDictionary dictionaryWithObjectsAndKeys:
                                               matchIDs,
                                               MATCH_ID,
                                               matchData,
                                               MATCH_DATA,
                                               names,
                                               PARTICIPANTS,
                                               turns,
                                               PLAYER_TURN,
                                               statuses,
                                               STATUSES,
                                               nil];
                  
                  NSError* jsonError;
                  
                  NSData* jsonData = [NSJSONSerialization dataWithJSONObject:info
                                                                     options:NSJSONWritingPrettyPrinted error:&jsonError];
                  
                  UnitySendMessage("GameKitManager", "MatchesLoaded", [[[[NSString alloc] initWithData:jsonData encoding:NSASCIIStringEncoding] autorelease] cStringUsingEncoding:NSASCIIStringEncoding]);
              }];
         }
     }];
}

- (void) loadMatchwithID:(string) identifier
{
    NSString* matchID = [[[NSString alloc] initWithCString:identifier encoding:NSASCIIStringEncoding] autorelease];
    
    //Check if we're trying to load the activeMatch
    if([_activeMatch.matchID isEqualToString:matchID])
    {
        //[self loadMatchData];
        UnitySendMessage("GameKitManager","MatchLoaded","");
        return;
    }
    else
    {
        [GKTurnBasedMatch loadMatchWithID:matchID withCompletionHandler:^(GKTurnBasedMatch *match, NSError *error)
         {
             if(error == nil)
             {
                 _activeMatch = match;
                 _gameData = match.matchData;
                 
                 
                 NSMutableString* matchIDs = [[[NSMutableString alloc] init]  autorelease];
                 
                 [matchIDs appendString:match.matchID];
                 [matchIDs appendString:@"#"];
                 
                 [matchIDs appendString:[self decodeData:match.matchData]];
                 
                 [matchIDs appendString:@"|"];
                 
                 UnitySendMessage("GameKitManager","MatchLoaded",[matchIDs cStringUsingEncoding:NSASCIIStringEncoding]);
                 //[self loadMatchData];
             }
         }];
    }
}

- (void)updateMatchData:(Byte[])data withLength:(int)length
{
    _gameData = [[NSData alloc] initWithBytes:data length:length];
}

- (void)saveMatchData:(Byte[])data withLength:(int)length
{
    _gameData = [[NSData alloc] initWithBytes:data length:length];
    [_activeMatch saveCurrentTurnWithMatchData:_gameData completionHandler:^(NSError *error)
     {
         if(error == nil)
             UnitySendMessage("GameKitManager", "MatchDataSaved", "");
     }];
}


//Assumes that matches are only 2 player
- (NSArray*)encodePlayerOrder
{
    NSMutableArray* playerOrder = [[[NSMutableArray alloc]init] autorelease];
    
    GKTurnBasedParticipant* front = (GKTurnBasedParticipant*)_activeMatch.participants[0];
    
    if([[GKLocalPlayer localPlayer].playerID isEqualToString:front.playerID])
    {
        [playerOrder addObject:_activeMatch.participants[1]];
        [playerOrder addObject:_activeMatch.participants[0]];
        
        return playerOrder;
    }
    else
    {
        return _activeMatch.participants;
    }
}

- (void)updateMessage:(string)message
{
    _activeMatch.message = [[[NSString alloc] initWithCString:message encoding:NSASCIIStringEncoding] autorelease];
}

- (void) advanceTurn
{
    NSArray *sortedPlayerOrder = [self encodePlayerOrder];
    
    [_activeMatch endTurnWithNextParticipants:sortedPlayerOrder turnTimeout:GKTurnTimeoutDefault matchData:_gameData completionHandler:^(NSError *error)
     {
         if(error)
         {
             //NSLog(error.debugDescription);//error
             UnitySendMessage("GameKitManager", "TurnAdvancedError", "");
             _gameData = nil;
             [_gameData release];
         }
         else
         {
             UnitySendMessage("GameKitManager", "TurnAdvancedSucess", "");
             _gameData = nil;
             [_gameData release];
         }
     }];
}

- (void) endGame
{
    for(GKTurnBasedParticipant* element in _activeMatch.participants)
    {
        element.matchOutcome = GKTurnBasedMatchOutcomeTied;
    }
    [_activeMatch endMatchInTurnWithMatchData:_gameData completionHandler:^(NSError *error)
     {
         if(error == nil)
         {
             UnitySendMessage("GameKitManager", "MatchEnded", ""/*[identifier cStringUsingEncoding:NSASCIIStringEncoding]*/);
             
             _gameData = nil;
             [_gameData release];
         }
         else
             UnitySendMessage("GameKitManager", "MatchEndedError", ""/*[identifier cStringUsingEncoding:NSASCIIStringEncoding]*/);
         
     }];
}

- (void) removeGame:(string)identifier
{
    NSString* matchID = [[[NSString alloc] initWithCString:identifier encoding:NSASCIIStringEncoding] autorelease];
    
    //Check if we're trying to load the activeMatch
    if([_activeMatch.matchID isEqualToString:matchID])
    {
        [_activeMatch removeWithCompletionHandler:^(NSError *error) {
            //some code
            if(error== nil)
                UnitySendMessage("GameKitManager", "MatchRemoved", [_activeMatch.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
        }];
    }
    else
    {
        [GKTurnBasedMatch loadMatchWithID:matchID withCompletionHandler:^(GKTurnBasedMatch *match, NSError *error)
         {
             if(error == nil)
             {
                 [match removeWithCompletionHandler:^(NSError *error) {
                     //some code
                     //NSLog(@"removed");
                     if(error == nil)
                         UnitySendMessage("GameKitManager", "MatchRemoved", [_activeMatch.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
                 }];
                 
                 _activeMatch = nil;
             }
         }];
    }
}

- (void) endGameFromQuit
{
    
}

- (void)sendQuitGame
{
    UnitySendMessage("GameKitManager", "MatchQuit", [_activeMatch.matchID cStringUsingEncoding:NSASCIIStringEncoding]);
}

- (void) quit
{
    _activeMatch.currentParticipant.matchOutcome = GKTurnBasedMatchOutcomeQuit;
    
    //NSLog(@"quit");
    
    //Check if your the current player
    if([[GKLocalPlayer localPlayer].playerID isEqualToString:_activeMatch.currentParticipant.playerID])
    {
        NSArray *sortedPlayerOrder = [self encodePlayerOrder];
        _activeMatch.message = @"Quit";
        
        [_activeMatch participantQuitInTurnWithOutcome:GKTurnBasedMatchOutcomeQuit nextParticipants:sortedPlayerOrder turnTimeout:GKTurnTimeoutDefault matchData:_gameData completionHandler:^(NSError *error)
         {
             //if(!_didResignActive)
             [self sendQuitGame];
         }];
    }
    else
    {
        [_activeMatch participantQuitOutOfTurnWithOutcome:GKTurnBasedMatchOutcomeQuit withCompletionHandler:^(NSError *error)
         {
             //Some code
             [self sendQuitGame];
         }];
    }
}

- (void) quitMatch:(string)identifier
{
    NSString* matchID = [[[NSString alloc] initWithCString:identifier encoding:NSASCIIStringEncoding] autorelease];
    
    //Check if we're trying to load the activeMatch
    if([_activeMatch.matchID isEqualToString:matchID])
    {
        [self quit];
    }
    else
    {
        [GKTurnBasedMatch loadMatchWithID:matchID withCompletionHandler:^(GKTurnBasedMatch *match, NSError *error)
         {
             if(match != nil)
             {
                 _activeMatch = match;
                 [self quit];
             }
         }];
    }
}

- (void)sendInitialize
{
    /*if(!firstTime)
     {
     [self onForcedOrientation:landscapeRight];
     firstTime = true;
     }*/
    //Triggered this here because Unity sets an incorrect category somewhere during its background initialization
    //[[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryPlayback error:NULL];
    
    UnitySendMessage("GameKitManager", "GameCenterAuthenicationDone", "");
}

- (void)loadMatchData
{
    [_activeMatch loadMatchDataWithCompletionHandler:^(NSData *matchData, NSError *error)
     {
         if([matchData length] > 0)
         {
             //Check for junk data
             string cData = (string)[matchData bytes];
             if(cData[strlen(cData) - 1] == '}')
             {
                 UnitySendMessage("GameKitManager", "MatchDataLoaded", cData);
             }
         }
     }];
}

#pragma mark Extern C Methods
extern "C"
{
    void GKAcceptInvite(){}
    
    void GKDeclineInvite(){}
    
    void GKCreateMatch(string playerToInvite){ [[GameKitManager sharedManager] createMatch:playerToInvite]; }
    
    void GKLoadMatch(string matchID){ [[GameKitManager sharedManager] loadMatchwithID:matchID]; }
    
    void GKLoadMatches(){ [[GameKitManager sharedManager] loadMatches]; }
    
    void GKUpdateMatchMessage(string data){}
    
    void GKAdvanceTurn(){ [[GameKitManager sharedManager] advanceTurn]; }
    
    void GKEndGame(){ [[GameKitManager sharedManager] endGame];}
    
    void GKRemoveMatch(string identifier) { [[GameKitManager sharedManager] removeGame:identifier]; }
    
    void GKQuitMatch(string matchID){ [[GameKitManager sharedManager] quitMatch:matchID]; }
    
    void GKUpdateMatchData(Byte* data, int length){ [[GameKitManager sharedManager] updateMatchData:data withLength:length]; }
    
    void GKSaveMatchData(Byte* data, int length){ [[GameKitManager sharedManager] saveMatchData:data withLength:length]; }
    
    void GKLoadMatchData(){}
}
#endif

@end
