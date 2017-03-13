//
//  GTAppController.h
//  Unity-iPhone
//
//  Created by Mike Hopke on 10/14/13.
//
//

#import "gametheory.h"
#import "AppDelegateListener.h"

@interface GTAppController :NSObject <AppDelegateListener>
{
}

-(void)setup;

+(GTAppController*) instance;
@end
