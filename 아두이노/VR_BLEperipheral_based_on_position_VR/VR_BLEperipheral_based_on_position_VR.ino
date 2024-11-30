/*********************************************************************
 This is an example for our nRF52 based Bluefruit LE modules

 Pick one up today in the adafruit shop!

 Adafruit invests time and resources providing this open source code,
 please support Adafruit and open-source hardware by purchasing
 products from Adafruit!

 MIT license, check LICENSE for more information
 All text above, and the splash screen below must be included in
 any redistribution
*********************************************************************/

#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
//bleuart
#include <bluefruit.h>
#include <Adafruit_LittleFS.h>
#include <InternalFileSystem.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <iconv.h>
#include <Arduino.h>
// BLE Service
BLEDfu  bledfu;  // OTA DFU service
BLEDis  bledis;  // device information
BLEUart bleuart; // uart over ble
BLEBas  blebas;  // battery

//position_VR의 변수들//////////////////////////////////////////////////////////////////////////////////////////

double xPos = 0, yPos = 0, headingVel = 0;
double S_gps_x, S_gps_y, S_gps_z = 0.0;// initial location
double Vx, Vy, Vz = 0.0; // initial velocity
double Vx_before, Vy_before, Vz_before;
double high_Vx, high_Vy, high_Vz;
double high_S_gps_x, high_S_gps_y, high_S_gps_z;
double filter_Vx, filter_Vy, filter_Vz;
double filter_S_gps_x, filter_S_gps_y, filter_S_gps_z;

double S_gps_x_before, S_gps_y_before, S_gps_z_before;
float a[3], high_a[3], a_before[3], w[3], h[3], S[3], An[3];
double pi, theta, psi = 0.0;
double high_pi, high_theta, high_psi = 0.0;
double pi_before, theta_before, psi_before = 0.0;
/////////////////////////////////////////////////////////////////////////////////////
uint16_t BNO055_SAMPLERATE_DELAY_MS = 10; //how often to read data from the board
uint16_t PRINT_DELAY_UNIT_MS = 5; // time delaying UNIT
uint16_t PRINT_DELAY_MS = 20; // how often to print the data
// uint16_t MUST_DELAY_MS = 20;
unsigned long lastPrintTime = millis();
unsigned long tStart = millis();
/////////////////////////////////////////////////////////////////////////////////////
uint16_t printCount = 1; //counter to avoid printing every 10MS sample
float alpha1 = 0.1;
float alpha2 = 10;
//velocity = accel*dt (dt in seconds)
//position = 0.5*accel*dt^2
double ACCEL_VEL_TRANSITION =  (double)(BNO055_SAMPLERATE_DELAY_MS) / 1000.0;
double t = ACCEL_VEL_TRANSITION;
double ACCEL_POS_TRANSITION = 0.5 * ACCEL_VEL_TRANSITION * ACCEL_VEL_TRANSITION;
double DEG_2_RAD = 0.01745329251; //trig functions require radians, BNO055 outputs degrees

uint8_t switchValue = 0;
int data_size = 20;
uint8_t initial_value = 0;
// Check I2C device address and correct line below (by default address is 0x29 or 0x28)
//                                   id, address

//시간 측정
unsigned long preTime;
unsigned long time_before = 0;

//소프트웨어 타이머
SoftwareTimer IMUTimer;
// SoftwareTimer BLETimer;


#define VBAT_PIN A0  // 배터리 전압 측정 핀
//버튼 인풋
int button_pin = 12;
// #define button_pin SCK
int sensorValue = 0;
float bat_raw_level = 0;
float battery_level = 0;
float perc = 0;
float raw_voltage = 0;

char* device_name = "Spoon";

// char* device_name = "Marker";
//핸들

uint16_t conn_handle_before = -1;
/* This driver uses the Adafruit unified sensor library (Adafruit_Sensor),
   which provides a common 'type' for sensor data and some helper functions.

   To use this driver you will also need to download the Adafruit_Sensor
   library and include it in your libraries folder.

   You should also assign a unique ID to this sensor for use with
   the Adafruit Sensor API so that you can identify this particular
   sensor in any data logs, etc.  To assign a unique ID, simply
   provide an appropriate value in the constructor below (12345
   is used by default in this example).

   Connections
   ===========
   Connect SCL to analog 5
   Connect SDA to analog 4
   Connect VDD to 3.3-5V DC
   Connect GROUND to common ground

   History
   =======
   2015/MAR/03  - First release (KTOWN)
*/

/* Set the delay between fresh samples */
// uint16_t BNO055_SAMPLERATE_DELAY_MS = 100;

// Check I2C device address and correct line below (by default address is 0x29 or 0x28)
//                                   id, address
Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x28);

void setup(void)
{
  Wire.begin();
  Serial.begin(115200);
  byte error, address;
  int nDevices;
  int target_add = -1;
  nDevices = 0;
  for(address = 1; address < 127; address++ ) 
  {
    // The i2c_scanner uses the return value of
    // the Write.endTransmisstion to see if
    // a device did acknowledge to the address.
    Wire.beginTransmission(address);
    error = Wire.endTransmission();
    if (error == 0)
    {
      Serial.print("I2C device found at address 0x");
      if (address<16) 
        Serial.print("0");
      Serial.print(address,HEX);
      target_add = address;
      Serial.println("  !");
      nDevices++;
    }
    else if (error==4) 
    {
      Serial.print("Unknown error at address 0x");
      if (address<16) 
        Serial.print("0");
      Serial.println(address,HEX);
    }    
  }
  if (nDevices == 0)
    Serial.println("No I2C devices found\n");
  else
    Serial.println("done\n");
 
  

  //////////////////////////  //bleuart init////////////////////////////////////////
  Serial.println("HYU BPNE LAB - BLE BNO BOARD");
  Serial.println("---------------------------\n");
  // Setup the BLE LED to be enabled on CONNECT
  // Note: This is actually the default behavior, but provided
  // here in case you want to control this LED manually via PIN 19
  Bluefruit.autoConnLed(true);
  // Config the peripheral connection with maximum bandwidth 
  // more SRAM required by SoftDevice
  // Note: All config***() function must be called before begin()
  Bluefruit.configPrphBandwidth(BANDWIDTH_MAX);
  Bluefruit.begin();
  Bluefruit.setName(device_name);
  // Bluefruit.setName("Marker");
  //시간은 6*1.25~6*1.25로 고정

  Bluefruit.setTxPower(4);    // Check bluefruit.h for supported values
  //Bluefruit.setName(getMcuUniqueID()); // useful testing with multiple central connections
  Bluefruit.Periph.setConnInterval(6, 6);  
  Bluefruit.Periph.setConnectCallback(connect_callback);
  Bluefruit.Periph.setDisconnectCallback(disconnect_callback);

  // To be consistent OTA DFU should be added first if it exists
  bledfu.begin();

  // Configure and Start Device Information Service
  bledis.setManufacturer("Adafruit Industries");
  bledis.setModel("nRF52832");
  
  bledis.begin();

  // Configure and Start BLE Uart Service
  bleuart.begin();

  // Start BLE Battery Service
  // analogReference(AR_INTERNAL);
  // analogReadResolution(10); 
  battery_level = readBatteryVoltage();
  blebas.begin();
  blebas.write(battery_level);
  // Set up and start advertising
  startAdv();
  
  Serial.println("Please use Adafruit's Bluefruit LE app to connect in UART mode");
  Serial.println("Once connected, enter character(s) that you wish to send");
  ///////////////////////////bleuart end//////////////////////////////////////////

  //시리얼 통신 안하면 delay 계속 주는 코드...
  // while (!Serial) delay(10);  // wait for serial port to open!
  
  uint8_t setting_data[5] = {0,1,1,1,1};
  bleuart.write(setting_data, 5);
  if (!bno.begin())
  {
    Serial.println("No BNO055 detected");
    initial_value = 0;
    while (1){
      battery_level = readBatteryVoltage();
      setting_data[0] = initial_value;
      setting_data[1] = target_add;
      setting_data[2] = target_add;
      setting_data[3] = target_add;
      setting_data[4] = target_add;
      
      blebas.write(battery_level);
      bleuart.write(setting_data, 5);
      delay(100);
    }
  }
  setting_data[0] = initial_value;
  setting_data[1] = 2;
  setting_data[2] = 2;
  setting_data[3] = 2;
  setting_data[4] = 2;
  bleuart.write(setting_data, 5);
  delay(1000);
  uint8_t system, gyro, accel, mag = 0;


  while(system  != 3 || gyro != 3 || accel != 3){
    bno.getCalibration(&system, &gyro, &accel, &mag);
    battery_level = readBatteryVoltage();
    //PRINT
    Serial.print(device_name);
    Serial.print(" - Battery level: "); Serial.print(bat_raw_level);
    Serial.print("% / Sensor value: "); Serial.print(sensorValue);
    Serial.print("/ Voltage: "); Serial.println(raw_voltage);
    Serial.print("CALIBRATING: ");
    Serial.print("Sys="); Serial.print(system);
    Serial.print(" Gyro="); Serial.print(gyro);
    Serial.print(" Accel="); Serial.print(accel);
    Serial.print(" Mag="); Serial.println(mag);

    initial_value = 0;
    setting_data[0] = initial_value;
    setting_data[1] = system;
    setting_data[2] = gyro;
    setting_data[3] = accel;
    setting_data[4] = mag;
    
    blebas.write(battery_level);
    bleuart.write(setting_data, 5);
    delay(100);
  }
  //버튼
  // pinMode(button_pin, INPUT_PULLDOWN);
  pinMode(button_pin, INPUT);

  //IMUTImer는 세팅 다 끝난 뒤에 켜준다.
  IMUTimer.begin(BNO055_SAMPLERATE_DELAY_MS, IMUTimer_callback);
  IMUTimer.start();
  // BLETimer.begin(10, BLETimer_callback);
  // BLETimer.start();
}
// void Send_Batterylevel(int time_now){
//   if(time_now > time_before + 5000){
//     battery_level = readBatteryVoltage();
//     blebas.write(battery_level);
//     time_before = time_now;
//   }
// }
void IMUTimer_callback(TimerHandle_t xTimerID)
{
  // freeRTOS timer ID, ignored if not used
  (void) xTimerID;

  // Position_VR에서 가져온 부분 //////////////////////////////////////////////////////////////////////////////////////////////////
  sensors_event_t orientationData , linearAccelData, magnetometerData, angVelocityData, gravityData;
  // bno.getEvent(&orientationData, Adafruit_BNO055::VECTOR_EULER);
  imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
  // imu::Vector<3> acc = bno.getVector(Adafruit_BNO055::VECTOR_ACCELEROMETER); 
  //  bno.getEvent(&angVelData, Adafruit_BNO055::VECTOR_GYROSCOPE);
  bno.getEvent(&linearAccelData, Adafruit_BNO055::VECTOR_LINEARACCEL);
  // bno.getEvent(&magnetometerData, Adafruit_BNO055::VECTOR_MAGNETOMETER);
  // bno.getEvent(&gravityData, Adafruit_BNO055::VECTOR_GRAVITY);
  //Raw data
  a[0] = linearAccelData.acceleration.x;
  a[1] = linearAccelData.acceleration.y;
  a[2] = linearAccelData.acceleration.z;
  // // a[0] = acc.x();
  // // a[1] = acc.y();
  // // a[2] = acc.z();
  // w[0] = angVelocityData.gyro.x;
  // w[1] = angVelocityData.gyro.y;
  // w[2] = angVelocityData.gyro.z;
  An[0] = euler.x();
  An[1] = euler.y();
  An[2] = euler.z();
  // h[0] = magnetometerData.magnetic.x;
  // h[1] = magnetometerData.magnetic.y;
  // h[2] =  magnetometerData.magnetic.z;


  double acc_magnitude = sqrt(pow(a[0],2) + pow(a[1],2) + pow(a[2],2));
  if(acc_magnitude < 0.004){
      a[0] = 0;
      a[1] = 0;
      a[2] = 0;
  }
  // //angular-drift compensation
  if (abs(An[0]) < 0.1) {
      An[0] = 0;
  }
  if (abs(An[1]) < 0.1) {
      An[1] = 0;
  }
  if (abs(An[2]) < 0.1) {
      An[2] = 0;
  }

  // //버튼 입력 수신부
  switchValue = digitalRead(button_pin);
}

float readBatteryVoltage() {
  sensorValue = analogRead(VBAT_PIN);
  raw_voltage = sensorValue/1023.0 * 3.6;  // ADC 읽기
  float ref_high = 4.2*10/13.9; // 3.02  5V를 3.3V로 만들어주는 회로 적용
  float ref_low = 3.0*10/13.9; // 2.19 
  bat_raw_level = (raw_voltage-ref_low) / (ref_high-ref_low) * 100.0;
  int bat_perc = constrain(floor(bat_raw_level), 0, 100);
  return bat_perc;
}

void startAdv(void)
{
  // Advertising packet
  Bluefruit.Advertising.addFlags(BLE_GAP_ADV_FLAGS_LE_ONLY_GENERAL_DISC_MODE);
  Bluefruit.Advertising.addTxPower();

  // Include bleuart 128-bit uuid
  Bluefruit.Advertising.addService(bleuart);
  // Bluefruit.Advertising.addService(blebas); // 배터리 서비스 추가

  // Secondary Scan Response packet (optional)
  // Since there is no room for 'Name' in Advertising packet
  Bluefruit.ScanResponse.addName();
    // 광고 데이터 설정
  // Bluefruit.Advertising.addName(); // 디바이스 이름 추가
  /* Start Advertising
   * - Enable auto advertising if disconnected
   * - Interval:  fast mode = 20 ms, slow mode = 152.5 ms
   * - Timeout for fast mode is 30 seconds
   * - Start(timeout) with timeout = 0 will advertise forever (until connected)
   * 
   * For recommended advertising interval
   * https://developer.apple.com/library/content/qa/qa1931/_index.html   
   */
  Bluefruit.Advertising.restartOnDisconnect(true);
  Bluefruit.Advertising.setInterval(32, 244);    // in unit of 0.625 ms
  Bluefruit.Advertising.setFastTimeout(30);      // number of seconds in fast mode
  Bluefruit.Advertising.start(0);                // 0 = Don't stop advertising after n seconds
}


// void BLETimer_callback(TimerHandle_t xTimerID)
// {
//   // freeRTOS timer ID, ignored if not used
//   (void) xTimerID;
//   char string_to_display[64];
//   snprintf(string_to_display, sizeof(string_to_display), "%.2f,%.2f,%.2f,%d,%.2f,%.2f,%.2f",
//             An[0], An[1], An[2], switchValue, a[0], a[1], a[2]);
//   //PRINT
//   Serial.print(string_to_display);
//   // bleuart.write(buffer, sizeof(buffer));
//   byte data[data_size];
//   // dataSize = sizeof(data) / sizeof(data[0])
//   initial_value = 1;
//   compressAndSendData(An, switchValue, a, data);
//   // printCount = 1;
//   Serial.print(" | Battery level: ");
//   Serial.print(battery_level);
//   /////////////////시간측정부///////////////////////////
//   Serial.print(" Delay: ");
//   unsigned long nowTime = millis();
//   Serial.println(nowTime - preTime);
//   preTime = nowTime;
//   /////////////////시간측정부///////////////////////////
// }


void loop(void)
{
  // //만약 이전 루프보다 10ms 이상 지났으면 print 및 송신
  if (millis() - lastPrintTime >= PRINT_DELAY_MS) {
      lastPrintTime = millis();
      char string_to_display[64];
      snprintf(string_to_display, sizeof(string_to_display), "%.2f,%.2f,%.2f,%d,%.2f,%.2f,%.2f",
              An[0], An[1], An[2], switchValue, a[0], a[1], a[2]);
      //PRINT
      Serial.print(string_to_display);
      // bleuart.write(buffer, sizeof(buffer));
      byte data[data_size];
      // dataSize = sizeof(data) / sizeof(data[0])
      initial_value = 1;
      compressAndSendData(An, switchValue, a, data);
      // printCount = 1;
      Serial.print(" | Battery level: ");
      Serial.print(battery_level);
      /////////////////시간측정부///////////////////////////
      Serial.print(" Delay: ");
      unsigned long nowTime = millis();
      Serial.println(nowTime - preTime);
      preTime = nowTime;
      /////////////////시간측정부/////////////////////////// 
      // delay(2);
  }
  // tStart = millis();
  //최소 딜레이 보장 10ms >> 한 루프가 최소 10ms
  // while ((millis() - tStart) < (MUST_DELAY_MS))
  // {
  //   //poll until the next sample is ready
  // }
}



//byte 20개로 압축하고 ble로 보내는 부분
void compressAndSendData(float An[3], uint8_t switchValue, float a[3], byte* data) {
  // 첫번째로는 inital_value 넣어줌
  data[0] = initial_value;
  // index = 1부터 넣어주어야한다. index = 0은 initial_value를 넣어주어야함.
  int index = 1;
  for (int i = 0; i < 3; i++) {
    int16_t compressed = (int16_t)(floor(An[i] * 10)); // 소수점을 줄여서 압축
    data[index++] = compressed >> 8;
    data[index++] = compressed & 0xFF;
    // Serial.print(" ");
    // Serial.print(compressed);
  }
  data[index++] = (byte)switchValue; // switchValue 추가
  // Serial.print(" ");
  // Serial.print(switchValue);
  for (int i = 0; i < 3; i++) {
    int16_t compressed = (int16_t)(floor(a[i] * 100)); // 소수점을 줄여서 압축
    data[index++] = compressed >> 8;
    data[index++] = compressed & 0xFF;
    // Serial.print(" ");
    // Serial.print(compressed);
  }
  // Serial.print(" ");
  bleuart.write(data, data_size); //데이터 사이즈는 20!! >> 총 13비트에다가 넣어줌.
  // Serial.print(" ");

}


// void printEvent(sensors_event_t* event) {
//   double x = -1000000, y = -1000000 , z = -1000000; //dumb values, easy to spot problem

//   delay(BNO055_SAMPLERATE_DELAY_MS);
//   if (event->type == SENSOR_TYPE_ACCELEROMETER) {
//     Serial.print("Accl:");
//     x = event->acceleration.x;
//     y = event->acceleration.y;
//     z = event->acceleration.z;
//   }
//   else if (event->type == SENSOR_TYPE_ORIENTATION) {
//     Serial.print("Orient:");
//     x = event->orientation.x;
//     y = event->orientation.y;
//     z = event->orientation.z;
//   }
//   else if (event->type == SENSOR_TYPE_MAGNETIC_FIELD) {
//     Serial.print("Mag:");
//     x = event->magnetic.x;
//     y = event->magnetic.y;
//     z = event->magnetic.z;
//   }
  
//   else if (event->type == SENSOR_TYPE_GYROSCOPE) {
//     Serial.print("Gyro:");
//     x = event->gyro.x;
//     y = event->gyro.y;
//     z = event->gyro.z;
//   }
//   else if (event->type == SENSOR_TYPE_ROTATION_VECTOR) {
//     Serial.print("Rot:");
//     x = event->gyro.x;
//     y = event->gyro.y;
//     z = event->gyro.z;
//   }
//   else if (event->type == SENSOR_TYPE_LINEAR_ACCELERATION) {
//     Serial.print("Linear:");
//     x = event->acceleration.x;
//     y = event->acceleration.y;
//     z = event->acceleration.z;
//   }
//   else if (event->type == SENSOR_TYPE_GRAVITY) {
//     Serial.print("Gravity:");
//     x = event->acceleration.x;
//     y = event->acceleration.y;
//     z = event->acceleration.z;
//   }
//   else {
//     Serial.print("Unk:");
//   }

//   Serial.print("\tx= ");
//   Serial.print(x);
//   Serial.print(" |\ty= ");
//   Serial.print(y);
//   Serial.print(" |\tz= ");
//   Serial.println(z);

// }


// callback invoked when central connects
void connect_callback(uint16_t conn_handle)
{
  conn_handle_before = conn_handle;
  // Get the reference to current connection
  BLEConnection* connection = Bluefruit.Connection(conn_handle);

  char central_name[32] = { 0 };
  connection->getPeerName(central_name, sizeof(central_name));

  Serial.print("Connected to ");
  Serial.println(central_name);
}

/**
 * Callback invoked when a connection is dropped
 * @param conn_handle connection where this event happens
 * @param reason is a BLE_HCI_STATUS_CODE which can be found in ble_hci.h
 */
void disconnect_callback(uint16_t conn_handle, uint8_t reason)
{
  (void) conn_handle;
  (void) reason;
  
  Serial.println();
  Serial.print("Disconnected, reason = 0x"); Serial.println(reason, HEX);
  Serial.println("Re - Advertising"); Serial.println();
  //연결끊고 다시연결(알고보니 다시 연결은 스스로하더라ㅎ)
  Bluefruit.disconnect(conn_handle_before);
  
  // startAdv();
}

// byte 배열 합하려고 만든 건데 안씀 
void arr_concat(uint8_t* arr1, uint8_t* arr2){
  uint8_t* arr3;
  for(int i = 0; i < sizeof(arr1); i++){
      arr3[i] = arr1[i];
  }
  for(int i = sizeof(arr1), j = 0; i < sizeof(arr1) + sizeof(arr2); i++, j++){
      arr3[i] = arr2[j];
  }
  *arr1 = *arr3;
}

//안씀

char * encoding(char *text_input, char *source, char *target)
{
    iconv_t it;
     
    int input_len = strlen(text_input) + 1;
    int output_len = input_len*2;

    size_t in_size = input_len;
    size_t out_size = output_len;

    char *output = (char *)malloc(output_len);

    char *output_buf = output;
    char *input_buf = text_input;

	it = iconv_open(target, source); 
    int ret = iconv(it, &input_buf, &in_size, &output_buf, &out_size);

    
    iconv_close(it);

    return output;
}


void string2ByteArray(char* input, byte* output)
{
    int loop;
    int i;

    loop = 0;
    i = 0;

    while (input[loop] != '\0') {
        output[i++] = input[loop++];
    }
}

