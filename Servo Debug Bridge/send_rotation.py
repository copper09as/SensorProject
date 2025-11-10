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
        初始化旋转角度客户端（适配Windows 10 + STM32F103C8T6）
        :param net_host: 服务器IP地址
        :param net_port: 服务器端口
        :param serial_port: 串口名称（如COM3），None则自动检测
        :param baud_rate: 串口波特率（需与STM32匹配，默认9600）
        """
        # 网络配置
        self.net_host = net_host
        self.net_port = net_port
        self.socket = None
        self.connected = False
        self.read_buffer = bytearray()  # 网络接收缓冲区

        # 串口配置（Windows下常见USB转串口芯片关键词）
        self.serial_port = serial_port
        self.baud_rate = baud_rate
        self.ser = None  # 串口对象
        self.target_serial_keywords = [
            "CH340",         # 匹配CH340芯片（STM32F103C8T6开发板常用）
            "CP210",         # 匹配CP2102芯片
            "USB-SERIAL",    # 匹配通用USB转串口设备
            "UART"           # 匹配UART转串口设备
        ]

        # 角度数据缓存
        self.last_angle = 0  # 上次发送的角度值（避免重复发送）

    # ------------------------------
    # 串口连接管理（核心修改：Windows下关键词匹配检测）
    # ------------------------------
    def _find_stm32_port(self) -> Optional[str]:
        """
        自动检测Windows系统中STM32F103C8T6对应的串口（通过描述关键词匹配）
        适配场景：STM32F103C8T6外接CH340/CP2102等USB转串口芯片
        """
        try:
            # 获取所有可用串口（Windows下需管理员权限，否则可能无法获取描述）
            ports = list_ports.comports()
            if not ports:
                print("未检测到任何串口设备，请检查STM32是否连接！")
                return None

            # 遍历串口，检查描述是否包含目标关键词
            for port in ports:
                # 提取串口描述（Windows下描述格式如："USB-SERIAL CH340 (COM3)"）
                serial_description = port.description.upper() if port.description else ""
                # 检查描述是否包含目标关键词（不区分大小写）
                if any(keyword.upper() in serial_description for keyword in self.target_serial_keywords):
                    print(f"自动检测到STM32串口: {port.device}（描述：{port.description}）")
                    return port.device

            # 未找到符合条件的串口
            print("未找到匹配的STM32串口，请手动指定！")
            return None
        except Exception as e:
            print(f"串口检测失败：{e}")
            return None

    def connect_serial(self) -> bool:
        """连接到STM32单片机的串口（自动检测或手动指定）"""
        # 1. 自动检测串口（若未指定）
        if not self.serial_port:
            self.serial_port = self._find_stm32_port()
            if not self.serial_port:
                return False  # 自动检测失败，返回 False

        # 2. 尝试连接串口
        try:
            self.ser = serial.Serial(
                port=self.serial_port,
                baudrate=self.baud_rate,
                timeout=0.1,         # 读取超时时间（秒）
                bytesize=serial.EIGHTBITS,  # 8位数据位
                parity=serial.PARITY_NONE,   # 无校验位
                stopbits=1   # 1位停止位
            )
            # 检查串口是否成功打开
            if self.ser.is_open:
                print(f"串口连接成功：{self.serial_port}（波特率：{self.baud_rate}）")
                return True
            else:
                print(f"串口无法打开：{self.serial_port}")
                return False
        except Exception as e:
            print(f"串口连接失败：{e}（请检查串口是否被其他程序占用！）")
            return False

    # ------------------------------
    # 网络连接管理（保持原逻辑，无需修改）
    # ------------------------------
    def connect_server(self) -> bool:
        """连接到服务器并启动接收线程"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(10)  # 连接超时设为10秒
            print(f"正在连接服务器 {self.net_host}:{self.net_port}...")
            self.socket.connect((self.net_host, self.net_port))
            self.connected = True
            print(f"服务器连接成功：{self.net_host}:{self.net_port}")

            # 启动网络接收线程（守护线程，程序退出时自动关闭）
            net_thread = threading.Thread(target=self._network_receive_loop, daemon=True)
            net_thread.start()
            return True
        except Exception as e:
            print(f"服务器连接失败：{e}")
            return False

    def _network_receive_loop(self) -> None:
        """持续接收服务器数据并解析"""
        while self.connected:
            try:
                # 接收服务器数据（1024字节缓冲区）
                data = self.socket.recv(1024)
                if not data:  # 服务器断开连接（收到空数据）
                    print("服务器连接已断开！")
                    self.connected = False
                    break
                # 将数据添加到缓冲区（处理粘包问题）
                self.read_buffer.extend(data)
                # 解析缓冲区中的完整数据包
                self._parse_network_data()
            except Exception as e:
                if self.connected:
                    print(f"网络接收错误：{e}")
                break

    # ------------------------------
    # 网络数据解析（保持原逻辑，无需修改）
    # ------------------------------
    def _parse_network_data(self) -> None:
        """解析网络缓冲区中的数据（按服务器协议格式：2字节长度+协议名称+数据）"""
        while len(self.read_buffer) >= 2:  # 至少需要2字节长度字段
            # 1. 解析2字节总长度（little-endian：低字节在前）
            total_length = (self.read_buffer[1] << 8) | self.read_buffer[0]
            # 检查缓冲区数据是否足够（2字节长度+总长度数据）
            if len(self.read_buffer) < 2 + total_length:
                break  # 数据不足，等待后续包

            # 2. 提取完整数据包（跳过2字节长度头）
            packet_data = self.read_buffer[2: 2 + total_length]
            # 移除缓冲区中已处理的数据（避免重复解析）
            self.read_buffer = self.read_buffer[2 + total_length:]

            # 3. 解析协议名称（格式：2字节名称长度+UTF8名称）
            proto_name, name_len = self._decode_proto_name(packet_data)
            if not proto_name:
                print("警告：协议名称解析失败，跳过无效包！")
                continue

            # 4. 处理MsgRotation协议（只关注角度数据）
            if proto_name == "MsgRotation":
                self._handle_rotation_data(packet_data[name_len:])

    def _decode_proto_name(self, packet_data: bytearray) -> Tuple[Optional[str], int]:
        """解析协议名称（格式：2字节长度+UTF8字符串）"""
        if len(packet_data) < 2:
            return None, 0  # 数据不足，无法解析
        # 解析2字节名称长度（little-endian）
        name_len = (packet_data[1] << 8) | packet_data[0]
        if name_len <= 0 or len(packet_data) < 2 + name_len:
            return None, 0  # 长度无效或数据不足
        try:
            # 解码协议名称（UTF8格式）
            proto_name = packet_data[2:2 + name_len].decode('utf-8')
            return proto_name, 2 + name_len  # 返回协议名称和占用字节数
        except UnicodeDecodeError:
            return None, 0  # 解码失败

    # ------------------------------
    # 角度数据处理（保持原逻辑，无需修改）
    # ------------------------------
    def _handle_rotation_data(self, body_bytes: bytes) -> None:
        """处理服务器发送的角度数据（MsgRotation协议）"""
        try:
            # 解析JSON数据（格式如：{"rotation": 10, "protoName": "MsgRotation"}）
            body_json = json.loads(body_bytes.decode('utf-8'))
            # 提取角度值（默认0度）
            angle = body_json.get('rotation', 0)
            # 限制角度范围为0-180度（匹配STM32舵机控制范围）
            angle = self._clamp_angle(angle)
            print(f"收到角度数据：{angle}°（原始值：{body_json.get('rotation')}）")

            # 避免重复发送相同角度（减少串口冗余数据）
            if angle != self.last_angle:
                self._send_to_stm32(angle)
                self.last_angle = angle  # 更新缓存的上次发送角度
        except json.JSONDecodeError:
            print(f"角度数据JSON解析失败：{body_bytes}")
        except Exception as e:
            print(f"角度数据处理错误：{e}")

    def _clamp_angle(self, angle: float) -> int:
        """将角度限制在0-180°范围（四舍五入为整数）"""
        return max(0, min(180, int(round(angle))))

    # ------------------------------
    # 串口数据发送（保持原逻辑，无需修改）
    # ------------------------------
    def _send_to_stm32(self, angle: int) -> bool:
        """
        将角度数据转换为STM32协议格式并发送
        协议格式：包头(0xFF) + 3字节角度数据（大端模式） + 包尾(0xFE)
        示例：角度10° → 0xFF 0x00 0x00 0x0A 0xFE
        """
        if not self.ser or not self.ser.is_open:
            print("串口未连接，无法发送数据！")
            return False

        try:
            # 1. 将角度值（0-180）映射到3字节十六进制（0x000000-0xFFFFFF）
            # 映射公式：hex_value = angle * (0xFFFFFF / 180) → 0-180对应0-16777215
            hex_value = int(angle * (0xFFFFFF / 180))
            # 2. 拆分3字节数据（大端模式：高位在前，如0x123456 → 0x12（高8位）、0x34（中8位）、0x56（低8位））
            byte_high = (hex_value >> 16) & 0xFF  # 高8位
            byte_mid = (hex_value >> 8) & 0xFF   # 中8位
            byte_low = hex_value & 0xFF          # 低8位
            # 3. 构建数据包（包头+数据+包尾）
            packet = bytes([0xFF, byte_high, byte_mid, byte_low, 0xFE])
            # 4. 发送数据包（Windows下串口发送需确认波特率、数据位等参数匹配）
            self.ser.write(packet)
            print(f"发送到STM32：角度={angle}°，数据包={packet.hex().upper()}")
            return True
        except Exception as e:
            print(f"串口发送失败：{e}")
            return False

    # ------------------------------
    # 主程序控制（保持原逻辑，无需修改）
    # ------------------------------
    def start(self) -> None:
        """启动客户端（连接串口→连接服务器→循环运行）"""
        # 1. 连接串口（自动检测或手动指定）
        if not self.connect_serial():
            print("串口连接失败，程序退出！")
            return

        # 2. 连接服务器（需先连接串口，避免服务器连接成功后串口无法使用）
        if not self.connect_server():
            self.ser.close()  # 关闭已连接的串口
            print("服务器连接失败，程序退出！")
            return

        # 3. 循环运行（保持程序不退出，直到用户中断或服务器断开）
        try:
            while self.connected and self.ser.is_open:
                time.sleep(1)  # 避免CPU占用过高
        except KeyboardInterrupt:
            print("\n用户中断程序！")
        finally:
            self.stop()  # 停止客户端（关闭串口和服务器连接）

    def stop(self) -> None:
        """停止客户端（关闭串口和服务器连接）"""
        # 断开服务器连接
        self.connected = False
        if self.socket:
            try:
                self.socket.close()
            except Exception:
                pass

        # 关闭串口连接
        if self.ser and self.ser.is_open:
            self.ser.close()
            print("串口已关闭！")

        print("程序已退出！")


# ------------------------------
# 程序入口（保持原逻辑，无需修改）
# ------------------------------
if __name__ == "__main__":
    # 手动指定串口（如果自动检测失败），示例：
    # Windows: serial_port="COM3"（需替换为实际串口）
    client = RotationClient(
        net_host='60.215.128.110',    # 服务器IP（需替换为实际IP）
        net_port=43195,               # 服务器端口（需替换为实际端口）
        serial_port="COM12",             # None表示自动检测，若失败请手动指定
        baud_rate=9600                # 串口波特率（需与STM32程序匹配）
    )
    client.start()