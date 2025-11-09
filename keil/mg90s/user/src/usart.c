#include "usart.h"

void Usart2_Init(u32 bp)
{
	NVIC_InitTypeDef NVIC_InitStructure;
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;

	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA, ENABLE);	// GPIOA时钟
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART2,ENABLE); //串口2时钟使能

 	USART_DeInit(USART2);  //复位串口2
	//USART2_TX   
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2; 
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
    GPIO_Init(GPIOA, &GPIO_InitStructure); 
   
    //USART2_RX	  
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
    GPIO_Init(GPIOA, &GPIO_InitStructure);  
	
	USART_InitStructure.USART_BaudRate = bp;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式
  
	USART_Init(USART2, &USART_InitStructure); //初始化串口2
  
	USART_Cmd(USART2, ENABLE);                    //使能串口 
	
	//使能接收中断
	USART_ITConfig(USART2, USART_IT_RXNE, ENABLE);//开启中断   
	
	//设置中断优先级
	NVIC_InitStructure.NVIC_IRQChannel = USART2_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=2 ;//抢占优先级3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器

}

	char USART_flag;
	char USART_data[21];
	int i=0;
	int USART_Ready=0;//数据接收完成标志

//串口中断
void USART2_IRQHandler(void)
{
	if(USART_GetITStatus(USART2,USART_IT_RXNE))//接收中断标志位
	{
		USART_flag = USART_ReceiveData(USART2);
		if(USART_flag==0x01)//检测包头
		{
		USART_Ready=1;
		}
		if(USART_Ready==1)
		{
		USART_data[i]=USART_flag;
		i++;
		if(i==20)//捕捉完成21个字节数据
		{
		USART_Ready=0;
		i=0;	
			}
		}
  USART_ClearITPendingBit(USART2,USART_IT_RXNE);//清除中断标志位
	}
}
//串口1发送一字节
void Send_Byte(uint8_t data)
{
	USART_SendData(USART2,data);
	while(!USART_GetFlagStatus(USART2,USART_FLAG_TXE));//等待发送数据完毕
	
}

//串口1接收一节字

u16 Rece_Byte(void)
{
	while(!USART_GetFlagStatus(USART2,USART_FLAG_RXNE))//等待接收数据完毕
	{
	}
	return USART_ReceiveData(USART2);
}

//回显函数
void Data_Echo(void)
{
	uint16_t date=0;
	date=Rece_Byte();
	Send_Byte(date);
}

//函数功能：printf重定向
int fputc(int c, FILE * stream)
{
	Send_Byte(c);
	return c;
}

