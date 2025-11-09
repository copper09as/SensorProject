#include "time1.h"

void Tim3_Init(void)
{
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3,ENABLE);//打开TIM时钟
	
	//配置TIM1时基单元
	TIM_TimeBaseInitTypeDef TIM_TimeBaseInitStruct={0};
	TIM_TimeBaseInitStruct.TIM_CounterMode = TIM_CounterMode_Up;//向上计数
	TIM_TimeBaseInitStruct.TIM_Period = 1000;//重装载值
	TIM_TimeBaseInitStruct.TIM_Prescaler = 7200 - 1;//预分频数
	TIM_TimeBaseInit(TIM3,&TIM_TimeBaseInitStruct);

	TIM_ITConfig(TIM3,TIM_IT_Update, ENABLE);//使能更新中断
	
	//TIM1中优先限配置
	NVIC_InitTypeDef NVIC_InitStruct;
	NVIC_InitStruct.NVIC_IRQChannel=TIM3_IRQn;//配置TIM1中断源
	NVIC_InitStruct.NVIC_IRQChannelCmd=ENABLE;//中断使能
	NVIC_InitStruct.NVIC_IRQChannelPreemptionPriority=0;//抢断优先级
	NVIC_InitStruct.NVIC_IRQChannelSubPriority=2;	//子优先级
	NVIC_Init(&NVIC_InitStruct);//初始化配置NVIC（中断向量控制寄存器）
	
	TIM_Cmd(TIM3, ENABLE);//使能定时器
}

u16 time[5]={0};

void TIM3_IRQHandler(void)
{
	if(TIM_GetITStatus(TIM3,TIM_IT_Update) != RESET)
	{
		TIM_ClearITPendingBit(TIM3,TIM_IT_Update);
		time[0]++;
		if(time[0] == 1){

			time[0] = 0;
			delay_ms (1);//延时10ms        
			Send_Byte( 0x01);
			Send_Byte( 0X03);
			Send_Byte( 0x00);
			Send_Byte( 0X00);
			Send_Byte( 0X00);
			Send_Byte( 0X08);
			Send_Byte( 0X44);
			Send_Byte( 0X0C);
		}

	}
}

