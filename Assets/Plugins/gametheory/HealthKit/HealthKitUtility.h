//
//  HealthKitUtility.h
//  Unity-iPhone
//
//  Created by Mike Hopke on 11/11/14.
//
//

#import <Foundation/Foundation.h>
#import <HealthKit/HealthKit.h>

#ifdef __IPHONE_8_0
@interface HKHealthStore (AAPLExtensions)

// Fetches the single most recent quantity of the specified type.
- (void)aapl_mostRecentQuantitySampleOfType:(HKQuantityType *)quantityType predicate:(NSPredicate *)predicate completion:(void (^)(HKQuantity *mostRecentQuantity, NSError *error))completion;

@end
#endif

@interface HealthKitUtility : NSObject

+ (HealthKitUtility*) instance;

- (BOOL) healthkitExists;
- (void) initHealthStore;

#ifdef __IPHONE_8_0
@property (nonatomic) HKHealthStore *healthStore;
#endif

@end