using System;
using System.Collections;
using System.Collections.Generic;

public class Program {

    public static void Func(String[] v) {
        Console.WriteLine("-----");
        foreach(String s in v) {
            Console.WriteLine(s);
        }
    }

    public static void Main() {
        String input = "Hello Cruel World";
        String[] v = input.Split();
        for(int i=0;i<v.Length;i++) {
            Console.WriteLine(v[i]);
        }
        Console.WriteLine("-----");
        foreach(String s in v) {
            Console.WriteLine(s);
        }
        Func(v);
    }
}