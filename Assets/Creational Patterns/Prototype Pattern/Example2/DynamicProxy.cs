using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Runtime.InteropServices;

public class DynamicProxy : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //反射实例
        MyClass myClass =(MyClass)Activator.CreateInstance(typeof(MyClass));  //创建对象实例 object obj=Activator.CreateInstance(type);  //创建对象实例
        ///获取字段必须指定 BindingFlags.Instance 或 BindingFlags.Static，类为static则指定BindingFlags.Instance
        FieldInfo fileInfo = myClass.GetType().GetField("s1",BindingFlags.NonPublic|BindingFlags.Instance);//获取私有字段
        PropertyInfo[] propertyInfos=  myClass.GetType().GetProperties();//获取属性
        MemberInfo[] memberInfos =myClass.GetType().GetMembers();//获取成员变量(字段+属性)
        fileInfo.SetValue(myClass, "s2");//修改字段值
        MethodInfo methodInfo = myClass.GetType().GetMethod("MyProcedure");
        methodInfo.Invoke(methodInfo,null);//实例方法必须依附于对象实例才能执行

        IMyInterface intf = LoggingProxy<IMyInterface>.Create(myClass);
        intf.MyProcedure();//
    }

}
 interface IMyInterface
{
    void MyProcedure();
    void Function();
}

class MyClass : IMyInterface
{
    private string s1;
    public string S1 { get { return s1; } }
    public string ps1;
    public MyClass()
    {
        s1 = "s1";
        ps1 = "ps1";
    }
    public void Function()
    {
        throw new NotImplementedException();
    }

    public void MyProcedure()
    {
        Console.WriteLine("Hello World");
    }
}
/// <summary>
///泛型类加载代理
/// </summary>
/// <typeparam name="T">必须为抽象接口</typeparam>
public class LoggingProxy<T> : RealProxy
{
    private readonly T _instance;

    private LoggingProxy(T instance) : base(typeof(T))
    {
        _instance = instance;
    }
    /// <summary>
    /// //返回 RealProxy 的当前实例的透明代理。
    /// </summary>
    /// <param name="instance">要代理的实例1
    /// </param>
    /// <returns></returns>
    public static T Create(T instance)
    {
        return (T)new LoggingProxy<T>(instance).GetTransparentProxy();
    }

    public override IMessage Invoke(IMessage msg)
    {
        var methodCall = (IMethodCallMessage)msg;
        var method = (MethodInfo)methodCall.MethodBase;
       
        try {
            Console.WriteLine("Before invoke: " + method.Name);//在调用方法前做记录
            var result = method.Invoke(_instance, methodCall.InArgs);
            Console.WriteLine("After invoke: " + method.Name);//在调用方法后做记录
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        } catch (Exception e) {
            Console.WriteLine("Exception: " + e);
            if (e is TargetInvocationException && e.InnerException != null) {
                return new ReturnMessage(e.InnerException, msg as IMethodCallMessage);
            }
            return new ReturnMessage(e, msg as IMethodCallMessage);
        }
    }
}