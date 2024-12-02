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
} prph_info_t;
prph_info_t prphs[BLE_MAX_CONNECTION];

BLEClientBas  clientBas;  // battery client
BLEClientDis  clientDis;  // device information client
BLEClientUart clientUart; // bleuart client
float An[3], a[3];
uint8_t switchValue;
uint8_t initial_value = 0;

//시간 측정
unsigned long preTime;
//배터리 레벨
int battery_level = 0;
unsigned long time_before = 0;
//버튼 핀
int button_pin1 = 15;
int button_pin2 = 16;

void setup()
{
  Serial.begin(115200);
//  while ( !Serial ) delay(10);   // for nrf52840 with native usb

  Serial.println("Bluefruit52 Central BLEUART - Multi Receiver");
  Serial.println("-----------------------------------\n");
  
  // Initialize Bluefruit with maximum connections as Peripheral = 0, Central = 1
  // SRAM usage required by SoftDevice will increase dramatically with number of connections
  Bluefruit.begin(0, 2);
  
  Bluefruit.setName("Bluefruit52 Central");

  // // Init peripheral pool
  // for (uint8_t idx = 0; idx < BLE_MAX_CONNECTION; idx++) {
  //   // Invalid all connection handle
  //   prphs[idx].conn_handle = BLE_CONN_HANDLE_INVALID;

  //   // All of BLE Central Uart Serivce
  //   prphs[idx].bleuart.begin();
  //   prphs[idx].bleuart.setRxCallback(bleuart_rx_callback);
  // }
  // Configure Battery client
  clientBas.begin();  

  // Configure DIS client
  clientDis.begin();

  // Init BLE Central Uart Serivce
  clientUart.begin();
  clientUart.setRxCallback(bleuart_rx_callback);

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
  // Check if advertising contain BleUart service
  if ( Bluefruit.Scanner.checkReportForService(report, clientUart) )
  {
    Serial.print("BLE UART service detected. Connecting ... ");

    // Connect to device with bleuart service in advertising
    Bluefruit.Central.connect(report);
  }else
  {      
    // For Softdevice v6: after received a report, scanner will be paused
    // We need to call Scanner resume() to continue scanning
    Bluefruit.Scanner.resume();
  }
}

/**
 * Callback invoked when an connection is established
 * @param conn_handle
 */
void connect_callback(uint16_t conn_handle){
  Serial.println("Connected");

  Serial.print("Dicovering Device Information ... ");
  if ( clientDis.discover(conn_handle) )
  {
    Serial.println("Found it");
    char buffer[32+1];
    
    // read and print out Manufacturer
    memset(buffer, 0, sizeof(buffer));
    if ( clientDis.getManufacturer(buffer, sizeof(buffer)) )
    {
      Serial.print("Manufacturer: ");
      Serial.println(buffer);
    }

    // read and print out Model Number
    memset(buffer, 0, sizeof(buffer));
    if ( clientDis.getModel(buffer, sizeof(buffer)) )
    {
      Serial.print("Model: ");
      Serial.println(buffer);
    }

    memset(buffer, 0, sizeof(buffer));
    if (Bluefruit.Connection(conn_handle)->getPeerName(buffer, sizeof(buffer) - 1))
    {
      Serial.print("Name: ");
      Serial.println(buffer);
    }
  }else
  {
    Serial.println("Found NONE");
  }

  Serial.print("Dicovering Battery ... ");
  if ( clientBas.discover(conn_handle) )
  {
    Serial.println("Found it");
    Serial.print("Battery level: ");
    battery_level = clientBas.read();
    Serial.print(battery_level);
    Serial.println("%");
  }else
  {
    Serial.println("Found NONE");
  }

  Serial.print("Discovering BLE Uart Service ... ");
  if ( clientUart.discover(conn_handle) )
  {
    Serial.println("Found it");

    Serial.println("Enable TXD's notify");
    clientUart.enableTXD();

    Serial.println("Ready to receive from peripheral");
  }else
  {
    Serial.println("Found NONE");
    
    // disconnect since we couldn't find bleuart service
    Bluefruit.disconnect(conn_handle);
  } 
  Serial.println();
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
    // Configure Battery client
  // clientBas.begin();  
  // // Configure DIS client
  // clientDis.begin();
  // // Init BLE Central Uart Serivce
  // clientUart.begin();
  // clientUart.setRxCallback(bleuart_rx_callback);
  Serial.print("Disconnected, reason = 0x"); Serial.println(reason, HEX);
}

/**
 * Callback invoked when uart received data
 * @param uart_svc Reference object to the service where the data 
 * arrived. In this example it is clientUart
 */
void bleuart_rx_callback(BLEClientUart& uart_svc)
{

  int data_size = 20;
  // Serial.print("[RX]: ");

  //데이터 배열 초기화
  byte data[data_size] = {0};

  int count = 0;
  while ( uart_svc.available() )
  {
    // Serial.print( (char) uart_svc.read() );
    data[count++] = uart_svc.read();
  }
  decompressData(data, An, &switchValue, a);

  
  // Serial.print("\t Count: ");
  // Serial.print(count);
  // Serial.print("\t Size: ");
  // Serial.print(sizeof(data));b
  /////////////////시간측정부///////////////////////////
  // Serial.print(" Delay: ");
  // unsigned long nowTime = millis();
  // Serial.println(nowTime - preTime);
  // preTime = nowTime;
  /////////////////시간측정부///////////////////////////
}
void Fetch_batterylevel(int time_now){
  if(time_now > time_before + 5000){
    battery_level = clientBas.read();
    time_before = time_now;
  }
}
void decompressData(byte* data, float An[3], uint8_t* switchValue, float a[3]) {
    uint8_t sys = 0;
    uint8_t gyro = 0;
    uint8_t accel = 0;
    uint8_t mag = 0;
    char buffer[64];

    if(data[0] == 0){
      Fetch_batterylevel(millis());
      initial_value = data[0];
      sys = data[1];
      gyro = data[2];
      accel = data[3];
      mag = data[4];
      Serial.print("Battery level: ");
      snprintf(buffer, sizeof(buffer), "Sys=%d Gyro=%d Accel=%d Mag=%d", sys, gyro, accel, mag);
      Serial.print(battery_level);
      Serial.print("% / CALIBRATING: ");
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
      *switchValue = (uint8_t)data[index++]; // readValue 복원
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
      snprintf(buffer, sizeof(buffer), "%.1f,%.1f,%.1f,%d,%.2f,%.2f,%.2f",
      An[0], An[1], An[2], *switchValue, a[0], a[1], a[2]);
      
    }
    Serial.println(buffer);
    // Serial.println(" ");
}


void loop()
{
  if ( Bluefruit.Central.connected() )
  {
    // Not discovered yet
    if ( clientUart.discovered() )
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
