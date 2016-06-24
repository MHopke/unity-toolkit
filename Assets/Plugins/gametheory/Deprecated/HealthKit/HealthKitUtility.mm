//
//  HealthKitUtility.mm
//  Unity-iPhone
//
//  Created by Mike Hopke on 11/11/14.
//
//

#import "BWG_Definitions.h"
#import "HealthKitUtility.h"

#ifdef __IPHONE_8_0
@implementation HKHealthStore (AAPLExtensions)

- (void)aapl_mostRecentQuantitySampleOfType:(HKQuantityType *)quantityType predicate:(NSPredicate *)predicate completion:(void (^)(HKQuantity *, NSError *))completion {
    NSSortDescriptor *timeSortDescriptor = [[NSSortDescriptor alloc] initWithKey:HKSampleSortIdentifierEndDate ascending:NO];
    
    // Since we are interested in retrieving the user's latest sample, we sort the samples in descending order, and set the limit to 1. We are not filtering the data, and so the predicate is set to nil.
    HKSampleQuery *query = [[HKSampleQuery alloc] initWithSampleType:quantityType predicate:nil limit:1 sortDescriptors:@[timeSortDescriptor] resultsHandler:^(HKSampleQuery *query, NSArray *results, NSError *error) {
        if (!results) {
            if (completion) {
                completion(nil, error);
            }
            
            return;
        }
        
        if (completion) {
            // If quantity isn't in the database, return nil in the completion block.
            HKQuantitySample *quantitySample = results.firstObject;
            HKQuantity *quantity = quantitySample.quantity;
            
            completion(quantity, error);
        }
    }];
    
    [self executeQuery:query];
}
@end
#endif



@implementation HealthKitUtility

string NOT_AVAILABLE = "N/A";
string HEALTH_OBJECT = "HealthKitManager";

NSString* ENERGY_KEY = @"energy";
NSString* DISTANCE_KEY = @"distance";
NSString* START_DATE_KEY = @"start";
NSString* END_DATE_KEY = @"end";

+ (HealthKitUtility*)instance
{
    static HealthKitUtility* instance= nil;
    
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        instance = [HealthKitUtility new];
    });
    
    return instance;
}

- (BOOL)healthkitExists
{
    return (NSClassFromString(@"HKHealthStore") != nil && [HKHealthStore isHealthDataAvailable]);
}

- (void)initHealthStore
{
    if([self healthkitExists])
    {
        self.healthStore = [[HKHealthStore alloc] init];
        
        NSSet *writeDataTypes = [self dataTypesToWrite];
        NSSet *readDataTypes = [self dataTypesToRead];
        
        [self.healthStore requestAuthorizationToShareTypes:writeDataTypes readTypes:readDataTypes completion:^(BOOL success, NSError *error)
         {
             if (!success)
             {
                 NSLog(@"You didn't allow HealthKit to access these read/write data types. In your app, try to handle this error gracefully when a user decides not to provide access. The error was: %@. If you're using a simulator, try it on a device.", error);
                 
                 return;
             }
             
             UnitySendMessage(HEALTH_OBJECT, "Initialized", "");
         }];
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "Initialized", "");
}

// Returns the types of data that Fit wishes to write to HealthKit.
- (NSSet *)dataTypesToWrite
{
    HKQuantityType *runWalkType = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning];
    HKQuantityType *activeEnergyBurnType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierActiveEnergyBurned];
    HKQuantityType *heightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierHeight];
    HKQuantityType *weightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierBodyMass];
    
    return [NSSet setWithObjects:runWalkType, activeEnergyBurnType, heightType, weightType, [HKWorkoutType workoutType], nil];
}

// Returns the types of data that Fit wishes to read from HealthKit.
- (NSSet *)dataTypesToRead
{
    HKQuantityType *runWalkType = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning];
    HKQuantityType *stepType = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierStepCount];
    HKQuantityType *activeEnergyBurnType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierActiveEnergyBurned];
    HKQuantityType *heightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierHeight];
    HKQuantityType *weightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierBodyMass];
    HKCharacteristicType *birthdayType = [HKObjectType characteristicTypeForIdentifier:HKCharacteristicTypeIdentifierDateOfBirth];
    HKCharacteristicType *biologicalSexType = [HKObjectType characteristicTypeForIdentifier:HKCharacteristicTypeIdentifierBiologicalSex];
    
    return [NSSet setWithObjects:runWalkType, stepType, activeEnergyBurnType, heightType, weightType, birthdayType, biologicalSexType, nil];
}

- (void)getRunWalkType
{
    //HKQuery* query = [[HKQuery]]
    //[self.healthStore executeQuery:];
}

- (void)getAge
{
    if ([self healthkitExists])
    {
        // Set the user's age unit (years).
        NSError *error;
        NSDate *dateOfBirth = [self.healthStore dateOfBirthWithError:&error];
        
        if (!dateOfBirth)
        {
            NSLog(@"Either an error occured fetching the user's age information or none has been stored yet. In your app, try to handle this gracefully.");
            
            UnitySendMessage(HEALTH_OBJECT, "ReceivedAge", NOT_AVAILABLE);
        }
        else
        {
            // Compute the age of the user.
            NSDate *now = [NSDate date];
            
            NSDateComponents *ageComponents = [[NSCalendar currentCalendar] components:NSCalendarUnitYear fromDate:dateOfBirth toDate:now options:NSCalendarWrapComponents];
            
            NSUInteger usersAge = [ageComponents year];
            
            UnitySendMessage(HEALTH_OBJECT, "ReceivedAge", [[NSNumberFormatter localizedStringFromNumber:@(usersAge) numberStyle:NSNumberFormatterNoStyle]cStringUsingEncoding:NSASCIIStringEncoding]);
        }
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "ReceivedAge", NOT_AVAILABLE);
}

- (void)getDateOfBirth
{
    if([self healthkitExists])
    {
        NSError *error;
        NSDate *dateOfBirth = [self.healthStore dateOfBirthWithError:&error];
        
        if (!dateOfBirth)
        {
            UnitySendMessage(HEALTH_OBJECT, "ReceivedDOB", NOT_AVAILABLE);
        }
        else
        {
            NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
            
            UnitySendMessage(HEALTH_OBJECT, "ReceivedDOB", [[formatter stringFromDate:dateOfBirth] cStringUsingEncoding:NSASCIIStringEncoding]);
        }
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "ReceivedDOB", NOT_AVAILABLE);
}

- (void)getWeight
{
    if([self healthkitExists])
    {
        HKQuantityType *weightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierBodyMass];
        
        [self.healthStore aapl_mostRecentQuantitySampleOfType:weightType predicate:nil completion:^(HKQuantity *weight, NSError *error)
         {
             //NSLog(@"weight %@",weight);
             
             if (weight)
             {
                 UnitySendMessage(HEALTH_OBJECT, "ReceivedWeight", [[[NSString alloc] initWithFormat:@"%f",[weight doubleValueForUnit:[HKUnit gramUnitWithMetricPrefix:HKMetricPrefixKilo]]] cStringUsingEncoding:NSASCIIStringEncoding]);
             }
             else
                 UnitySendMessage(HEALTH_OBJECT, "ReceivedWeight",NOT_AVAILABLE);
         }];
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "ReceivedWeight",NOT_AVAILABLE);
}

- (void)getHeight
{
    if([self healthkitExists])
    {
        HKQuantityType *heightType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierHeight];
        
        [self.healthStore aapl_mostRecentQuantitySampleOfType:heightType predicate:nil completion:^(HKQuantity *height, NSError *error)
         {
             //NSLog(@"height %@",height);
             
             if (height)
             {
                 UnitySendMessage(HEALTH_OBJECT, "ReceivedHeight", [[[NSString alloc] initWithFormat:@"%f",[height doubleValueForUnit:[HKUnit unitFromString:@"cm"]]] cStringUsingEncoding:NSASCIIStringEncoding]);
             }
             else
                 UnitySendMessage(HEALTH_OBJECT, "ReceivedHeight",NOT_AVAILABLE);
         }];
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "ReceivedHeight",NOT_AVAILABLE);
}

- (void)getSex
{
    if([self healthkitExists])
    {
        NSError *error;
        HKBiologicalSexObject* biologicalSex = [self.healthStore biologicalSexWithError:&error];
        
        if(!biologicalSex)
        {
            UnitySendMessage(HEALTH_OBJECT, "ReceivedBiologicalSex", "NOT_AVAILABLE");
        }
        else
        {
            if([biologicalSex biologicalSex] == HKBiologicalSexFemale)
                UnitySendMessage(HEALTH_OBJECT, "ReceivedBiologicalSex", "FEMALE");
            else if([biologicalSex biologicalSex] == HKBiologicalSexMale)
                UnitySendMessage(HEALTH_OBJECT, "ReceivedBiologicalSex", "MALE");
            else
                UnitySendMessage(HEALTH_OBJECT, "ReceivedBiologicalSex", "NOT_SET");
        }
    }
    else
        UnitySendMessage(HEALTH_OBJECT, "ReceivedBiologicalSex", "NOT_AVAILABLE");
}

- (void)getEnergyBurned
{
    if([self healthkitExists])
    {
        HKQuantityType* caloriesType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierActiveEnergyBurned];
        
        [self.healthStore aapl_mostRecentQuantitySampleOfType:caloriesType predicate:nil completion:^(HKQuantity *activeCalories, NSError *error)
         {
             //something
             if(activeCalories)
             {
                 //            [activeCalories doubleValueForUnit:[HKUnit]]
                 UnitySendMessage(HEALTH_OBJECT, "ReceivedActiveCalories", "");
             }
         }];
    }
}

- (void)postWorkoutFrom:(string)JSON
{
    if([self healthkitExists])
    {
        NSData* data = [[[NSString alloc] initWithCString:JSON encoding:NSUTF8StringEncoding] dataUsingEncoding:NSUTF8StringEncoding];
        
        NSError* error = nil;
        NSDictionary* dictionary = [NSJSONSerialization JSONObjectWithData: data options: NSJSONReadingAllowFragments error: &error];
        
        if(error != nil)
        {
            //NSLog(@"%@",error);
            return;
        }
        
        HKQuantity* energyBurned = [HKQuantity quantityWithUnit:[HKUnit calorieUnit] doubleValue:[[dictionary valueForKey:ENERGY_KEY] doubleValue]];
        
        HKQuantity* distance = [HKQuantity quantityWithUnit:[HKUnit meterUnit] doubleValue:[[dictionary valueForKey:DISTANCE_KEY]doubleValue]];
        
        //generate dates
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc]init];
        dateFormatter.dateFormat = @"MM/dd/yyyy HH:mm:ss";
        
        NSDate* startDate = [dateFormatter dateFromString:[dictionary valueForKey:START_DATE_KEY]];
        NSDate* endDate = [dateFormatter dateFromString:[dictionary valueForKey:END_DATE_KEY]];
        
        HKWorkout* run = [HKWorkout workoutWithActivityType:HKWorkoutActivityTypeRunning startDate:startDate endDate:endDate duration:0.0 totalEnergyBurned:energyBurned totalDistance:distance metadata:nil];
        
        [_healthStore saveObject:run withCompletion:^(BOOL success, NSError *error) {
            if(!success)
            {
                //failed
                NSLog(@"failed: %@",error);
            }
            else
                //success
                NSLog(@"success");
        }];
    }
}

- (void) getDailySteps
{
    if([self healthkitExists])
    {
        HKQuantityType *stepsType = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierStepCount];
        
        [self.healthStore aapl_mostRecentQuantitySampleOfType:stepsType predicate:nil completion:^(HKQuantity *steps, NSError *error) {
        }];
    }
}

#pragma mark - Extern Methods

extern "C"
{
    void InitHealthKit() { [[HealthKitUtility instance] initHealthStore]; }
    
    void PostHKRun(string json){ [[HealthKitUtility instance] postWorkoutFrom:json]; }
    
    void GetHKAge(){ [[HealthKitUtility instance] getAge]; }
    
    void GetHKHeight(){ [[HealthKitUtility instance] getHeight]; }
    
    void GetHKWeight(){ [[HealthKitUtility instance] getWeight]; }
    
    void GetHKSex(){ [[HealthKitUtility instance] getSex]; }
    
    void GetHKDateOfBirth(){ [[HealthKitUtility instance] getDateOfBirth]; }
}

@end
