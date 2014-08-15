//
//  iOSGeneral.m
//  Unity-iPhone
//
//  Created by Mike Hopke on 2/10/14.
//
//
#import "iOSGeneral.h"
#import <Social/Social.h>
#import <AVFoundation/AVFoundation.h>

void UnityPause(bool pause);

@implementation iOSGeneral

+ (iOSGeneral*)sharedManager
{
	static iOSGeneral *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[iOSGeneral alloc] init];
	
	return sharedManager;
}

- (void) popAlertWith:(string) header andText:(string) text;
{
    UnityPause(true);
    UIAlertView *alert;
    
    alert = [[UIAlertView alloc] initWithTitle:[[[NSString alloc] initWithCString:header encoding:NSASCIIStringEncoding] autorelease] message:[[[NSString alloc] initWithCString:text encoding:NSASCIIStringEncoding] autorelease]
                                      delegate:self cancelButtonTitle:nil otherButtonTitles:@"OK", nil];
    [alert show];
    [alert release];
}

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 0)
    {
        UnitySendMessage("BWGPluginsManager","AlertViewDismissed","");
    }
    
    UnityPause(false);
}

extern "C"
{
    void iPopAlertWithHeaderAndText(string header, string text)
    {
        [[iOSGeneral sharedManager] popAlertWith:header andText:text];
    }
    
    void iPostToTwitter(string postMessage)
    {
        
        if([SLComposeViewController isAvailableForServiceType:SLServiceTypeTwitter])
        {
            SLComposeViewController *tweetSheet = [SLComposeViewController
                                                   composeViewControllerForServiceType:SLServiceTypeTwitter];
            [tweetSheet setInitialText:[[[NSString alloc] initWithUTF8String:postMessage] autorelease]];
            
            //[GetAppController().rootViewController presentViewController:tweetSheet animated:YES completion:nil];
        }
        else
        {
            //[GetAppController() popAlertWithHeader:"Sorry" andText:"You can't send a tweet right now, make sure your device has an internet connection and you have at least one Twitter account setup"];
        }
    }
    
    void iPostToFacebook(string status)
    {
        if([SLComposeViewController isAvailableForServiceType:SLServiceTypeFacebook])
        {
            SLComposeViewController *statusSheet = [SLComposeViewController
                                                    composeViewControllerForServiceType:SLServiceTypeFacebook];
            [statusSheet setInitialText:[[[NSString alloc] initWithUTF8String:status] autorelease]];
            
            //[GetAppController().rootViewController presentViewController:statusSheet animated:YES completion:nil];
        }
        else
        {
            //[GetAppController() popAlertWithHeader:"Sorry" andText:"You can't post a status right now, make sure your device has an internet connection and you have at least one Facebook account setup"];
        }
    }
}

@end
