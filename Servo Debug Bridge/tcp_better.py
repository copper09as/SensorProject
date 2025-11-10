import socket
import threading
import json
import time
import math
from datetime import datetime
from typing import Optional, Tuple


class THLClient:
    def __init__(self, host: str = '60.215.128.110', port: int = 43195):
        # 网络连接参数
        self.host = host
        self.port = port
        self.socket = None
        self.connected = False
        self.read_buffer = bytearray()  # 接收缓冲区

        # 环境分析参数（科学阈值，可根据应用场景调整）
        self.season = self._get_season()  # 动态季节判断（影响阈值）
        self.temp_ranges = {  # 温度阈值（°C）：[舒适下限, 舒适上限, 危险下限, 危险上限]
            'spring': (18, 26, 10, 35),
            'summer': (22, 28, 18, 38),
            'autumn': (18, 26, 10, 35),
            'winter': (16, 24, 5, 30)
        }
        self.humidity_ranges = {  # 湿度阈值（%）：[舒适下限, 舒适上限]
            'spring': (40, 65),
            'summer': (45, 70),
            'autumn': (40, 65),
            'winter': (35, 60)
        }
        self.light_ranges = {  # 光照阈值（lux）：[白天下限, 白天上限, 夜间上限]
            'spring': (200, 800, 50),
            'summer': (300, 1000, 50),
            'autumn': (200, 800, 50),
            'winter': (150, 700, 50)
        }

    # ------------------------------
    # 网络连接与接收相关方法（保持不变）
    # ------------------------------
    def connect(self) -> bool:
        """连接到服务器并启动接收线程"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(99)
            print(f"正在连接到服务器 {self.host}:{self.port}...")
            self.socket.connect((self.host, self.port))
            self.connected = True
            print(f"已成功连接到服务器 {self.host}:{self.port}")

            receive_thread = threading.Thread(target=self._receive_loop)
            receive_thread.daemon = True
            receive_thread.start()
            return True
        except Exception as e:
            print(f"连接失败: {e}")
            return False

    def _receive_loop(self) -> None:
        """持续接收服务器数据并处理"""
        while self.connected:
            try:
                data = self.socket.recv(1024)
                if not data:
                    print("与服务器的连接已断开")
                    self.connected = False
                    break
                self.read_buffer.extend(data)
                self._process_buffer()
            except Exception as e:
                if self.connected:
                    print(f"接收数据错误: {e}")
                break

    def _process_buffer(self) -> None:
        """按协议解析缓冲区数据"""
        while len(self.read_buffer) >= 2:
            total_length = (self.read_buffer[1] << 8) | self.read_buffer[0]
            if len(self.read_buffer) < 2 + total_length:
                break

            packet_data = self.read_buffer[2: 2 + total_length]
            proto_name, name_bytes_used = self._decode_proto_name(packet_data)
            if not proto_name:
                print("警告：协议名称解析失败，跳过无效数据包")
                self.read_buffer = self.read_buffer[2 + total_length:]
                continue

            body_data = packet_data[name_bytes_used:].decode('utf-8')
            try:
                body_json = json.loads(body_data)
            except json.JSONDecodeError:
                print(f"警告：消息体JSON解析失败，原始数据: {body_data}")
                self.read_buffer = self.read_buffer[2 + total_length:]
                continue

            if proto_name == "MsgForward":
                self._handle_sensor_data(body_json)

            self.read_buffer = self.read_buffer[2 + total_length:]

    def _decode_proto_name(self, packet_data: bytearray) -> Tuple[Optional[str], int]:
        """解析协议名称"""
        if len(packet_data) < 2:
            return None, 0
        name_length = (packet_data[1] << 8) | packet_data[0]
        if name_length <= 0 or len(packet_data) < 2 + name_length:
            return None, 0
        try:
            proto_name = packet_data[2: 2 + name_length].decode('utf-8')
        except UnicodeDecodeError:
            return None, 0
        return proto_name, 2 + name_length

    # ------------------------------
    # 数据处理与分析（核心优化部分）
    # ------------------------------
    def _handle_sensor_data(self, body_json: dict) -> None:
        """处理服务器发送的传感器数据"""
        thl_data = body_json.get("thlData")  # 注意字段名小写（根据实际协议调整）
        if not thl_data:
            print("警告：MsgForward协议中未找到thlData字段")
            return

        # 数据质量校验（新增）
        if not self._validate_data_quality(thl_data):
            print("数据质量不通过，跳过分析")
            return

        print(f"\n=== 收到服务器传感器数据 ===")
        print(f"数据ID: {thl_data.get('THDataId')}")
        print(f"温度: {thl_data.get('Temperature')}°C")
        print(f"湿度: {thl_data.get('Humidity')}%")
        print(f"光照: {thl_data.get('Light')}lux")
        print(f"时间: {thl_data.get('DTime')}")

        self._analyze_and_send(thl_data)

    def _validate_data_quality(self, thl_data: dict) -> bool:
        """数据质量校验（剔除异常值）"""
        temp = thl_data.get('Temperature', 0.0)
        humidity = thl_data.get('Humidity', 0.0)
        light = thl_data.get('Light', 0.0)

        # 物理范围校验
        if not (-40 <= temp <= 85):  # 温度超出自然环境可能范围
            print(f"数据异常：温度 {temp}°C 超出合理范围")
            return False
        if not (0 <= humidity <= 100):  # 湿度必须在0-100%
            print(f"数据异常：湿度 {humidity}% 超出合理范围")
            return False
        if not (0 <= light <= 200000):  # 光照上限设为20万lux（中午阳光直射约10万）
            print(f"数据异常：光照 {light}lux 超出合理范围")
            return False
        return True

    def _analyze_and_send(self, thl_data: dict) -> None:
        """分析数据并发送结果"""
        temp = thl_data.get("Temperature", 0.0)
        humidity = thl_data.get("Humidity", 0.0)
        light = thl_data.get("Light", 0.0)
        dtime = thl_data.get("DTime", "")

        # 季节动态更新（每天更新一次）
        if self._is_new_day(dtime):
            self.season = self._get_season()
            print(f"季节更新为: {self.season}")

        analy_result = self._analyze_data(temp, humidity, light, dtime)
        self._send_analy_result(analy_result)

    def _analyze_data(self, temp: float, humidity: float, light: float, dtime: str) -> str:
        """核心分析逻辑（科学化优化）"""
        # 1. 解析时间与昼夜
        hour, is_day = self._parse_time(dtime)

        # 2. 动态阈值获取（基于季节和昼夜）
        temp_low, temp_high, temp_danger_low, temp_danger_high = self.temp_ranges[self.season]
        humi_low, humi_high = self.humidity_ranges[self.season]
        light_day_low, light_day_high, light_night_high = self.light_ranges[self.season]

        # 3. 多维度分析（新增分级与科学指标）
        status = []
        severity = 0  # 异常严重程度（0=无异常，1=轻微，2=中度，3=严重）

        # 温度分析（新增舒适度和危险分级）
        temp_status, temp_severity = self._analyze_temp(temp, temp_low, temp_high, temp_danger_low, temp_danger_high)
        if temp_status:
            status.append(temp_status)
            severity = max(severity, temp_severity)

        # 湿度分析（结合温度计算体感湿度）
        humi_status, humi_severity = self._analyze_humidity(humidity, humi_low, humi_high, temp)
        if humi_status:
            status.append(humi_status)
            severity = max(severity, humi_severity)

        # 光照分析（新增昼夜动态阈值和植物光照需求）
        light_status, light_severity = self._analyze_light(light, is_day, light_day_low, light_day_high,
                                                           light_night_high)
        if light_status:
            status.append(light_status)
            severity = max(severity, light_severity)

        # 4. 综合结果（新增严重程度标识和建议）
        if not status:
            # 正常情况：添加舒适度指数（新增）
            comfort_idx = self._calculate_comfort_index(temp, humidity)
            return f"环境舒适（舒适度指数：{comfort_idx:.1f}）| 温湿度光照均在合理范围"
        else:
            # 异常情况：添加严重程度和建议
            severity_str = ["", "轻微", "中度", "严重"][severity]
            suggestion = self._generate_suggestion(status, severity)
            return f"【{severity_str}环境异常】：{'; '.join(status)} | 建议：{suggestion}"

    # ------------------------------
    # 科学分析子方法（新增/优化）
    # ------------------------------
    def _parse_time(self, dtime: str) -> Tuple[int, bool]:
        """解析时间，返回小时和昼夜标识"""
        try:
            # 处理带时区的时间格式（如"2025-11-08T12:41:38.2193432+08:00"）
            if '+' in dtime:
                dtime = dtime.split('+')[0]  # 移除时区偏移
            dt = datetime.fromisoformat(dtime)
            hour = dt.hour
            is_day = 6 <= hour < 18  # 6:00-18:00为白天
            return hour, is_day
        except (ValueError, TypeError):
            print("时间解析失败，默认按白天处理")
            return 12, True  # 默认白天12点

    def _get_season(self) -> str:
        """根据当前日期判断季节（动态调整阈值）"""
        month = datetime.now().month
        if 3 <= month <= 5:
            return 'spring'
        elif 6 <= month <= 8:
            return 'summer'
        elif 9 <= month <= 11:
            return 'autumn'
        else:
            return 'winter'

    def _is_new_day(self, dtime: str) -> bool:
        """判断是否跨天（用于更新季节和阈值）"""
        try:
            current_day = datetime.now().day
            if '+' in dtime:
                dtime = dtime.split('+')[0]
            data_day = datetime.fromisoformat(dtime).day
            return current_day != data_day
        except (ValueError, TypeError):
            return False

    def _analyze_temp(self, temp: float, low: float, high: float, danger_low: float, danger_high: float) -> Tuple[
        str, int]:
        """温度分析（新增舒适度和危险分级）"""
        if temp < danger_low:
            return f"温度极低（{temp}°C），存在冻伤风险", 3
        elif temp < low:
            return f"温度偏低（{temp}°C），建议保暖", 1
        elif temp > danger_high:
            return f"温度极高（{temp}°C），存在中暑风险", 3
        elif temp > high:
            return f"温度偏高（{temp}°C），建议降温", 2
        return "", 0

    def _analyze_humidity(self, humidity: float, low: float, high: float, temp: float) -> Tuple[str, int]:
        """湿度分析（结合温度计算体感湿度）"""
        # 计算体感湿度（Heat Index简化公式，仅用于舒适度参考）

        体感湿度 = humidity if temp < 25 else humidity + (temp - 25) * 0.5

        if 体感湿度 < low:
            return f"湿度干燥（{humidity}%，体感{体感湿度:.1f}%），建议加湿", 1


        elif 体感湿度 > high:
            return f"湿度潮湿（{humidity}%，体感{体感湿度:.1f}%），建议除湿", 2
        return "", 0


    def _analyze_light(self, light: float, is_day: bool, day_low: float, day_high: float, night_high: float) -> Tuple[
        str, int]:
        """光照分析（新增植物光照需求参考）"""
        if is_day:
            if light < day_low:
                return f"光照不足（{light}lux），不利于植物光合作用", 2
            elif light > day_high:
                return f"光照过强（{light}lux），可能导致叶面灼伤", 2
        else:
            if light > night_high:
                return f"夜间光照异常（{light}lux），影响植物休眠", 1
        return "", 0


    def _calculate_comfort_index(self, temp: float, humidity: float) -> float:
        """计算舒适度指数（简化版温湿度指数）"""
        # 公式来源：https://en.wikipedia.org/wiki/Heat_index
        return round(0.5 * (temp + 61.0 + ((temp - 68.0) * 1.2) + (humidity * 0.094)), 1)


    def _generate_suggestion(self, status: list, severity: int) -> str:
        """根据异常类型生成建议（新增）"""
        suggestions = []
        if any("温度" in s for s in status):
            suggestions.append("调整空调温度至22-26°C")
        if any("湿度" in s for s in status):
            suggestions.append("使用加湿器/除湿机维持湿度40-60%")
        if any("光照" in s for s in status):
            suggestions.append("白天开窗透光，夜间关闭灯光")

        # 根据严重程度调整建议语气
        if severity >= 3:
            return "立即处理！" + "；".join(suggestions)
        elif severity >= 2:
            return "请尽快处理：" + "；".join(suggestions)
        else:
            return "建议：" + "；".join(suggestions)


# ------------------------------
# 发送分析结果（保持不变）
# ------------------------------
    def _send_analy_result(self, result: str) -> bool:
        """发送分析结果到服务器"""
        if not self.connected:
            print("未连接到服务器，发送失败")
            return False

        try:
            send_data = {
                "AnalyResult": result,
                "protoName": "MsgThlAnaly"
            }

            name_bytes = self._encode_proto_name("MsgThlAnaly")
            body_bytes = json.dumps(send_data, ensure_ascii=False).encode('utf-8')

            total_length = len(name_bytes) + len(body_bytes)
            send_bytes = bytearray(2 + total_length)
            send_bytes[0] = total_length & 0xFF
            send_bytes[1] = (total_length >> 8) & 0xFF
            send_bytes[2:2 + len(name_bytes)] = name_bytes
            send_bytes[2 + len(name_bytes):] = body_bytes

            self.socket.sendall(send_bytes)
            print(f"\n=== 已发送分析结果 ===")
            print(f"结果: {result}")
            print(f"协议数据: {json.dumps(send_data, ensure_ascii=False)}")
            return True
        except Exception as e:
            print(f"发送分析结果失败: {e}")
            return False


    def _encode_proto_name(self, name: str) -> bytearray:
        """编码协议名称"""
        name_bytes = name.encode('utf-8')
        name_length = len(name_bytes)
        encoded = bytearray(2 + name_length)
        encoded[0] = name_length & 0xFF
        encoded[1] = (name_length >> 8) & 0xFF
        encoded[2:2 + name_length] = name_bytes
        return encoded


def disconnect(self) -> None:
    """断开连接"""
    self.connected = False
    if self.socket:
        try:
            self.socket.close()
        except Exception:
            pass
    print("已断开连接")


# ------------------------------
# 运行客户端
# ------------------------------
if __name__ == "__main__":
    client = THLClient(host='60.215.128.110', port=43195)
    if client.connect():
        try:
            while client.connected:
                time.sleep(1)
        except KeyboardInterrupt:
            client.disconnect()