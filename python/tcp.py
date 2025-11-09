import socket
import threading
import json
import time
from datetime import datetime
import random
import sys
import struct


class THLClient:
    def __init__(self, host='60.215.128.110', port=43195):
        self.host = host
        self.port = port
        self.socket = None
        self.connected = False
        self.th_data_id = 0
        self.read_buffer = bytearray()  # 接收缓冲区
        self.proto_name = "MsgForward"  # 协议名称（必须与服务器一致）

    def connect(self):
        """连接到服务器"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(10)
            print(f"正在连接到服务器 {self.host}:{self.port}...")
            self.socket.connect((self.host, self.port))
            self.connected = True
            print(f"已成功连接到服务器 {self.host}:{self.port}")

            # 启动接收消息的线程
            receive_thread = threading.Thread(target=self._receive_messages)
            receive_thread.daemon = True
            receive_thread.start()

            # 自动发送测试数据
            print("连接成功，发送测试数据...")
            self.send_thl_data(temperature=25.0, humidity=60.0, light=500.0)

            # 处理用户输入
            self._handle_user_input()

        except socket.timeout:
            print("连接超时")
        except ConnectionRefusedError:
            print("连接被拒绝")
        except Exception as e:
            print(f"连接失败: {e}")

    def _create_thl_data(self, temperature=None, humidity=None, light=None):
        """创建THL数据结构"""
        if temperature is None:
            temperature = round(random.uniform(20.0, 35.0), 2)
        if humidity is None:
            humidity = round(random.uniform(30.0, 80.0), 2)
        if light is None:
            light = round(random.uniform(0.0, 1000.0), 2)

        # 生成当前时间戳（ISO 8601格式）
        current_time = datetime.now().isoformat()

        # 构建数据包
        data_packet = {
            "ThlData": {
                "THDataId": self.th_data_id,
                "Temperature": temperature,
                "Humidity": humidity,
                "Light": light,
                "DTime": current_time
            },
            "protoName": "MsgForward"
        }

        self.th_data_id += 1
        return data_packet

    def _encode_name(self, name):
        """编码协议名称（完全匹配C#的EncodeName方法）"""
        # 1. UTF8编码名称
        name_bytes = name.encode('utf-8')

        # 2. 获取名称长度（Int16类型，即2字节）
        name_length = len(name_bytes)

        # 3. 创建2字节长度前缀 + 名称字节的数组
        encoded = bytearray(2 + name_length)

        # 4. 写入长度（little-endian格式）
        encoded[0] = name_length & 0xFF  # 低字节
        encoded[1] = (name_length >> 8) & 0xFF  # 高字节

        # 5. 写入名称字节
        encoded[2:2 + name_length] = name_bytes

        return encoded

    def _encode_body(self, data):
        """编码消息体（JSON格式）"""
        return json.dumps(data, ensure_ascii=False).encode('utf-8')

    def send_thl_data(self, temperature=None, humidity=None, light=None):
        """发送THL数据到服务器"""
        if not self.connected:
            print("未连接到服务器，无法发送数据")
            return False

        try:
            # 1. 创建数据结构
            data_packet = self._create_thl_data(temperature, humidity, light)

            # 2. 编码协议名称（关键修复：按C# EncodeName格式编码）
            name_bytes = self._encode_name(self.proto_name)  # 格式: [2字节长度][UTF8名称]

            # 3. 编码消息体
            body_bytes = self._encode_body(data_packet)

            # 4. 计算总长度 (nameBytes.Length + bodyBytes.Length)
            total_length = len(name_bytes) + len(body_bytes)

            # 5. 创建发送缓冲区: [2字节总长度][编码后的名称][消息体]
            send_bytes = bytearray(2 + total_length)

            # 设置总长度信息 (little-endian格式)
            send_bytes[0] = total_length & 0xFF  # 低字节
            send_bytes[1] = (total_length >> 8) & 0xFF  # 高字节

            # 复制编码后的协议名称（包含其自己的2字节长度前缀）
            send_bytes[2:2 + len(name_bytes)] = name_bytes

            # 复制消息体
            send_bytes[2 + len(name_bytes):] = body_bytes

            # 6. 发送数据
            self.socket.sendall(send_bytes)

            print(f"已发送数据: THDataId={data_packet['ThlData']['THDataId']}, "
                  f"温度={data_packet['ThlData']['Temperature']}°C")

            return True
        except Exception as e:
            print(f"发送数据失败: {e}")
            self.connected = False
            return False

    def _decode_name(self, bytes_data, offset):
        """解码协议名称（完全匹配C#的DecodeName方法）"""
        # 1. 检查是否有足够的字节（至少2字节长度）
        if offset + 2 > len(bytes_data):
            return "", 0

        # 2. 读取2字节长度（little-endian）
        name_length = (bytes_data[offset + 1] << 8) | bytes_data[offset]

        # 3. 检查长度是否有效
        if name_length <= 0 or offset + 2 + name_length > len(bytes_data):
            return "", 0

        # 4. 读取名称字节并解码（UTF8）
        name_bytes = bytes_data[offset + 2: offset + 2 + name_length]
        name = name_bytes.decode('utf-8')

        # 5. 返回名称和总占用字节数（2字节长度 + name_length字节名称）
        total_bytes = 2 + name_length
        return name, total_bytes

    def _process_received_message(self, name, data):
        """处理接收到的消息"""
        print(f"\n收到服务器响应 - 协议: {name}")
        try:
            json_data = json.loads(data)
            print(f"内容: {json.dumps(json_data, indent=2, ensure_ascii=False)}")
        except json.JSONDecodeError:
            print(f"原始内容: {data}")

    def _receive_messages(self):
        """接收服务器消息"""
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
                    print(f"接收消息时出错: {e}")
                break

    def _process_buffer(self):
        """处理接收缓冲区中的数据（完全匹配C# OnReceiveData逻辑）"""
        while len(self.read_buffer) >= 2:  # 至少需要2字节总长度
            # 1. 读取前2字节总长度（little-endian）
            total_length = (self.read_buffer[1] << 8) | self.read_buffer[0]

            # 2. 检查是否有足够的数据
            if len(self.read_buffer) < 2 + total_length:
                break  # 数据不足，等待更多数据

            # 3. 提取完整的消息数据（总长度部分）
            message_data = self.read_buffer[2: 2 + total_length]

            # 4. 解码协议名称（使用C# DecodeName逻辑）
            proto_name, name_bytes = self._decode_name(message_data, 0)
            if not proto_name:  # 解码失败
                print("警告：协议名称解码失败，跳过该消息")
                # 移除无效数据，继续处理后续内容
                self.read_buffer = self.read_buffer[2 + total_length:]
                continue

            # 5. 提取消息体（总长度 - 名称占用字节数）
            body_data = message_data[name_bytes:].decode('utf-8')

            # 6. 处理消息
            self._process_received_message(proto_name, body_data)

            # 7. 从缓冲区移除已处理的数据
            self.read_buffer = self.read_buffer[2 + total_length:]

    def _handle_user_input(self):
        """处理用户输入"""
        print("\n" + "=" * 50)
        print("THL数据客户端 (已完全匹配服务器协议)")
        print("命令说明:")
        print("  send [温度] [湿度] [光照] - 发送自定义数据")
        print("  auto - 启用自动发送 (每10秒一次)")
        print("  status - 显示连接状态")
        print("  quit - 退出程序")
        print("=" * 50)

        auto_sending = False
        while self.connected:
            try:
                user_input = input("\n请输入命令: ").strip()

                if not self.connected:
                    break

                if user_input.lower() == 'quit':
                    self.disconnect()
                    break
                elif user_input.lower() == 'status':
                    print(f"连接状态: {'已连接' if self.connected else '已断开'}")
                elif user_input.lower() == 'auto':
                    auto_sending = not auto_sending
                    if auto_sending:
                        print("开始自动发送数据 (每10秒一次)...")
                        threading.Thread(target=self._auto_send_data, args=(lambda: auto_sending,), daemon=True).start()
                    else:
                        print("停止自动发送数据")
                        auto_sending = False
                elif user_input.lower().startswith('send'):
                    parts = user_input.split()
                    temp = humi = light = None
                    if len(parts) >= 2:
                        try:
                            temp = float(parts[1])
                        except:
                            pass
                    if len(parts) >= 3:
                        try:
                            humi = float(parts[2])
                        except:
                            pass
                    if len(parts) >= 4:
                        try:
                            light = float(parts[3])
                        except:
                            pass
                    self.send_thl_data(temp, humi, light)
                elif user_input:
                    self.send_thl_data()

            except KeyboardInterrupt:
                self.disconnect()
                break

    def _auto_send_data(self, is_running):
        """自动发送数据"""
        while is_running() and self.connected:
            self.send_thl_data()
            for _ in range(10):
                if not is_running() or not self.connected:
                    break
                time.sleep(1)

    def disconnect(self):
        """断开连接"""
        self.connected = False
        if self.socket:
            try:
                self.socket.close()
            except:
                pass
        print("已断开与服务器的连接")


if __name__ == "__main__":
    host = '60.215.128.110'
    port = 43195

    if len(sys.argv) > 1:
        host = sys.argv[1]
    if len(sys.argv) > 2:
        try:
            port = int(sys.argv[2])
        except:
            print("端口号无效，使用默认端口43195")

    client = THLClient(host, port)
    try:
        client.connect()
    except KeyboardInterrupt:
        client.disconnect()