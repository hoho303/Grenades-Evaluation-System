# 1 "c:\\Project\\Arduino\\NodeMCU\\NemLuuDan\\NemLuuDan.ino"
# 1 "c:\\Project\\Arduino\\NodeMCU\\NemLuuDan\\NemLuuDan.ino"
# 2 "c:\\Project\\Arduino\\NodeMCU\\NemLuuDan\\NemLuuDan.ino" 2
# 3 "c:\\Project\\Arduino\\NodeMCU\\NemLuuDan\\NemLuuDan.ino" 2
# 4 "c:\\Project\\Arduino\\NodeMCU\\NemLuuDan\\NemLuuDan.ino" 2
char* ssid = "MayChamDiemNemLuuDan";
char* pass = "20092003";
ESP8266WebServer server(80);
IPAddress local_ip(192,168,1,1);
IPAddress gateway(192,168,1,1);
IPAddress subnet(255,255,255,0);
static int lan = 0;
static int diem[5] = {10, -1, -1, -1, -1};
void setup()
{
    Serial.begin(115200);
    delay(10);
    caiDatServer();
}

void loop(){
    server.handleClient();
}

/*Các Hàm xử lý*/

void caiDatServer()
{
    WiFi.mode(WIFI_AP);
    WiFi.softAP(ssid,pass);
    WiFi.softAPConfig(local_ip, gateway, subnet);
    delay(100);
    server.on("/", onConnection);
    server.on("/getZero", onGetZero);
    server.on("/getData", onGetData);
    server.on("/getData1",onGetData1);
    server.on("/getData2",onGetData2);
    server.on("/getData3",onGetData3);
    server.on("/getData4",onGetData4);
    server.on("/getData5",onGetData5);
    server.on("/reset", onReset);
    server.onNotFound(onNotFound);
    server.begin();
    Serial.println("Khoi dong Server");
}
void onReset()
{
    lan = 0;
    for(int i=0;i<5;i++)
    {
        diem[i] = -1;
    }
}
void onGetData()
{
    server.send(200, "text/html", (String)diem[lan]);
    if(diem[lan] != -1)
    {
        lan++;
    }
}
void onGetZero()
{
    diem[lan] == 0;
    lan++;
    server.send(200, "text/html", "Đã cập nhật điểm");
}
void onConnection()
{
    server.send(200, "text/html", SendHTML(diem));
}
void onNotFound()
{
    server.send(404, "text/plain", "Not found");
}
void onGetData1()
{
    server.send(200,"text/plain",(String)diem[0]);
}
void onGetData2()
{
    server.send(200,"text/plain",(String)diem[1]);
}
void onGetData3()
{
    server.send(200,"text/plain",(String)diem[2]);
}
void onGetData4()
{
    server.send(200,"text/plain",(String)diem[3]);
}
void onGetData5()
{
    server.send(200,"text/plain",(String)diem[4]);
}
String SendHTML(int diem[])
{
    String webSource;
    webSource += "<!DOCTYPE html> <html>\n";
    webSource += "<head><meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <title>Ném Lựu Đạn</title></head>\n";
    webSource += "<body style=\"text-align: center\">\n <h1>Máy chấm điểm ném lựu đạn</h1>\n";
    webSource += "<table class=\"table\" style = \"padding-left : 520px\">\n";
    webSource += "<thead>\n<tr>\n<th>Lần Ném\n</th>\n<th style=\"padding-left: 100px\">\nĐiểm\n</th>\n</tr>\n";
    for(unsigned int i=0;i<5;i++)
    {
        if(diem[i] == -1)
        {
            webSource += "<tr>\n<th>" + (String)(i+1) + "\n</th>\n<th style=\"padding-left: 100px\">\nChưa có\n</th>\n</tr>\n";
        }
        else
        {
            webSource += "<tr>\n<th>" + (String)(i+1) + "\n</th>\n<th style=\"padding-left: 100px\">\n" + diem[i] +"\n</th>\n</tr>\n";
        }
    }
    webSource += "</thead>\n</table>\n</body>\n</html>";
    return webSource;
}
