using UnityEngine;
using System;
using System.Collections;
public class Class31 : MonoBehaviour
{

    public void Start()
    {
        string instruction = "up move 5 and down run 10 and left move 5";
        InstructionHandler handler = new InstructionHandler();
        handler.Handle(instruction);
        string outString;
        outString = handler.Output();
        Debug.Log(outString);
    }
}

//抽象表达式
public abstract class AbstractNode
{
    public abstract string Interpret();
}

//And解释：非终结符表达式
public class AndNode : AbstractNode
{

    private AbstractNode left; //And的左表达式
    private AbstractNode right; //And的右表达式

    public AndNode(AbstractNode left, AbstractNode right)
    {
        this.left = left;
        this.right = right;
    }

    //And表达式解释操作
    public override string Interpret()
    {
        return left.Interpret() + ", 再" + right.Interpret();
    }
}

//简单句子解释：非终结符表达式
public class SentenceNode : AbstractNode
{
    private AbstractNode direction;
    private AbstractNode action;
    private AbstractNode distance;

    public SentenceNode(AbstractNode direction, AbstractNode action, AbstractNode distance)
    {
        this.direction = direction;
        this.action = action;
        this.distance = distance;
    }

    //简单句子的解释操作
    public override string Interpret()
    {
        return direction.Interpret() + action.Interpret() + distance.Interpret();
    }
}

//方向解释：终结符表达式
public class DirectionNode : AbstractNode
{
    private string direction;

    public DirectionNode(string direction)
    {
        this.direction = direction;
    }

    //方向表达式的解释操作
    public override string Interpret()
    {
        if (direction.Equals("up", StringComparison.CurrentCultureIgnoreCase))
        {
            return "向上";
        }
        else if (direction.Equals("down", StringComparison.CurrentCultureIgnoreCase))
        {
            return "向下";
        }
        else if (direction.Equals("left", StringComparison.CurrentCultureIgnoreCase))
        {
            return "向左";
        }
        else if (direction.Equals("right", StringComparison.CurrentCultureIgnoreCase))
        {
            return "向右";
        }
        else
        {
            return "无效指令";
        }
    }
}

//动作解释：终结符表达式
public class ActionNode : AbstractNode
{
    private string action;

    public ActionNode(string action)
    {
        this.action = action;
    }

    //动作（移动方式）表达式的解释操作
    public override string Interpret()
    {
        if (action.Equals("move", StringComparison.CurrentCultureIgnoreCase))
        {
            return "移动";
        }
        else if (action.Equals("run", StringComparison.CurrentCultureIgnoreCase))
        {
            return "快速移动";
        }
        else
        {
            return "无效指令";
        }
    }
}

//距离解释：终结符表达式
public class DistanceNode : AbstractNode
{

    private string distance;

    public DistanceNode(string distance)
    {
        this.distance = distance;
    }

    //距离表达式的解释操作
    public override string Interpret()
    {
        return this.distance;
    }
}

//指令处理类：工具类
public class InstructionHandler
{
    private AbstractNode node;

    public void Handle(string instruction)
    {
        AbstractNode left = null, right = null;
        AbstractNode direction = null, action = null, distance = null;
        Stack stack = new Stack(); //声明一个栈对象用于存储抽象语法树
        string[] words = instruction.Split(' '); //以空格分隔指令字符串
        for (int i = 0; i < words.Length; i++)
        {
            // 本实例采用栈的方式来处理指令，如果遇到“and”，
            // 则将其后的三个单词作为三个终结符表达式连成一个简单句子SentenceNode,
            // 作为“and”的右表达式，而将从栈顶弹出的表达式作为“and”的左表达式，
            // 最后将新的“and”表达式压入栈中。    		        
            if (words[i].Equals("and", StringComparison.CurrentCultureIgnoreCase))
            {
                left = (AbstractNode)stack.Pop(); //弹出栈顶表达式作为左表达式
                string word1 = words[++i];
                direction = new DirectionNode(word1);
                string word2 = words[++i];
                action = new ActionNode(word2);
                string word3 = words[++i];
                distance = new DistanceNode(word3);
                right = new SentenceNode(direction, action, distance); //右表达式
                stack.Push(new AndNode(left, right)); //将新表达式压入栈中
            }
            //如果是从头开始进行解释，则将前三个单词组成一个简单句子SentenceNode并将该句子压入栈中
            else
            {
                string word1 = words[i];
                direction = new DirectionNode(word1);
                string word2 = words[++i];
                action = new ActionNode(word2);
                string word3 = words[++i];
                distance = new DistanceNode(word3);
                left = new SentenceNode(direction, action, distance);
                stack.Push(left); //将新表达式压入栈中
            }
        }
        this.node = (AbstractNode)stack.Pop(); //将全部表达式从栈中弹出
    }

    public string Output()
    {
        string result = node.Interpret(); //解释表达式
        return result;
    }

}
