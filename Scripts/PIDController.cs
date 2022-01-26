using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * From: https://gist.github.com/bradley219/5373998
 * 
 *  use example:
 *  PIDControllerFloat c = new PIDControllerFloat(-4.0f, 4.0f, 0.5f, 0.1f, 0.001f);
 *  float val = 20.0f;
 *  for (int i = 0; i < 20; ++i) {
 *      float inc = c.Calculate(10.0f, val, 1.0f / 50.0f);
 *      Debug.Log("val: " + val + ", inc: " + inc);
 *      val += inc;
 *  }
 */


/*
 * Float PID controller
 */
public class PIDControllerFloat
{
    private readonly float m_Min;
    private readonly float m_Max;
    private readonly float m_Kp;
    private readonly float m_Ki;
    private readonly float m_Kd;

    private float m_Integral;
    private float m_PreviousError;

    public PIDControllerFloat(float min, float max, float Kp, float Ki, float Kd) {
        m_Min = min;
        m_Max = max;
        m_Kp = Kp;
        m_Ki = Ki;
        m_Kd = Kd;

        m_Integral = 0.0f;
        m_PreviousError = 0.0f;
    }


    public float Calculate(float setpoint, float currentValue, float deltaTime) {

        // Calculate error
        float error = setpoint - currentValue;

        // Proportional term
        float POut = m_Kp * error;

        // Integral term
        m_Integral += error * deltaTime;
        float IOut = m_Ki * m_Integral;

        // Derivate term
        float derivate = (error - m_PreviousError) / deltaTime;
        float DOut = m_Kd * derivate;

        // Calculate total output
        float output = POut + IOut + DOut;
        output = Mathf.Clamp(output, m_Min, m_Max);

        // Save error
        m_PreviousError = error;

        return output;
    } 
}


/**
 * Vector3 PID controller
 */
public class PIDControllerVector3
{
    private readonly Vector3 m_Min;
    private readonly Vector3 m_Max;
    private readonly float m_Kp;
    private readonly float m_Ki;
    private readonly float m_Kd;

    private Vector3 m_Integral;
    private Vector3 m_PreviousError;

    public PIDControllerVector3(Vector3 min, Vector3 max, float Kp, float Ki, float Kd) {
        m_Min = min;
        m_Max = max;
        m_Kp = Kp;
        m_Ki = Ki;
        m_Kd = Kd;

        m_Integral = Vector3.zero;
        m_PreviousError = Vector3.zero;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="setpoint"></param>
    /// <param name="currentValue"></param>
    /// <param name="dampening">Value between 1.0f (no dampening) and 0.0f (output will be Vector3.zero)</param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public Vector3 Calculate(Vector3 setpoint, Vector3 currentValue, float dampening, float deltaTime) {

        // Calculate error
        Vector3 error = setpoint - currentValue;

        // Proportional term
        Vector3 POut = m_Kp * error;

        // Integral term
        m_Integral += error * deltaTime;
        Vector3 IOut = m_Ki * m_Integral;

        // Derivate term
        Vector3 derivate = (error - m_PreviousError) / deltaTime;
        Vector3 DOut = m_Kd * derivate;

        // Calculate total output
        Vector3 output = POut + IOut + DOut;
        output = new Vector3(
            Mathf.Clamp(output.x, m_Min.x * dampening, m_Max.x * dampening),
            Mathf.Clamp(output.y, m_Min.y * dampening, m_Max.y * dampening),
            Mathf.Clamp(output.z, m_Min.z * dampening, m_Max.z * dampening)
        );

        // Save error
        m_PreviousError = error;

        return output;
    }
}


/**
 * Vector3 PID controller
 */
public class PIDControllerQuaternion
{
    private readonly Vector3 m_Min;
    private readonly Vector3 m_Max;
    private readonly float m_Kp;
    private readonly float m_Ki;
    private readonly float m_Kd;

    private Vector3 m_Integral;
    private Vector3 m_PreviousError;

    public PIDControllerQuaternion(Vector3 min, Vector3 max, float Kp, float Ki, float Kd) {
        m_Min = min;
        m_Max = max;
        m_Kp = Kp;
        m_Ki = Ki;
        m_Kd = Kd;

        m_Integral = Vector3.zero;
        m_PreviousError = Vector3.zero;
    }


    public Quaternion Calculate(Vector3 setpoint, Vector3 currentValue, float deltaTime) {

        // Calculate error
        Vector3 error = setpoint - currentValue;

        // Proportional term
        Vector3 POut = m_Kp * error;

        // Integral term
        m_Integral += error * deltaTime;
        Vector3 IOut = m_Ki * m_Integral;

        // Derivate term
        Vector3 derivate = (error - m_PreviousError) / deltaTime;
        Vector3 DOut = m_Kd * derivate;

        // Calculate total output
        Vector3 output = POut + IOut + DOut;
        output = new Vector3(
            Mathf.Clamp(output.x, m_Min.x, m_Max.x),
            Mathf.Clamp(output.y, m_Min.y, m_Max.y),
            Mathf.Clamp(output.z, m_Min.z, m_Max.z)
        );

        // Save error
        m_PreviousError = error;

        return Quaternion.identity; // output;
    }
}