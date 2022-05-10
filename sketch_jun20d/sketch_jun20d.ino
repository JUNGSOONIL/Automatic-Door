#include <PN532.h>
#include <SPI.h>
#include <Servo.h>
#define PN532_CS 10
PN532 nfc(PN532_CS);
Servo moter;

int Trig_pin = 5; // 초음파센서
int Echo_pin = 8; // 초음파센서
unsigned long pulse_width = 0; // 초음파센서
unsigned long distance = 0; // 초음파센서
bool flag = false;
char flag1='0';
void setup(void)
{
    pinMode(Trig_pin, OUTPUT);
    pinMode(Echo_pin, INPUT);
    pinMode(3, OUTPUT); // 스피커 센서 입력핀
    moter.attach(6); // 서보 모터 입력핀
    moter.write(0);
    Serial.begin(9600);
    Serial.println("Start A5");
    nfc.begin();
    uint32_t versiondata = nfc.getFirmwareVersion();
    if (!versiondata)
    {
        Serial.print("NFC 보드가 발견되지 않았습니다!");
        while (1) ;
    }
    nfc.SAMConfig();   
}

void loop(void)
{
  if(Serial.available()>0){
  flag1=Serial.read();

  }
  if(flag1=='3'){
    uint32_t id;
    uint32_t id1 = 3509431052;
    uint32_t id2 = 2705101068;

    id = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A);
    trigPulse (Trig_pin);
    pulse_width = pulseIn(Echo_pin, HIGH);
    distance = pulse_width * 0.017;
    if (id == id1)
    {
        tone(3, 330);
        delay(100);
        noTone(3);
        Serial.println("");
        Serial.println("O");
        flag = true;
    }
   delay(100);
    if (distance <= 20 && flag == true && distance!=0)
        { // 문 앞으로 오면 if문
            moter.write(70); // 문을 열어준다
            delay(3000);
            moter.write(0); // 문을 닫아준다
            flag = false;
        }

    
    else if (id == id2)
    {
        Serial.println("");
        Serial.println("X");
        tone(3, 500);
        delay(100);
        noTone(3);
    }
    delay(250);
    
  }
  else{
    delay(250);
    }
delay(250);
}

void trigPulse (int pin) 
{ 
digitalWrite(pin, LOW); 
delayMicroseconds(2); 
digitalWrite(pin, HIGH); 
delayMicroseconds(10); 
digitalWrite(pin, LOW);
 }
