#include "oled.h"
#include "oledfont.h" 
#include "pic.h"

extern const unsigned char Aciss_8X16[];//字符库
extern const unsigned char indexs[][3];//寻找汉字位置
const unsigned char GB2312[];//汉字库

/*
	@brief			初始化OLED与单片机的IO接口
	@param			无
	@retval			无
 */
static void OLED_GPIO_Init(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//定义一个GPIO_InitTypeDef类型的结构体
	
	RCC_APB2PeriphClockCmd( RCC_APB2Periph_GPIOB, ENABLE);	//打开GPIOC的外设时钟
	
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_12 | GPIO_Pin_13;	//选择控制的引脚
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_OD;	//设置为通用开漏输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;	//设置输出速率为50MHz
	GPIO_Init(GPIOB,&GPIO_InitStructure);	//调用库函数初始化GPIOA
	
	OLED_SCLK_Set();	//设PA5（SCL）为高电平
	OLED_SDIN_Set();	//设PA6（SDA）为高电平
}



/*
	@brief			模拟IIC起始信号
	@param			无
	@retval			无
 */
static void OLED_IIC_Start(void)
{

	OLED_SCLK_Set();	//时钟线置高
	OLED_SDIN_Set();	//信号线置高
	delay_us(1);	//延迟1us
	OLED_SDIN_Clr();	//信号线置低
	delay_us(1);	//延迟1us
	OLED_SCLK_Clr();	//时钟线置低
	delay_us(1);	//延迟1us
}


/*
	@brief			模拟IIC停止信号
	@param			无
	@retval			无
 */
static void OLED_IIC_Stop(void)
{
	OLED_SDIN_Clr();	//信号线置低
	delay_us(1);	//延迟1us
	OLED_SCLK_Set();	//时钟线置高
	delay_us(1);	//延迟1us
	OLED_SDIN_Set();	//信号线置高
	delay_us(1);	//延迟1us
}


/*
	@brief			模拟IIC读取从机应答信号
	@param			无
	@retval			无
 */
static unsigned char IIC_Wait_Ack(void)
{
	unsigned char ack;

	OLED_SCLK_Clr();	//时钟线置低
	delay_us(1);	//延迟1us
	OLED_SDIN_Set();	//信号线置高
	delay_us(1);	//延迟1us
	OLED_SCLK_Set();	//时钟线置高
	delay_us(1);	//延迟1us

	if(OLED_READ_SDIN())	//读取SDA的电平
		ack = IIC_NO_ACK;	//如果为1，则从机没有应答
	else
		ack = IIC_ACK;		//如果为0，则从机应答

	OLED_SCLK_Clr();//时钟线置低
	delay_us(1);	//延迟1us

	return ack;	//返回读取到的应答信息
}

static void Write_IIC_Byte(unsigned char IIC_Byte)
{
	unsigned char i;  //定义变量
	for(i=0;i<8;i++) //for循环8次
	{
		OLED_SCLK_Clr();	//时钟线置低，为传输数据做准备
		delay_us(1);	//延迟1us

		if(IIC_Byte & 0x80)	//读取最高位
		  	OLED_SDIN_Set();//最高位为1
		else
			OLED_SDIN_Clr();	//最高位为0

		IIC_Byte <<= 1;  //数据左移1位
		delay_us(1);	//延迟1us
		OLED_SCLK_Set(); //时钟线置高，产生上升沿，把数据发送出去
		delay_us(1);	//延迟1us
	}
	OLED_SCLK_Clr();	//时钟线置低
	delay_us(1);	//延迟1us

	while(IIC_Wait_Ack());	//从机应答
}

/*
	@brief			IIC写入命令
	@param			IIC_Command：写入的命令
	@retval			无
 */
static void Oled_Send_Cmd(unsigned char IIC_Command)
{
   OLED_IIC_Start();
   Write_IIC_Byte(0x78);//写入从机地址，SD0 = 0
   Write_IIC_Byte(0x00);//写入命令
   Write_IIC_Byte(IIC_Command);//数据
   OLED_IIC_Stop();  //发送停止信号
}


/*
	@brief			IIC写入数据
	@param			IIC_Data：数据
	@retval			无
 */
static void Oled_Send_Data(unsigned char IIC_Data)
{
   OLED_IIC_Start();
   Write_IIC_Byte(0x78);	//写入从机地址，SD0 = 0
   Write_IIC_Byte(0x40);	//写入数据
   Write_IIC_Byte(IIC_Data);//数据
   OLED_IIC_Stop();		//发送停止信号
}



/*
	@brief			开显示
	@param			无
	@retval			无
 */ 
void OLED_Display_On(void)
{
	Oled_Send_Cmd(0X8D);  //设置OLED电荷泵
	Oled_Send_Cmd(0X14);  //使能，开
	Oled_Send_Cmd(0XAF);  //开显示
}


/*
	@brief			关显示
	@param			无
	@retval			无
 */  
void OLED_Display_Off(void)
{
	Oled_Send_Cmd(0XAE);  //关显示
	Oled_Send_Cmd(0X8D);  //设置OLED电荷泵
	Oled_Send_Cmd(0X10);  //失能，关
}		   			 


/*
	@brief			清屏
	@param			无
	@retval			无
 */	  
void OLED_Clear(u8 data)  
{  
	u8 i,j;
	for(i=0;i<8;i++)
	{
		Oled_Send_Cmd(0xB0+i);//发送页起始地址
		Oled_Send_Cmd(0x00);//发送列地址低四位
		Oled_Send_Cmd(0x10);//发送列地址高四位
		for(j=0;j<128;j++)
		{
		 Oled_Send_Data(data);
		}
	}
}



void OLED_Init(void)
{
	OLED_GPIO_Init();	//GPIO口初始化
 
	delay_ms(200);	//延迟，由于单片机上电初始化比OLED快，所以必须加上延迟，等待OLED上复位完成

	Oled_Send_Cmd(0xAE);	//关闭显示

	Oled_Send_Cmd(0x00);	//设置低列地址
	Oled_Send_Cmd(0x10);	//设置高列地址
	Oled_Send_Cmd(0x40);	//设置起始行地址
	Oled_Send_Cmd(0xB0);	//设置页地址

	Oled_Send_Cmd(0x81); 	// 对比度设置，可设置亮度
	Oled_Send_Cmd(0xFF);	//  265  

	Oled_Send_Cmd(0xA1);	//设置段（SEG）的起始映射地址；column的127地址是SEG0的地址
	Oled_Send_Cmd(0xA6);	//正常显示；0xa7逆显示

	Oled_Send_Cmd(0xA8);	//设置驱动路数（16~64）
	Oled_Send_Cmd(0x3F);	//64duty
	
	Oled_Send_Cmd(0xC8);	//重映射模式，COM[N-1]~COM0扫描

	Oled_Send_Cmd(0xD3);	//设置显示偏移
	Oled_Send_Cmd(0x00);	//无偏移
	
	Oled_Send_Cmd(0xD5);	//设置震荡器分频
	Oled_Send_Cmd(0x80);	//使用默认值
	
	Oled_Send_Cmd(0xD9);	//设置 Pre-Charge Period
	Oled_Send_Cmd(0xF1);	//使用官方推荐值
	
	Oled_Send_Cmd(0xDA);	//设置 com pin configuartion
	Oled_Send_Cmd(0x12);	//使用默认值
	
	Oled_Send_Cmd(0xDB);	//设置 Vcomh，可调节亮度（默认）
	Oled_Send_Cmd(0x40);	////使用官方推荐值
	
	Oled_Send_Cmd(0x8D);	//设置OLED电荷泵
	Oled_Send_Cmd(0x14);	//开显示
	
	Oled_Send_Cmd(0xAF);	//开启OLED面板显示
	OLED_Clear(0);        //清屏
//	Set_Postion(0,0); 	 //设置数据写入的起始行、列
//	OLED_Clear(0);
}  
 /***********************
函数名：Set_Postion
函数功能：
形参：
u8 page 页 
u8 col 列
返回值：void
函数说明：
页地址：0xb0
列低位地址:0x00
列高位地址:0x10
************************/
void Set_Postion(u8 page,u8 col)
{
	Oled_Send_Cmd(0xB0+page);//发送页起始地址
	Oled_Send_Cmd(0x00+(col&0x0f));//发送列地址低四位
	Oled_Send_Cmd(0x10+((col&0xf0)>>4));//发送列地址高四位
}  	     	  

/***********************
函数名：Oled_ShowPic
函数功能：OLED显示图片
形参：
u8 page 页     
u8 col 列     
u8 height  图片高度
u8 width   图片宽度
u8 *pic    图片数据
返回值：void
64*64
************************/

void Oled_ShowPic(u8 page,u8 col,u8 height,u8 width,u8 *pic)
{
	u8 i,j;
	for(i=0;i<height/8;i++)//活得页范围
	{
		Set_Postion(page+i,col);//定义框架
		for(j=0;j<width;j++)
		{
			Oled_Send_Data(pic[j+i*width]);//往框架输入数据，以列方式
		}
	}

}
//清空某字符
void Oled_clear(u8 page,u8 col,u8 height,u8 width)
{
	u8 i,j;
	for(i=0;i<height/8;i++)//活得页范围
	{
		Set_Postion(page+i,col);//定义框架
		for(j=0;j<width;j++)
		{
			Oled_Send_Data(0x00);//往框架输入数据，以列方式
		}
	}

}


/***********************
函数名：Oled_ShowChar
函数功能：OLED显示字符
形参：
u8 page 页     
u8 col 列     

u8 pic    字符数据
返回值：void
8*16
************************/


void Oled_ShowChar(u8 page,u8 col,u8 eng)
{
	u8 i,j,Char;
	Char=eng-' ';//获取字符所在位置
	for(i=0;i<2;i++)
	{
		Set_Postion(page+i,col);
		for(j=0;j<8;j++)
		{
			Oled_Send_Data(Aciss_8X16[j+Char*16+i*8]);
		}
	}
}
/***********************
函数名：Oled_Showstring
函数功能：OLED显示字符串
形参：
u8 page 页  2
u8 col 列    8 

u8 *str    字符串
返回值：void
8*16
************************/
void Oled_Showstring(u8 page,u8 col,u8*str)
{

	while(*str !='\0')
	{
		if(col>=128)
		{
			col=0;
			page +=2;

		}
		if(page>=8)
		{
			page=0;
		}
		Oled_ShowChar(page,col,*str);
		str++;//字符递加
		col +=8;//每显示完一个字符，列+8				
}
		
}

/***********************
函数名：Oled_ShowChi
函数功能：OLED显示汉字
形参：
u8 page 页     
u8 col 列     
u8 *chi    显示汉字
返回值：void
    16*16  = 32 byte
************************/

void Oled_ShowChi(u8 page,u8 col,u8*chi)
{
	u8 i,j,z;
	for(i=0;i<sizeof(indexs)/sizeof(indexs[0]);i++)
	{
		if(*chi == indexs[i][0]&&*(chi+1)==indexs[i][1])//比较汉字在第几个位置
		{
			break;
		}
	}
	for(j=0;j<2;j++)
	{
		Set_Postion(page+j,col);
		for(z=0;z<16;z++)
		{
			Oled_Send_Data(GB2312[z+i*32+j*16]);
		}

	}

}

/***********************
函数名：Oled_ShowAll
函数功能：OLED显示汉英字符串
形参：
u8 page 页     
u8 col 列     
u8 *str    显示汉字
返回值：void
		8*16;
    16*16  = 32 byte
************************/


void Oled_ShowAll(u8 page ,u8 col ,u8*str)
{

	while(*str != '\0')
{
	
	if(*str>127)
	{
		if(col>=120)//放不下字
		{
			page +=2;//
			col=0;

		}
	if(page>=8)
		{
				page=0;
		}
		Oled_ShowChi(page,col,str);
		col +=16;
		str +=2;
	}
	else
	{
		if(col>=128)//放不下字
		{
			page +=2;//
			col=0;

		}
			if(page>=8)
		{
				page=0;
		}
	 Oled_ShowChar(page,col,*str);
		str++;
		col +=8;
	}
	
	
	
	
	}


}

/*
	@brief			OLED滚屏函数，范围0~1页，水平向左
	@param			无
	@retval			无
 */	
void OLED_Scroll(void)
{
	Oled_Send_Cmd(0x2E);	//关闭滚动
	Oled_Send_Cmd(0x27);	//水平向左滚动
	Oled_Send_Cmd(0x00);	//虚拟字节
	Oled_Send_Cmd(0x00);	//起始页 0
	Oled_Send_Cmd(0x00);	//滚动时间间隔
	Oled_Send_Cmd(0x01);	//终止页 1
	Oled_Send_Cmd(0x00);	//虚拟字节
	Oled_Send_Cmd(0xFF);	//虚拟字节
	Oled_Send_Cmd(0x2F);	//开启滚动
}

