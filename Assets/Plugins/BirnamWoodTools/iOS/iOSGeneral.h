//
//  iOSGeneral.h
//  Unity-iPhone
//
//  Created by Mike Hopke on 2/10/14.
//
//
#import "BWG_Definitions.h"
#import <Foundation/Foundation.h>

@interface iOSGeneral : NSObject<UIAlertViewDelegate>

- (void) popAlertWith:(string) header andText:(string) text;

+ (iOSGeneral*) sharedManager;

@end
