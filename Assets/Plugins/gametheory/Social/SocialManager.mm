//
//  SocialManager.cpp
//  Unity-iPhone
//
//  Created by Mike Hopke on 10/7/14.
//
//

#import "gametheory.h"
#import <Social/Social.h>
#import "UnityAppController.h"

extern "C"
{
    void iPostToTwitter(string postMessage)
    {
        
        if([SLComposeViewController isAvailableForServiceType:SLServiceTypeTwitter])
        {
            SLComposeViewController *tweetSheet = [SLComposeViewController
                                                   composeViewControllerForServiceType:SLServiceTypeTwitter];
            [tweetSheet setInitialText:[[NSString alloc] initWithUTF8String:postMessage]];
            
            [GetAppController().rootViewController presentViewController:tweetSheet animated:YES completion:nil];
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
            [statusSheet setInitialText:[[NSString alloc] initWithUTF8String:status]];
            [GetAppController().rootViewController presentViewController:statusSheet animated:YES completion:nil];
            /*if ([statusSheet respondsToSelector:@selector(popoverPresentationController)])
            {
                // iOS 8+
                UIPopoverPresentationController *presentationController = [statusSheet popoverPresentationController];
                
                presentationController.sourceView = sender; // if button or change to self.view.
            }*/
            //[GetAppController().rootViewController presentViewController:statusSheet animated:YES completion:nil];
        }
        else
        {
            //[GetAppController() popAlertWithHeader:"Sorry" andText:"You can't post a status right now, make sure your device has an internet connection and you have at least one Facebook account setup"];
        }
    }
}