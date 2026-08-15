#include "pch.h"
#include "MathLib.h"

extern "C" {
    __declspec(dllexport) double ProcessTemperature(double input)
    {
        return input * 2.0 + 5.0;
    }

    __declspec(dllexport) int AddTwoIntegers(int a, int b)
    {
        return a + b;
    }

    __declspec(dllexport) double GetPi()
    {
        return 3.141592653589793;
    }
}