import socket
import threading
import json
import time
from typing import Optional, Tuple
import serial
from serial.tools import list_ports


class RotationClient:
    def __init__(self,
                 net_host: str = '60.215.128.110',
                 net_port: int = 43195,
                 serial_port: Optional[str] = None,
                 baud_rate: int = 9600):
        """
        初始化旋转角度客户端
        :param net_host: 服务器IP地址
        :param net_port: 服务器端口
        :param serial_port: 串口名称（如COM3或/dev/ttyUSB0），None则自动检测
        :param baud_rate: 串口波特率（需与STM32匹配）
        """
        # 网络配置
        self.net_host = net_host
        self.net_port = net_port
        self.socket = None
        self.connected = False
        self.read_buffer = bytearray()  # 网络接收缓冲区

        # 串口配置
        self.serial_port = serial_port
        self.baud_rate = baud_rate
        self.ser = None  # 串口对象

        # 角度数据缓存
        self.last_angle = 0  # 上次发送的角度值（避免重复发送）

    # ------------------------------
    # 串口连接管理
    # ------------------------------
    def _find_stm32_port(self) -> Optional[str]:
        """自动检测STM32连接的串口（通过VID/PID筛选，需根据实际硬件调整）"""
        # STM32常见VID/PID（举例，需根据实际使用的USB转串口芯片修改）
        # 例如：CH340芯片VID=0x1A86, PID=0x7523；CP2102芯片VID=0x10C4, PID=0xEA60
        target_vid_pid = [(0x1A86, 0x7523), (0x10C4, 0xEA60)]

        ports = list_ports.comports()
        for port in ports:
            if (port.vid, port.pid) in target_vid_pid:
                print(f"自动检测到STM32串口: {port.device}")
                return port.device
        return None

    def connect_serial(self) -> bool:
        """连接到STM32单片机的串口"""
        # 如果未指定串口，自动检测
        if not self.serial_port:
            self.serial_port = self._find_stm32_port()
            if not self.serial_port:
                print("未找到STM32串口，请检查连接或手动指定串口")
                return False

        try:
            self.ser = serial.Serial(
                port=self.serial_port,
                baudrate=self.baud_rate,
                timeout=0.1,
                bytesize=serial.EIGHTBITS,
                parity=serial.PARITY_NONE,
                stopbits=serial.STOPBITS_1
            )
            if self.ser.is_open:
                print(f"串口连接成功: {self.serial_port} (波特率: {self.baud_rate})")
                return True
            return False
        except Exception as e:
            print(f"串口连接失败: {e}")
            return False

    # ------------------------------
    # 网络连接管理
    # ------------------------------
    def connect_server(self) -> bool:
        """连接到服务器并启动接收线程"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(10)  # 连接超时设为10秒
            print(f"正在连接到服务器 {self.net_host}:{self.net_port}...")
            self.socket.connect((self.net_host, self.net_port))
            self.connected = True
            print(f"服务器连接成功: {self.net_host}:{self.net_port}")

            # 启动网络接收线程
            net_thread = threading.Thread(target=self._network_receive_loop, daemon=True)
            net_thread.start()
            return True
        except Exception as e:
            print(f"服务器连接失败: {e}")
            return False

    def _network_receive_loop(self) -> None:
        """持续接收服务器数据并解析"""
        while self.connected:
            try:
                data = self.socket.recv(1024)
                if not data:  # 服务器断开连接
                    print("服务器连接已断开")
                    self.connected = False
                    break
                self.read_buffer.extend(data)
                self._parse_network_data()  # 解析缓冲区中的协议数据
            except Exception as e:
                if self.connected:
                    print(f"网络接收错误: {e}")
                break

    def _parse_network_data(self) -> None:
        """解析网络缓冲区中的数据（按服务器协议格式）"""
        while len(self.read_buffer) >= 2:  # 至少需要2字节总长度
            # 1. 解析2字节总长度（little-endian：低字节在前）
            total_length = (self.read_buffer[1] << 8) | self.read_buffer[0]
            if len(self.read_buffer) < 2 + total_length:
                break  # 数据不足，等待后续包

            # 2. 提取完整数据包（跳过2字节长度头）
            packet_data = self.read_buffer[2: 2 + total_length]
            self.read_buffer = self.read_buffer[2 + total_length:]  # 移除已处理数据

            # 3. 解析协议名称（[2字节名称长度][UTF8名称]）
            proto_name, name_len = self._decode_proto_name(packet_data)
            if not proto_name:
                print("警告：协议名称解析失败，跳过无效包")
                continue

            # 4. 处理MsgRotation协议（只关注角度数据）
            if proto_name == "MsgRotation":
                self._handle_rotation_data(packet_data[name_len:])

    def _decode_proto_name(self, packet_data: bytearray) -> Tuple[Optional[str], int]:
        """解析协议名称（格式：2字节长度 + UTF8字符串）"""
        if len(packet_data) < 2:
            return None, 0
        name_len = (packet_data[1] << 8) | packet_data[0]  # 2字节长度（little-endian）
        if name_len <= 0 or len(packet_data) < 2 + name_len:
            return None, 0
        try:
            proto_name = packet_data[2:2 + name_len].decode('utf-8')
            return proto_name, 2 + name_len  # 返回名称和总占用字节数
        except UnicodeDecodeError:
            return None, 0

    # ------------------------------
    # 角度数据处理
    # ------------------------------
    def _handle_rotation_data(self, body_bytes: bytes) -> None:
        """处理服务器发送的角度数据（MsgRotation协议）"""
        try:
            body_json = json.loads(body_bytes.decode('utf-8'))
            angle = body_json.get('rotation', 0)  # 提取角度值
            angle = self._clamp_angle(angle)  # 限制角度范围为0-180
            print(f"收到角度数据: {angle}°")

            # 避免重复发送相同角度
            if angle != self.last_angle:
                self._send_to_stm32(angle)
                self.last_angle = angle  # 更新缓存
        except json.JSONDecodeError:
            print(f"角度数据JSON解析失败: {body_bytes}")
        except Exception as e:
            print(f"角度数据处理错误: {e}")

    def _clamp_angle(self, angle: float) -> int:
        """将角度限制在0-180°范围（匹配STM32舵机控制范围）"""
        angle = int(round(angle))  # 四舍五入为整数
        return max(0, min(180, angle))  # 限制上下限

    # ------------------------------
    # 串口数据发送（按STM32协议）
    # ------------------------------
    def _send_to_stm32(self, angle: int) -> bool:
        """
        将角度数据转换为STM32协议格式并发送
        协议格式：包头(0xFF) + 3字节角度数据 + 包尾(0xFE)
        """
        if not self.ser or not self.ser.is_open:
            print("串口未连接，发送失败")
            return False

        try:
            # 1. 将角度值(0-180)映射到3字节十六进制（0x000000-0xFFFFFF）
            # 映射公式：hex_value = angle * (0xFFFFFF / 180)
            hex_value = int(angle * (0xFFFFFF / 180))  # 0xFFFFFF = 16777215

            # 2. 拆分3字节数据（大端模式：高位在前）
            byte1 = (hex_value >> 16) & 0xFF  # 高8位
            byte2 = (hex_value >> 8) & 0xFF  # 中8位
            byte3 = hex_value & 0xFF  # 低8位

            # 3. 构建完整数据包（包头+数据+包尾）
            packet = bytes([0xFF, byte1, byte2, byte3, 0xFE])

            # 4. 发送数据
            self.ser.write(packet)
            print(f"发送到STM32: 角度={angle}°, 数据包={packet.hex().upper()}")
            return True
        except Exception as e:
            print(f"串口发送失败: {e}")
            return False

    # ------------------------------
    # 主连接管理
    # ------------------------------
    def start(self) -> None:
        """启动客户端（连接网络和串口）"""
        # 1. 先连接串口
        serial_ok = self.connect_serial()
        if not serial_ok:
            print("串口连接失败，程序退出")
            return

        # 2. 再连接服务器
        net_ok = self.connect_server()
        if not net_ok:
            self.ser.close()
            print("服务器连接失败，程序退出")
            return

        # 3. 保持运行
        try:
            while self.connected and self.ser.is_open:
                time.sleep(1)
        except KeyboardInterrupt:
            print("\n用户中断程序")
        finally:
            self.stop()

    def stop(self) -> None:
        """停止客户端（断开网络和串口）"""
        self.connected = False
        if self.socket:
            try:
                self.socket.close()
            except Exception:
                pass
        if self.ser and self.ser.is_open:
            self.ser.close()
            print("串口已关闭")
        print("程序已退出")


# ------------------------------
# 程序入口
# ------------------------------
if __name__ == "__main__":
    # 手动指定串口（如果自动检测失败），例如：
    # Windows: serial_port="COM3"
    # Linux: serial_port="/dev/ttyUSB0"
    # MacOS: serial_port="/dev/tty.usbserial-1420"
    client = RotationClient(
        net_host='60.215.128.110',
        net_port=43195,
        serial_port=None,  # None表示自动检测
        baud_rate=9600
    )
    client.start()