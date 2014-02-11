//
//  GameKitManager.h
//  Unity-iPhone
//
//  Created by Mike Hopke on 2/11/14.
//
//
#import <GameKit/GameKit.h>
#import <Foundation/Foundation.h>

@interface GameKitManager : NSObject<GKGameCenterControllerDelegate, GKLocalPlayerListener>

+ (GameKitManager*) sharedManager;

@end

//Basic information
NSArray* _leaderboards;
NSString* _localPlayerID;
NSMutableDictionary* _achievementsDictionary;

//Turnbased matchmaking
#ifdef TURN_BASED_INCLUDED
GKTurnBasedMatch* _activeMatch;
NSData* _gameData;
#endif
