#include "main.h"

extern char USART_data[21];//串口数据存储数组

float ph;
int con;
int N;
int P;
int K;
float tem;
float humi;
int ec;
int salt;

u8 bufss1[20];
u8 bufss2[20];
u8 bufss3[20];
u8 bufss4[20];
u8 bufss5[20];
u8 bufss6[20];
u8 bufss7[20];
u8 bufss8[20];

int main(void)
	{

		NVIC_PriorityGroupConfig(NVIC_PriorityGroup_2);//中断组的选择
		Usart2_Init(4800);//串口2初始化
		OLED_Init();//OLED初始化
		Tim3_Init();//定时器初始化
		while(1)
			{
				ph=(USART_data[3]*16+USART_data[4])/10;
				N=(USART_data[5]*16+USART_data[6]);
				P=(USART_data[7]*16+USART_data[8]);
				K=(USART_data[9]*16+USART_data[10]);
	
				tem=(USART_data[9]*16+USART_data[10]);
				humi=(USART_data[9]*16+USART_data[10]);
				ec=(USART_data[9]*16+USART_data[10]);
				salt=(USART_data[9]*16+USART_data[10]);
	
				sprintf((char *)bufss1,"PH:%.1f",ph);
				sprintf((char *)bufss2,"N:%d",N);
				sprintf((char *)bufss3,"P:%d",P);
				sprintf((char *)bufss4,"K:%d",K);
				
				sprintf((char *)bufss5,"TEM:%.1f",tem);
				sprintf((char *)bufss6,"HUMI:%.1f",humi);
				sprintf((char *)bufss7,"EC:%d",ec);
				sprintf((char *)bufss8,"SALT:%d",salt);
				
				Oled_ShowAll(0,0,bufss1);//显示中英字符串
				Oled_ShowAll(2,0,bufss2);//显示中英字符串
				Oled_ShowAll(4,0,bufss3);//显示中英字符串
				Oled_ShowAll(6,0,bufss4);//显示中英字符串

				Oled_ShowAll(0,0,bufss5);//显示中英字符串
				Oled_ShowAll(2,0,bufss6);//显示中英字符串
				Oled_ShowAll(4,0,bufss7);//显示中英字符串
				Oled_ShowAll(6,0,bufss8);//显示中英字符串
			}
	}

