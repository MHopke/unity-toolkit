//
//  TTSManager.h
//  Unity-iPhone
//
//  Created by Mike Hopke on 3/11/14.
//
//

#import <AVFoundation/AVFoundation.h>
#import <Foundation/Foundation.h>

@interface TTSManager : NSObject

+ (TTSManager*) sharedManager;

- (void) speak:(NSString*)text;

@property (retain) AVSpeechSynthesizer* _speechSynthesizer;
@property(retain) AVSpeechSynthesisVoice* _voice;
@property float _speechRate;

@end
