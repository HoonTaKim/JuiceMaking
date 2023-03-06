using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FRUITCOLOR
{
    DEFAULT,//예외처리&초기화용 enum.
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    PURPLE,
    PINK,
    WHITE
}


public class JuiceColor : MonoBehaviour
{
    [SerializeField] private Image juice = null;

    private void Awake()
    {
        juice.color = Color.white;
    }

    // 색상 지정
    public static Color singleColor(FRUITCOLOR color)
    {
        Debug.Log(color);

        Color singleColor = Color.black;

        switch(color)
        {
            case FRUITCOLOR.RED:
                singleColor = Color.red;
                break;
            case FRUITCOLOR.ORANGE:
                singleColor = new Color(255, 165, 0);
                break;
            case FRUITCOLOR.YELLOW:
                singleColor = Color.yellow;
                break;
            case FRUITCOLOR.GREEN:
                singleColor = Color.green;
                break;
            case FRUITCOLOR.BLUE:
                singleColor = Color.blue;
                break;
            case FRUITCOLOR.PURPLE:
                singleColor = new Color(139, 0, 255);
                break;
            case FRUITCOLOR.WHITE:
                singleColor = Color.white;
                break;
        }

        return singleColor;
    }

    // 색 섞는 함수
    public static Color mixColor(FRUITCOLOR color1, FRUITCOLOR color2)
    {
        //만약 확인했는데 색이 black으로 나오면 어디선가 오류가 떳다는거임
        Color mixedColor = Color.black;
        //color1 인덱스가 color2보다 높으면 swap
        if((int)color1 > (int)color2)
        {
            FRUITCOLOR temp = color1;
            color1 = color2;
            color2 = temp;
        }

        switch (color1)
        {
            case FRUITCOLOR.RED:
                switch (color2)
                {
                    case FRUITCOLOR.ORANGE:
                        mixedColor = new Color(229, 41, 23, 255);
                        break;
                    case FRUITCOLOR.YELLOW:
                        mixedColor = new Color(255, 127, 0, 255);
                        break;
                    case FRUITCOLOR.GREEN:
                        mixedColor = new Color(132, 93, 74, 255);
                        break;
                    case FRUITCOLOR.BLUE:
                        mixedColor = new Color(186, 91, 186, 255);
                        break;
                    case FRUITCOLOR.PURPLE:
                        mixedColor = new Color(170, 85, 85, 255);
                        break;
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(255, 125, 125, 255);
                        break;
                }
                break;
            case FRUITCOLOR.ORANGE:
                switch (color2)
                {
                    case FRUITCOLOR.YELLOW:
                        mixedColor = new Color(241, 133, 2, 255);
                        break;
                    case FRUITCOLOR.GREEN:
                        mixedColor = new Color(137, 128, 48, 255);
                        break;
                    case FRUITCOLOR.BLUE:
                        mixedColor = new Color(71, 57, 37, 255);
                        break;
                    case FRUITCOLOR.PURPLE:
                        mixedColor = new Color(147, 77, 53, 255);
                        break;
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(255, 186, 118, 255);
                        break;
                }
                break;
            case FRUITCOLOR.YELLOW:
                switch (color2)
                {
                    case FRUITCOLOR.GREEN:
                        mixedColor = new Color(22, 216, 63, 255);
                        break;
                    case FRUITCOLOR.BLUE:
                        mixedColor = new Color(53, 119, 65, 255);
                        break;
                    case FRUITCOLOR.PURPLE:
                        mixedColor = new Color(127, 90, 59, 255);
                        break;
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(255, 225, 128, 255);
                        break;
                }
                break;
            case FRUITCOLOR.GREEN:
                switch (color2)
                {
                    case FRUITCOLOR.BLUE:
                        mixedColor = new Color(41, 122, 97, 255);
                        break;
                    case FRUITCOLOR.PURPLE:
                        mixedColor = new Color(60, 84, 62, 255);
                        break;
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(255, 225, 128, 255);
                        break;
                }
                break;
            case FRUITCOLOR.BLUE:
                switch (color2)
                {
                    case FRUITCOLOR.PURPLE:
                        mixedColor = new Color(68, 42, 221, 255);
                        break;
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(80, 176, 255, 255);
                        break;
                }
                break;
            case FRUITCOLOR.PURPLE:
                switch (color2)
                {
                    case FRUITCOLOR.WHITE:
                        mixedColor = new Color(229, 120, 255, 255);
                        break;
                }
                break;
            default:
                mixedColor = Color.black;
                break;

        }

        mixedColor = mixedColor / 255;
        return mixedColor;
    }
}

