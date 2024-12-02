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

/*
 * This sketch demonstrate the central API(). A additional bluefruit
 * that has bleuart as peripheral is required for the demo.
 */

#include <bluefruit.h>
// Struct containing peripheral info
typedef struct
{
  char name[16 + 1];

  uint16_t conn_handle;

  // Each prph need its own bleuart client service
  BLEClientUart bleuart;
  BLEClientBas  clientBas;
  // BLECluentDis  clientDis;
} prph_info_t;

prph_info_t prphs[BLE_MAX_CONNECTION];

// BLEClientBas  clientBas;  // battery client
// BLEClientDis clientDis;  // device information client
// BLEClientUart clientUart; // bleuart client

float An[3], a[3];
uint8_t switchValue;
uint8_t initial_value = 0;

//시간 측정
unsigned long preTime;
//배터리 레벨
int battery_level = 0;
int battery_level1 = 0;
int battery_level2 = 0;
unsigned long time_before = 0;
//버튼 핀
int button_pin1 = 16;
int button_pin2 = 15;
uint16_t conn_handle1 = BLE_CONN_HANDLE_INVALID;
uint16_t conn_handle2 = BLE_CONN_HANDLE_INVALID;
uint16_t conn_handle_before = BLE_CONN_HANDLE_INVALID;
uint8_t switchValue1 = 0;
uint8_t switchValue2 = 0;
uint8_t switch_bit = 0;
uint8_t switch_bit_before = 0;
// Software Timer for blinking the RED LED
SoftwareTimer blinkTimer;
uint8_t connection_num = 0;  // for blink pattern
int count = 0;
const int data_size = 20;


void setup()
{
  Serial.begin(115200);
  while ( !Serial ) delay(10);   // for nrf52840 with native usb
  blinkTimer.begin(100, blink_timer_callback);
  blinkTimer.start();

  Serial.println("Bluefruit52 Central BLEUART - Multi Receiver");
  Serial.println("-----------------------------------\n");
  
  // Initialize Bluefruit with maximum connections as Peripheral = 0, Central = 1
  // SRAM usage required by SoftDevice will increase dramatically with number of connections
  Bluefruit.begin(0, 2);
  
  Bluefruit.setName("Central Multi Receiver");

  // Init peripheral pool
  for (uint8_t idx = 0; idx < BLE_MAX_CONNECTION; idx++) {
    // Invalid all connection handle
    prphs[idx].conn_handle = BLE_CONN_HANDLE_INVALID;

    // All of BLE Central Uart Serivce
    prphs[idx].bleuart.begin();
    prphs[idx].bleuart.setRxCallback(bleuart_rx_callback);
    // Configure Battery client
    prphs[idx].clientBas.begin();
  }


  // Configure DIS client
  // clientDis.begin();
  // Init BLE Central Uart Serivce
  // clientUart.begin();
  // clientUart.setRxCallback(bleuart_rx_callback);

  // Increase Blink rate to different from PrPh advertising mode
  Bluefruit.setConnLedInterval(250);

  // Callbacks for Central
  Bluefruit.Central.setConnectCallback(connect_callback);
  Bluefruit.Central.setDisconnectCallback(disconnect_callback);

  /* Start Central Scanning
   * - Enable auto scan if disconnected
   * - Interval = 100 ms, window = 80 ms
   * - Don't use active scan
   * - Start(timeout) with timeout = 0 will scan forever (until connected)
   */
  Bluefruit.Scanner.setRxCallback(scan_callback);
  Bluefruit.Scanner.restartOnDisconnect(true);
  Bluefruit.Scanner.setInterval(160, 80); // in unit of 0.625 ms
  Bluefruit.Scanner.filterUuid(BLEUART_UUID_SERVICE);
  Bluefruit.Scanner.useActiveScan(false);
  Bluefruit.Scanner.start(0);                   // // 0 = Don't stop scanning after n seconds

  //버튼 핀
  pinMode(button_pin1, INPUT_PULLDOWN);
  pinMode(button_pin2, INPUT_PULLDOWN);
}

/**
 * Callback invoked when scanner pick up an advertising data
 * @param report Structural advertising data
 */
void scan_callback(ble_gap_evt_adv_report_t* report)
{
    // Since we configure the scanner with filterUuid()
  // Scan callback only invoked for device with bleuart service advertised
  // Connect to the device with bleuart service in advertising packet
  Bluefruit.Central.connect(report);
}

/**
 * Callback invoked when an connection is established
 * @param conn_handle
 */
void connect_callback(uint16_t conn_handle){
  // Find an available ID to use
  connection_num++;
  int id = findConnHandle(BLE_CONN_HANDLE_INVALID);

  // Eeek: Exceeded the number of connections !!!
  if (id < 0) return;

  prph_info_t* peer = &prphs[id];
  peer->conn_handle = conn_handle;
  //이름 배열 초기화가 되어야 이름 자릿수 차이날 떄 오류안남.
  memset(peer->name, 0, sizeof(peer->name));
  Bluefruit.Connection(conn_handle)->getPeerName(peer->name, sizeof(peer->name) - 1);

  Serial.print("Connected to ");
  Serial.println(peer->name);
  // if(strcmp(peer->name,"Spoon") == 0){
  //   conn_handle1 = conn_handle;
  //   Serial.print("Name saved: "); Serial.print("Spoon "); Serial.print("conn_handle: "); Serial.println(conn_handle);
  // }else if(strcmp(peer->name,"Marker") == 0){
  //   conn_handle2 = conn_handle;
  //   Serial.print("Name saved: "); Serial.print("Marker "); Serial.print("conn_handle: "); Serial.println(conn_handle);
  // }else{
  //   Serial.println("No name saved");
  // }
  Serial.print("Discovering BLE UART service ... ");

  if (peer->bleuart.discover(conn_handle)) {
    Serial.println("Found it");
    Serial.println("Enabling TXD characteristic's CCCD notify bit");
    peer->bleuart.enableTXD();

    Serial.println("Continue scanning for more peripherals");
    if(connection_num<2)
      Bluefruit.Scanner.start(0);

    Serial.println("Enter some text in the Serial Monitor to send it to all connected peripherals:");
  } else {
    Serial.println("Found NO BLEUART");

    // disconnect since we couldn't find bleuart service
    Bluefruit.disconnect(conn_handle);
  }


  Serial.print("Dicovering Battery ... ");
  if ( peer->clientBas.discover(conn_handle) )
  {
    Serial.print("Found: ");
    Serial.println(peer->name);
    Serial.print("Battery level: ");
    // peer->clientBas.enableNotify();
    // Serial.print(peer->clientBas.read());
    if (strcmp(peer->name,"Spoon") == 0){
      //이름과 일치할때에만 배터리 레벨 가져오기
      battery_level1 = peer-> clientBas.read();
      Serial.print(battery_level1);
    }
    else if (strcmp(peer->name,"Marker") == 0){
      battery_level2 = peer -> clientBas.read();
      Serial.print(battery_level2);
    }
    else{
    }
    Serial.println("%");
  }else
  {
    Serial.println("Found NO Bas");
  }


}

/**
 * Callback invoked when a connection is dropped
 * @param conn_handle
 * @param reason is a BLE_HCI_STATUS_CODE which can be found in ble_hci.h
 */
void disconnect_callback(uint16_t conn_handle, uint8_t reason)
{
  (void) conn_handle;
  (void) reason;
  connection_num--;

  // Mark the ID as invalid
  int id = findConnHandle(conn_handle);

  // Non-existant connection, something went wrong, DBG !!!
  if (id < 0) return;
  // Mark conn handle as invalid
  prphs[id].conn_handle = BLE_CONN_HANDLE_INVALID;
  // All of BLE Central Uart Serivce
  prphs[id].bleuart.begin();
  prphs[id].bleuart.setRxCallback(bleuart_rx_callback);
  // Configure Battery client
  prphs[id].clientBas.begin();

  Serial.print(prphs[id].name);
  Serial.println(" disconnected!");
  if(connection_num<2)
    Bluefruit.Scanner.start(0);
}

/**
 * Callback invoked when uart received data
 * @param uart_svc Reference object to the service where the data 
 * arrived. In this example it is clientUart
 */
void bleuart_rx_callback(BLEClientUart& uart_svc)
{
  //2개 받게 바꾼 부분///////////////////
  // uart_svc is prphs[conn_handle].bleuart
  uint16_t conn_handle = uart_svc.connHandle();
  int id = findConnHandle(conn_handle);
  prph_info_t* peer = &prphs[id];
  // Print sender's name
  // Serial.printf("[From %s]: ", peer->name);
  //2개 받게 바꾼 부분//////////////////
  
  // if(peer->name == Spoon)

  // int data_size = 20;
  // // Serial.print("[RX]: ");
  // byte data[data_size] = {0};

  //데이터 배열 초기화
  // memset(data, 0, sizeof(data));
  byte data[data_size] = {0};
  switchValue1 = digitalRead(button_pin1);
  switchValue2 = digitalRead(button_pin2);
  //스푼이 1 마커가 2
  switch_bit_before = switch_bit;
  switch_bit = (switchValue2 << 1) | switchValue1;
  



  //스위치 비트가 달라질 때에만 Spoon과 
  // if(switch_bit!=switch_bit_before){
  //   switch_bit
  // }

  count = 0;
  //switch_bit랑 핸들이 일치할 때에만 데이터를 받는다.
  //최근에 연결된 것이 내가 원하는 것과 일치하고, 스위치 맞을 때. 
  if((strcmp(peer->name,"Spoon") == 0 && switch_bit ==1) || (strcmp(peer->name,"Marker") == 0 && switch_bit ==2)){
  // if(switch_bit ==1 || switch_bit ==2){
    while ( uart_svc.available()){
      data[count++] = uart_svc.read();
      if (count >= data_size) {
        break;  // 더 이상 데이터를 읽지 않음
      }
    }
    // if(uart_svc.available())
    decompressData(data, An, &switchValue, a, switch_bit, peer);

    // 이거 때문에 문제 생기는 것 같은데??
    // //컨넥션 핸들 갱신해둔다. 
    // if(conn_handle_before != conn_handle){
    //   conn_handle_before = conn_handle;
    //   Fetch_batterylevel(millis(), switch_bit, peer);
    // }

  }else{
    while ( uart_svc.available()){
      data[count++] = uart_svc.read();
    }
    // Serial.print("Unmatch!! Switch_bit: "); Serial.print(switch_bit); Serial.print("/ Peripheral_name: "); Serial.println(peer->name);
    // Serial.println("Not Connected!!!");
      // //기존 연결 다 끊어버림..
      // if(conn_handle_before != conn_handle)
      //   Bluefruit.disconnect(conn_handle);
      // conn_handle_before = conn_handle;
      //아예 끊으면 문제가 생김. 걍 두개 전부 연결해두는게 목표기 때문.
  }

  // 테스트용 코드
  // while ( uart_svc.available() )
  // {
  //   // Serial.print( (char) uart_svc.read() );
  //   data[count++] = uart_svc.read();
  // }
  // decompressData(data, An, &switchValue, a, switch_bit, peer);

  
  // Serial.print("\t Count: ");
  // Serial.print(count);
  // Serial.print("\t Size: ");
  // Serial.print(sizeof(data));

}
void Fetch_batterylevel(int time_now, uint8_t switch_bit, prph_info_t* peer){
  //이건 10초마다 call해야함.
  if(time_now > time_before + 5000){
    char* name = peer->name;
    // for(int i = 0; i < sizeof(name); i++){
    //   Serial.println(name[i]);
    // }
    Serial.println("Battery Level Update");
    if (switch_bit == 1 && strcmp(name,"Spoon") == 0){
      //이름과 일치할때에만 배터리 레벨 가져오기
      // Serial.println(name);
      battery_level1 = peer-> clientBas.read();
    }
    else if (switch_bit == 2 && strcmp(name,"Marker") == 0){
      // Serial.println(name);
      battery_level2 = peer -> clientBas.read();
    }else{
    }
    time_before = time_now;
  }
}
void decompressData(byte* data, float An[3], uint8_t* switchValue, float a[3], uint8_t switch_bit, prph_info_t* peer) {
    uint8_t sys = 0;
    uint8_t gyro = 0;
    uint8_t accel = 0;
    uint8_t mag = 0;
    char buffer[128];

    if(data[0] == 0){
      Fetch_batterylevel(millis(), switch_bit, peer);
      initial_value = data[0];
      sys = data[1];
      gyro = data[2];
      accel = data[3];
      mag = data[4];
      // Serial.print("Mode: ");
      // if(switch_bit == 1)
      //   Serial.print("Spoon /");  // 출력
      // else if(switch_bit == 2)
      //   Serial.print("Marker /");
      // else 
      //   Serial.print("None /"); // 이건 애초에 걸러져서 호출될 일 없긴 하다.
      // Serial.print(" Battery level: ");
      // snprintf(buffer, sizeof(buffer), "Sys=%d Gyro=%d Accel=%d Mag=%d", sys, gyro, accel, mag);
      // if(switch_bit == 1){
      //   battery_level = battery_level1;
      // }else if(switch_bit == 2){
      //   battery_level = battery_level2;
      // }
      // Serial.print(battery_level);
      // Serial.print("% / CALIBRATING: ");
      snprintf(buffer, sizeof(buffer), "Mode: %s / Battery level: %d%% / CALIBRATING: Sys=%d Gyro=%d Accel=%d Mag=%d Conn_handle = %d",
                 (switch_bit == 1 ? "Spoon" : (switch_bit == 2 ? "Marker" : "None")),
                 (switch_bit == 1 ? battery_level1 : (switch_bit == 2 ? battery_level2 : 0)),
                 sys, gyro, accel, mag, peer->conn_handle);
    }
    else if(data[0] == 1){
      //첫번째 데이터를 넣음
      initial_value = data[0];
      //첫번째 데이터를 
      int index = 1;
      for (int i = 0; i < 3; i++) {
          int16_t decompressed = (data[index++] << 8) | data[index++];
          An[i] = decompressed / 10.0; // 소수점을 복원
          // Serial.print(" ");
          // Serial.print(decompressed);
      }
      *switchValue = !(uint8_t)data[index++]; // readValue 복원
      // Serial.print(" ");
      // Serial.print(switchValue);
      for (int i = 0; i < 3; i++) {
          int16_t decompressed = (data[index++] << 8) | data[index++];
          a[i] = decompressed / 100.0; // 소수점을 복원
          // Serial.print(" ");
          // Serial.print(decompressed);
      }
      // Serial.print(" Index: ");
      // Serial.print(index);
      // Serial.print(" Size: ");
      // Serial.print(sizeof(data));
      // Serial.print(" ");
      snprintf(buffer, sizeof(buffer), "%d,%.1f,%.1f,%.1f,%d,%.2f,%.2f,%.2f",
      switch_bit, An[0], An[1], An[2], *switchValue, a[0], a[1], a[2]);
      // /////////////////시간측정부///////////////////////////
      // Serial.print(" Delay: ");
      // unsigned long nowTime = millis();
      // Serial.println(nowTime - preTime);
      // preTime = nowTime;
      // /////////////////시간측정부///////////////////////////
    }
    Serial.println(buffer);
    // Serial.println(" ");
}


void loop()
{
  if ( Bluefruit.Central.connected() )
  {
    int id = findConnHandle(conn_handle_before);
    prph_info_t* peer = &prphs[id];
    // Not discovered yet
    if ( peer -> bleuart.discovered() )
    {
      // Discovered means in working state
      // Get Serial input and send to Peripheral
      if ( Serial.available() )
      {
        // delay(2); // delay a bit for all characters to arrive
        
        // char str[128+1] = { 0 };
        // Serial.readBytes(str, 128);
        
        // clientUart.print( str );
      }
    }
  }
}

int findConnHandle(uint16_t conn_handle) {
  for (int id = 0; id < BLE_MAX_CONNECTION; id++) {
    if (conn_handle == prphs[id].conn_handle) {
      return id;
    }
  }

  return -1;
}

void blink_timer_callback(TimerHandle_t xTimerID) {
  (void)xTimerID;

  // Period of sequence is 10 times (1 second).
  // RED LED will toggle first 2*n times (on/off) and remain off for the rest of period
  // Where n = number of connection
  static uint8_t count = 0;

  if (count < 2 * connection_num) digitalToggle(LED_RED);
  if (count % 2 && digitalRead(LED_RED)) digitalWrite(LED_RED, LOW);  // issue #98

  count++;
  if (count >= 10) count = 0;
}
