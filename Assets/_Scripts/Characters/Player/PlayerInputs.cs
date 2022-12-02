using System;
using UnityEngine;

public class PlayerInputs
{
    public Action<Vector2> e_LeftStickValue;
    public Action          e_OnJumpPressed;   

    public void VirtualUpdate()
    {        
       

        if (Input.GetButtonDown("Jump"))       
            e_OnJumpPressed?.Invoke();          
    } 

    public void VirtualFixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));        
                input = Vector2.ClampMagnitude(input, 1f);   

        e_LeftStickValue?.Invoke(input);
    }
    
    
   
}
