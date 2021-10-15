//Libraries that we are going to use
#include <SparkTime.h>
#include <PietteTech_DHT.h>

//PietteTech_DHT library can handle different type of sensors. I will be using a DHT11 conected to digital Pin 4
#define DHTTYPE  DHT11
#define DHTPIN   D4
PietteTech_DHT DHT(DHTPIN, DHTTYPE);

//Global variables
double serverTemp;
double serverHumidity;
String allTempMeasurements = "";
String allHumidMeasurements = "";
int loopCount=0; //Just to know the loops of the main function

//Time related variables
UDP UDPClient;
SparkTime rtc;
int lastHour=0;
unsigned long currentTime;


void setup()
{
    Serial.begin(9600);
    //Cloud variables
    Particle.variable("serverTemp", &serverTemp, DOUBLE);
    Particle.variable("serverHumidity", &serverHumidity, DOUBLE);
    Particle.variable("loopCount", &loopCount, INT);
    Particle.variable("last_temps", &allTempMeasurements, STRING);
    Particle.variable("last_humids", &allHumidMeasurements, STRING);
    Particle.variable("last_timeUpdated", &lastHour, INT);
    
    //Starts the RTC with a NTP
    rtc.begin(&UDPClient, "north-america.pool.ntp.org");
    rtc.setTimeZone(0); // gmt offset
}

void loop()
{
    
    bool changedHour= DidHourChange();
    
    //Get the sensor values
    int result = DHT.acquireAndWait(2000);
    serverTemp = DHT.getCelsius();
    serverHumidity = DHT.getHumidity();
    
    //Log each hour
    if(changedHour)
    {
        Particle.publish("server_temperature_value",String::format("%.1f", serverTemp));
        Particle.publish("server_humidity_value",String::format("%.1f", serverHumidity));
        AddNewTempMeasurement(serverTemp);
        AddNewHumidMeasurement(serverHumidity);
    }
    
    delay(10000); //wait 10s
    
    if(loopCount<2147483647)//Int.Max= 2,147,483,647.
    {
        loopCount++;
    }
}

bool DidHourChange()
{
    if(loopCount==0) 
        return false; // this is for the first value only
    
    currentTime = rtc.now();
    int min = rtc.minute(currentTime);
    int hour = rtc.hour(currentTime);

    if (lastHour!=hour)
    {
        lastHour=hour;
        return true;
    }
    
    return false;
}

void AddNewTempMeasurement(double tempMeasurement)
{
    //string max length 864 character
    if(allTempMeasurements.length()>800)
    {
        //Makes room for more by removing the first 5 chars, corresponding to 1 measurement
        allTempMeasurements.remove(0,5);
    }
    allTempMeasurements.concat(String::format("%2.1f;", tempMeasurement));
}


void AddNewHumidMeasurement(double humidMeasurement)
{
    //string max length 864 character
    if(allHumidMeasurements.length()>800)
    {
        //Make room for more by removing the first 5 chars, corresponding to 1 measurement
        allHumidMeasurements.remove(0,5);
    }
    allHumidMeasurements.concat(String::format("%2.1f;", humidMeasurement));
}
