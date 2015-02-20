//
//  TTSManager.m
//  Unity-iPhone
//
//  Created by Mike Hopke on 3/11/14.
//
//

#import "TTSManager.h"

@implementation TTSManager

+ (TTSManager*)sharedManager
{
	static TTSManager *sharedManager = nil;
	
	if( !sharedManager )
    {
		sharedManager = [[TTSManager alloc] init];
        sharedManager._voice = [AVSpeechSynthesisVoice voiceWithLanguage:@"en-US"];
        sharedManager._speechSynthesizer = [[AVSpeechSynthesizer alloc]init];
        sharedManager._speechRate = 0.16f;
    }
	
	return sharedManager;
}

-(void) speak:(NSString *)text
{
    AVSpeechUtterance *utterance = [AVSpeechUtterance speechUtteranceWithString:text];
    utterance.rate = [TTSManager sharedManager]._speechRate;
    utterance.voice = [TTSManager sharedManager]._voice;
    //[[TTSManager sharedManager]._speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryWord];
    [[TTSManager sharedManager]._speechSynthesizer speakUtterance:utterance];
}

extern "C" void iTextToSpeech(const char* text)
{
    [[TTSManager sharedManager] speak:[[[NSString alloc] initWithCString:text encoding:NSASCIIStringEncoding] autorelease]];
}

@end
