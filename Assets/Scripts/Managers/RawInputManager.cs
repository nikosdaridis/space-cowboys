using UnityEngine;

// Get Real Input from Keyboard and Controller and convert it to Virtual Input
public static class RawInputManager
{
    // Player 1
    public static bool P1Left()
    {
        float axisInput = Input.GetAxis("P1Left_J");
        bool axisLeft = false;

        if (axisInput < 0.0f)
            axisLeft = true;

        return Input.GetButton("P1Left_K") || axisLeft;
    }

    public static bool P1Right()
    {
        float axisInput = Input.GetAxis("P1Right_J");
        bool axisRight = false;

        if (axisInput > 0.0f)
            axisRight = true;

        return Input.GetButton("P1Right_K") || axisRight;
    }

    public static bool P1Jump()
    {
        return Input.GetButtonDown("P1Jump_KJ");
    }

    public static bool P1Drop()
    {
        float axisInput = Input.GetAxis("P1Drop_J");
        bool axisDrop = false;

        if (axisInput > 0.0f)
            axisDrop = true;

        return Input.GetButton("P1Drop_K") || axisDrop;
    }

    public static bool P1Shoot()
    {
        return Input.GetButton("P1Shoot_KJ");
    }

    // Player 2
    public static bool P2Left()
    {
        float axisInput = Input.GetAxis("P2Left_J");
        bool axisLeft = false;

        if (axisInput < 0.0f)
            axisLeft = true;

        return Input.GetButton("P2Left_K") || axisLeft;
    }

    public static bool P2Right()
    {
        float axisInput = Input.GetAxis("P2Right_J");
        bool axisRight = false;

        if (axisInput > 0.0f)
            axisRight = true;

        return Input.GetButton("P2Right_K") || axisRight;
    }

    public static bool P2Jump()
    {
        return Input.GetButtonDown("P2Jump_KJ");
    }

    public static bool P2Drop()
    {
        float axisInput = Input.GetAxis("P2Drop_J");
        bool axisDrop = false;

        if (axisInput > 0.0f)
            axisDrop = true;

        return Input.GetButton("P2Drop_K") || axisDrop;
    }

    public static bool P2Shoot()
    {
        return Input.GetButton("P2Shoot_KJ");
    }
}