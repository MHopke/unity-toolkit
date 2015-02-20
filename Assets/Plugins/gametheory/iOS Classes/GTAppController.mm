//
//  GTAppController.mm
//  Unity-iPhone
//
//  Created by Mike Hopke on 10/14/13.
//
//

#import "GTAppController.h"
//#import "AWSManager.h"

#define IMPL_APP_CONTROLLER_SUBCLASS(GTAppController)

@implementation GTAppController

+ (GTAppController*)instance
{
    static GTAppController* instance= nil;
    
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        instance = [GTAppController new];
    });
    
    return instance;
}

-(void)setup
{
    UnityRegisterAppDelegateListener(self);
}

- (void)applicationDidBecomeActive:(NSNotification*)notification
{
}

- (void)applicationWillTerminate:(NSNotification *)notification
{
}

- (void)applicationWillResignActive:(NSNotification*)notification
{
}

- (void)onOpenURL:(NSNotification *)notification
{
    NSURL* url = [notification.userInfo objectForKey:@"url"];
    UnitySendMessage("OAuthManager", "ReceivedAuthenticationCode", [url.description cStringUsingEncoding:NSASCIIStringEncoding]);
}

extern "C"
{
    void AFInit()
    {
        [[GTAppController instance] setup];
    }
}
@end
