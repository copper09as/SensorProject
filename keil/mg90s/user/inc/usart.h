#ifndef _USART_H_
#define _USART_H_

#include "includes.h"





void Usart2_Init(u32 bp);
void Send_Byte(uint8_t data);
u16 Rece_Byte(void);
void Data_Echo(void);




#endif

