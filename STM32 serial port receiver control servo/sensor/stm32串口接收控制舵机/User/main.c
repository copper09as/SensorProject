#include "stm32f10x.h"                  // Device header
#include "Delay.h"
#include "Serial.h"
#include "Servo.h"                      

int main(void)
{
	/*模块初始化*/
	Servo_Init();
	Serial_Init();		//串口初始化
	
	// 初始角度设置为0度
	uint16_t angle = 0;
	Servo_SetAngle_X(angle);
	
	while (1)
	{
		if (Serial_GetRxFlag() == 1)	//如果接收到数据包
		{
			// 获取转换后的角度值（0-180度）
			angle = Serial_GetAngle();
			
			// 控制舵机转动到指定角度
			Servo_SetAngle_X(angle);
			
			// 可以添加延时让舵机有足够时间转动
			Delay_ms(100);
		}
	}
}